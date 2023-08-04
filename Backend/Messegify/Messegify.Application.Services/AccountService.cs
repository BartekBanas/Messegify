using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using Messegify.Application.Dtos;
using Messegify.Application.Errors;
using Messegify.Application.Service.Extensions;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Events;
using Messegify.Infrastructure.Error;
using Microsoft.AspNetCore.Http;

namespace Messegify.Application.Services;

public interface IAccountService
{
    Task<AccountDto> GetAccountAsync(Guid accountId);
    Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
    Task RegisterAccountAsync(RegisterAccountDto registerDto);
    Task<string> AuthenticateAsync(LoginDto loginDto);
    Task CreateContactAsync(Guid accountAId, Guid accountBId);
    Task DeleteAccountAsync(Guid accountId);
    Task<IEnumerable<ContactDto>> GetContactsAsync(Guid accountId);
}

public class AccountService : IAccountService
{
    private readonly IRepository<Account> _accountRepository;
    private readonly IRepository<Contact> _contactRepository;
    private readonly IRepository<ChatRoom> _chatroomRepository;

    private readonly IHashingService _hashingService;
    private readonly IValidator<Account> _validator;
    private readonly IValidator<Contact> _contactValidator;
    private readonly IJwtService _jwtService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IMapper _mapper;

    public AccountService(
        IRepository<Account> accountRepository, 
        IRepository<Contact> contactRepository,
        IRepository<ChatRoom> chatroomRepository,
        IHashingService hashingService,
        IValidator<Account> validator,
        IValidator<Contact> contactValidator,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _contactRepository = contactRepository;
        _hashingService = hashingService;
        _validator = validator;
        _contactValidator = contactValidator;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _chatroomRepository = chatroomRepository;
    }

    public async Task<AccountDto> GetAccountAsync(Guid accountId)
    {
        var account = await _accountRepository.GetOneRequiredAsync(accountId);

        var dtos = _mapper.Map<AccountDto>(account);

        return dtos;
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
    {
        var accounts = await _accountRepository.GetAllAsync();

        var dtos = _mapper.Map<IEnumerable<AccountDto>>(accounts);
    
        return dtos;
    }

    public async Task RegisterAccountAsync(RegisterAccountDto registerDto)
    {
        var passwordHash = _hashingService.HashPassword(registerDto.Password);

        var newAccount = new Account()
        {
            Email = registerDto.Email,
            Name = registerDto.Username,
            PasswordHash = passwordHash,
        };
        
        await _validator.ValidateRequiredAsync(newAccount);

        await _accountRepository.CreateAsync(newAccount);

        newAccount.AddDomainEvent(new AccountCreatedDomainEvent(newAccount));
        
        await _accountRepository.SaveChangesAsync();
    }

    public async Task<string> AuthenticateAsync(LoginDto loginDto)
    {
        var foundAccount = await _accountRepository
            .GetOneAsync(account =>
                account.Name == loginDto.UsernameOrEmail || account.Email == loginDto.UsernameOrEmail);

        if (foundAccount == default)
            throw new ForbiddenError();

        if (!_hashingService.VerifyPassword(foundAccount, loginDto.Password))
            throw new ForbiddenError();

        var claims = GenerateClaimsIdentity(foundAccount);

        var token = _jwtService.GenerateSymmetricJwtToken(claims);
        return token;
    }

    public async Task CreateContactAsync(Guid accountAId, Guid accountBId)
    {
        var contacts = await _contactRepository
            .GetAsync(contact => (contact.FirstAccountId == accountAId && contact.SecondAccountId == accountBId)
                                 || (contact.FirstAccountId == accountBId && contact.SecondAccountId == accountAId));

        if (contacts.Any())
        {
            throw new ItemDuplicatedErrorException("Contact already exists");
        }
        
        var newContact = new Contact()
        {
            FirstAccountId = accountAId,
            SecondAccountId = accountBId
        };

        await _contactValidator.ValidateAsync(newContact);

        await _contactRepository.CreateAsync(newContact);
        
        newContact.AddDomainEvent(new ContactCreatedDomainEvent(newContact));
        
        await _contactRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<ContactDto>> GetContactsAsync(Guid accountId)
    {
        var account = await _accountRepository.GetOneAsync(accountId);
        var user = _httpContextAccessor.HttpContext.User ?? throw new NullReferenceException();
        
        var contacts = await _contactRepository
            .GetAsync(contact => contact.FirstAccountId == accountId || contact.SecondAccountId == accountId);

        var dtos = _mapper.Map<IEnumerable<ContactDto>>(contacts);
        
        return dtos;
    }

    public async Task DeleteAccountAsync(Guid accountId)
    {
        var contacts = await GetContactsAsync(accountId);

        foreach (var contact in contacts)
        {
            await _contactRepository.DeleteAsync(contact.Id);
            await _chatroomRepository.DeleteAsync(contact.ContactChatRoomId);
        }
        
        await _accountRepository.DeleteAsync(accountId);

        await _contactRepository.SaveChangesAsync();
    }

    private ClaimsIdentity GenerateClaimsIdentity(Account account)
    {
        return new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.Sid, account.Id.ToString()),
            new(ClaimTypes.PrimarySid, account.Id.ToString()),
            new(ClaimTypes.Name, account.Name),
            new(ClaimTypes.Email, account.Email),
        });
    }
}