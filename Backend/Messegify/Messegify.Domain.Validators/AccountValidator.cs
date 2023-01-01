using FluentValidation;
using Messegify.Domain.Entities;

namespace Messegify.Domain.Validators;

public class AccountValidator : AbstractValidator<Account>
{
    public AccountValidator()
    {
        RuleFor(account => account.Email).EmailAddress().NotEmpty();
        RuleFor(account => account.Name).Length(3, 32).NotEmpty();
        RuleFor(account => account.PasswordHash).NotEmpty();
    }
}