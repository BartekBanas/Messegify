using Messegify.Application.Dtos;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Messegify.Application.Services;

public interface IAccountService
{
    public Task RegisterAccount(RegisterAccountDto registerDto);
}

public class AccountService : IAccountService
{
    private readonly IRepository<Account> _accountRepository;
    private readonly IHashingService _hashingService;
    

    public AccountService(
        IRepository<Account> accountRepository, 
        IHashingService hashingService)
    {
        _accountRepository = accountRepository;
        _hashingService = hashingService;
    }

    public async Task RegisterAccount(RegisterAccountDto registerDto)
    {
        var passwordHash = _hashingService.HashPassword(registerDto.Password);

        await _accountRepository.CreateAsync(new Account()
        {
            Email = registerDto.Email,
            Name = registerDto.Username,
            PasswordHash = passwordHash,
        });
    }

    public async Task Authenticate()
    {
        
    }
}