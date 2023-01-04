using System.Security.Claims;
using FluentValidation;
using Messegify.Application.Dtos;
using Messegify.Application.Errors;
using Messegify.Application.Service.Extensions;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Events;
using Microsoft.AspNetCore.Identity;

namespace Messegify.Application.Services;

public interface IAccountService
{
    Task RegisterAccountAsync(RegisterAccountDto registerDto);
    Task<string> AuthenticateAsync(LoginDto loginDto);
    public Task ContactAsync(Guid accountAId, Guid accountBId);
    public Task<IEnumerable<Contact>> GetContactsAsync(Guid accountId);
}

public class AccountService : IAccountService
{
    private readonly IRepository<Account> _accountRepository;
    private readonly IRepository<Contact> _contactRepository;
    
    private readonly IHashingService _hashingService;
    private readonly IValidator<Account> _validator;
    private readonly IJwtService _jwtService;

    public AccountService(
        IRepository<Account> accountRepository, 
        IHashingService hashingService,
        IValidator<Account> validator, 
        IJwtService jwtService, 
        IRepository<Contact> contactRepository)
    {
        _accountRepository = accountRepository;
        _hashingService = hashingService;
        _validator = validator;
        _jwtService = jwtService;
        _contactRepository = contactRepository;
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

    public async Task ContactAsync(Guid accountAId, Guid accountBId)
    {
        var newEntity = new Contact()
        {
            FirstAccountId = accountAId,
            SecondAccountId = accountBId
        };

        await _contactRepository.CreateAsync(newEntity);
        
        newEntity.AddDomainEvent(new ContactCreatedDomainEvent(newEntity));
        
        await _contactRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<Contact>> GetContactsAsync(Guid accountId)
    {
        var contacts = await _contactRepository
            .GetAsync(x => x.FirstAccountId == accountId || x.SecondAccountId == accountId);
        
        return contacts;
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