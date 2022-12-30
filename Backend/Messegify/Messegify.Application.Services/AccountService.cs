using Messegify.Application.Dtos;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;

namespace Messegify.Application.Services;

public interface IAccountService
{
    public Task RegisterAccount(RegisterAccountDto registerDto);
}

public class AccountService : IAccountService
{
    private readonly IRepository<Account> _accountRepository;

    public AccountService(IRepository<Account> accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task RegisterAccount(RegisterAccountDto registerDto)
    {
        await _accountRepository.CreateAsync(new Account()
        {
            Email = registerDto.Email,
            Name = registerDto.Username,
            PasswordHash = registerDto.Password,
        });
    }
}