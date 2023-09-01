using System.Linq.Expressions;
using FluentValidation.TestHelper;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Validators;
using Moq;

namespace Domain.Validators.Tests;

public class AccountValidatorTests
{
    [Fact]
    public async Task Validate_ValidAccount_NoValidationErrors()
    {
        // Arrange
        var accountRepository = new Mock<IRepository<Account>>();
        accountRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Account, bool>>>(), null))
            .ReturnsAsync(new List<Account>());

        var validator = new AccountValidator(accountRepository.Object);
        var account = new Account
        {
            Email = "test@example.com",
            Name = "TestUser",
            PasswordHash = "hashedPassword"
        };

        // Act
        var result = await validator.TestValidateAsync(account);

        // Assert
        result.ShouldNotHaveValidationErrorFor(acc => acc.Email);
        result.ShouldNotHaveValidationErrorFor(acc => acc.Name);
        result.ShouldNotHaveValidationErrorFor(acc => acc.PasswordHash);
    }
}