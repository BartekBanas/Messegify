﻿using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using Messegify.Application.Dtos;
using Messegify.Application.Errors;
using Messegify.Application.Service.Extensions;
using Messegify.Application.Services.ChatroomRequests;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Events;
using Microsoft.AspNetCore.Http;

namespace Messegify.Application.Services;

public interface IAccountService
{
    Task<AccountDto> GetAccountAsync(Guid accountId);
    Task<IEnumerable<AccountDto>> GetAccountsAsync();
    Task<IEnumerable<AccountDto>> GetAccountsAsync(int pageSize, int pageNumber);
    Task RegisterAccountAsync(RegisterAccountDto registerDto);
    Task<AccountDto> UpdateAccountAsync(Guid accountId, UpdateAccountDto updateDto);
    Task DeleteAccountAsync(Guid accountId);
    Task<string> AuthenticateAsync(LoginDto loginDto);
    Task CreateContactAsync(Guid accountAId, Guid accountBId, CancellationToken cancellationToken);
    Task DeleteContactAsync(Guid contactId, CancellationToken cancellationToken);
    Task<IEnumerable<ContactDto>> GetContactsAsync();
    Task<IEnumerable<ContactDto>> GetActiveContactsAsync();
}

public class AccountService : IAccountService
{
    private readonly IRepository<Account> _accountRepository;
    private readonly IRepository<Contact> _contactRepository;

    private readonly IValidator<Account> _validator;
    private readonly IValidator<Contact> _contactValidator;
    private readonly IHashingService _hashingService;
    private readonly IJwtService _jwtService;
    
    private readonly IChatroomRequestHandler _chatroomRequestHandler;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IMapper _mapper;

    public AccountService(
        IRepository<Account> accountRepository, 
        IRepository<Contact> contactRepository,
        IHashingService hashingService,
        IValidator<Account> validator,
        IValidator<Contact> contactValidator,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper, 
        IChatroomRequestHandler chatroomRequestHandler)
    {
        _accountRepository = accountRepository;
        _contactRepository = contactRepository;
        _hashingService = hashingService;
        _validator = validator;
        _contactValidator = contactValidator;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _chatroomRequestHandler = chatroomRequestHandler;
    }

    public async Task<AccountDto> GetAccountAsync(Guid accountId)
    {
        var account = await _accountRepository.GetOneRequiredAsync(accountId);

        var dtos = _mapper.Map<AccountDto>(account);

        return dtos;
    }

    public async Task<IEnumerable<AccountDto>> GetAccountsAsync()
    {
        var accounts = await _accountRepository.GetAsync();

        var dtos = _mapper.Map<IEnumerable<AccountDto>>(accounts);
    
        return dtos;
    }

    public async Task<IEnumerable<AccountDto>> GetAccountsAsync(int pageSize, int pageNumber)
    {
        var pagedAccounts = await _accountRepository.GetAsync(pageSize, pageNumber);
        var dtos = _mapper.Map<IEnumerable<AccountDto>>(pagedAccounts);
        
        return dtos;
    }

    public async Task RegisterAccountAsync(RegisterAccountDto registerDto)
    {
        var passwordHash = _hashingService.HashPassword(registerDto.Password);

        var newAccount = new Account()
        {
            Email = registerDto.Email,
            Name = registerDto.Name,
            PasswordHash = passwordHash,
            DateCreated = DateTime.UtcNow,
        };
        
        await _validator.ValidateRequiredAsync(newAccount);

        await _accountRepository.CreateAsync(newAccount);

        newAccount.AddDomainEvent(new AccountCreatedDomainEvent(newAccount));
        
        await _accountRepository.SaveChangesAsync();
    }
    
    public async Task<AccountDto> UpdateAccountAsync(Guid accountId, UpdateAccountDto updateDto)
    {
        var originalAccount = _accountRepository.GetOneRequiredAsync(accountId).Result;

        string? passwordHash = null;
        
        if (updateDto.Password != null)
        {
            passwordHash = _hashingService.HashPassword(updateDto.Password);
        }

        var testAccount = new Account
        {
            Id = new Guid(),
            DateCreated = originalAccount.DateCreated,

            Name = updateDto.Name ?? "testAccountName",
            Email = updateDto.Email ?? "testAccountEmail@email.com",
            PasswordHash = passwordHash ?? "testAccountPassword"
        };

        await _validator.ValidateAsync(testAccount);
        
        var updatedAccount = new Account
        {
            Id = originalAccount.Id,
            DateCreated = originalAccount.DateCreated,

            Name = updateDto.Name ?? originalAccount.Name,
            Email = updateDto.Email ?? originalAccount.Email,
            PasswordHash = passwordHash ?? originalAccount.PasswordHash
        };

        if (originalAccount.Equals(updatedAccount))
        {
            throw new BadRequestError("Updated account is indifferent to the original");
        }

        await _accountRepository.UpdateAsync(updatedAccount, accountId);
        await _accountRepository.SaveChangesAsync();

        var dto = _mapper.Map<Account, AccountDto>(updatedAccount);

        return dto;
    }
    
