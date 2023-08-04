using FluentValidation;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Infrastructure.Error;

namespace Messegify.Domain.Validators;

public class AccountValidator : AbstractValidator<Account>
{
    private readonly IRepository<Account> _accountRepository;

    public AccountValidator(IRepository<Account> accountRepository)
    {
        _accountRepository = accountRepository;
        
        RuleFor(account => account.Email).EmailAddress().NotEmpty();
        RuleFor(account => account.Name).Length(3, 32).NotEmpty();
        RuleFor(account => account.PasswordHash).NotEmpty();
        RuleFor(account => account).CustomAsync(ValidateUniqueName);
    }
    
    private async Task ValidateUniqueName(Account account, ValidationContext<Account> context, CancellationToken token)
    {
        var matchingAccounts = await _accountRepository
            .GetAsync(foundAccount => foundAccount.Name == account.Name);

        if (matchingAccounts.Any())
        {
            throw new ItemDuplicatedErrorException("Account with that username already exists");
        }
    }
}