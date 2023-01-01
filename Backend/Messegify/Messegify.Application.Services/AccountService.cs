using Messegify.Application.Dtos;
using Messegify.Application.Errors;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Messegify.Application.Services;

public interface IAccountService
{
    Task RegisterAccount(RegisterAccountDto registerDto);
    Task<string> Authenticate(LoginDto loginDto);
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

        await _accountRepository.SaveChangesAsync();
    }

    public async Task<string> Authenticate(LoginDto loginDto)
    {
        var foundAccount = await _accountRepository
            .GetOneAsync(account =>
                account.Name == loginDto.UsernameOrEmail || account.Email == loginDto.UsernameOrEmail);

        if (foundAccount == default)
        {
            throw new ForbiddenError();
        }

        if (!_hashingService.VerifyPassword(foundAccount, loginDto.Password))
        {
            throw new ForbiddenError();
        }
        
        // TODO: THIS SHOULD BE A JWT TOKEN
        return "THIS SHOULD BE A JWT TOKEN";
    }
}