    public async Task DeleteAccountAsync(Guid accountId)
    {
        var contacts = await GetContactsAsync();

        foreach (var contact in contacts)
        {
            var deleteChatroomRequest = new DeleteChatroomRequest(contact.ContactChatRoomId);
            var token = new CancellationToken();

            await _chatroomRequestHandler.Handle(deleteChatroomRequest, token);
            
            await _contactRepository.DeleteOneAsync(contact.Id);
        }
        
        await _accountRepository.DeleteOneAsync(accountId);

        await _contactRepository.SaveChangesAsync();
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

    public async Task CreateContactAsync(Guid accountAId, Guid accountBId, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository
            .GetOneAsync(contact => contact.FirstAccountId == accountAId & contact.SecondAccountId == accountBId ||
                                 contact.FirstAccountId == accountBId & contact.SecondAccountId == accountAId);

        if (contact is not null)
        {
            if (contact.Active)
            {
                throw new RedundantContactCreationRequestError();
            }
            else
            {
                contact.Active = true;
                var contactChatroom = await _chatroomRequestHandler.Handle(
                    new GetChatroomRequest(contact.ContactChatRoomId), cancellationToken);

                if (!contactChatroom.Members.Contains(accountAId))
                {
                    await _chatroomRequestHandler.Handle(new AddToChatroomRequest(
                        contact.ContactChatRoomId, accountAId), cancellationToken);
                }
                
                if (!contactChatroom.Members.Contains(accountBId))
                {
                    await _chatroomRequestHandler.Handle(new AddToChatroomRequest(
                        contact.ContactChatRoomId, accountBId), cancellationToken);
                }
            }
        }
        else
        {
            var newContact = new Contact
            {
                FirstAccountId = accountAId,
                SecondAccountId = accountBId
            };

            await _contactValidator.ValidateAsync(newContact, cancellationToken);

            await _contactRepository.CreateAsync(newContact);

            newContact.AddDomainEvent(new ContactCreatedDomainEvent(newContact));

            await _contactRepository.SaveChangesAsync();
        }
    }

    public async Task DeleteContactAsync(Guid contactId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetId();
        var contact = await _contactRepository.GetOneRequiredAsync(contactId);
        var getChatroomRequest = new GetChatroomRequest(contact.ContactChatRoomId);
        var chatroom = await _chatroomRequestHandler.Handle(getChatroomRequest, cancellationToken);

        if (contact.Active)
        {
            contact.Active = false;
            await _chatroomRequestHandler.Handle(new LeaveChatroomRequest(contact.ContactChatRoomId), cancellationToken);
        }
        else if (chatroom.Members.Count() == 1 &&
                 chatroom.Members.Any(memberId => memberId == userId)) 
        {
            var request = new DeleteChatroomRequest(contact.ContactChatRoomId);
            await _chatroomRequestHandler.Handle(request, cancellationToken);

            await _contactRepository.DeleteOneAsync(contactId);
            await _contactRepository.SaveChangesAsync();
        }
        else
        {
            throw new BadRequestError("You had already abandoned this contact");
        }
    }

    public async Task<IEnumerable<ContactDto>> GetContactsAsync()
    {
        var accountId = _httpContextAccessor.HttpContext.User.GetId();
        
        var contacts = await _contactRepository
            .GetAsync(contact => contact.FirstAccountId == accountId || contact.SecondAccountId == accountId);

        var dtos = _mapper.Map<IEnumerable<ContactDto>>(contacts);
        
        return dtos;
    }

    public async Task<IEnumerable<ContactDto>> GetActiveContactsAsync()
    {
        var accountId = _httpContextAccessor.HttpContext.User.GetId();

        var contacts = await _contactRepository.GetAsync(contact =>
                contact.Active & (contact.FirstAccountId == accountId || contact.SecondAccountId == accountId));

        var dtos = _mapper.Map<IEnumerable<ContactDto>>(contacts);
        
        return dtos;
    }

    private static ClaimsIdentity GenerateClaimsIdentity(Account account)
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