using System.Linq.Expressions;
using FluentValidation.TestHelper;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Validators;
using Messegify.Infrastructure.Error;
using Moq;
using Xunit;

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

    [Fact]
    public async Task Validate_DuplicateName_ThrowsItemDuplicatedErrorException()
    {
        // Arrange
        var existingAccounts = new List<Account>
        {
            new Account { Name = "ExistingUser" }
        };

        var accountRepository = new Mock<IRepository<Account>>();
        accountRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Account, bool>>>(), null))
            .ReturnsAsync(existingAccounts);

        var validator = new AccountValidator(accountRepository.Object);
        var account = new Account { Name = "ExistingUser" };

        // Act & Assert
        await Assert.ThrowsAsync<ItemDuplicatedErrorException>(async () => await validator.TestValidateAsync(account));
    }

    [Fact]
    public async Task Validate_DuplicateEmail_ValidationErrors()
    {
        // Arrange
        var existingAccounts = new List<Account>
        {
            new Account { Email = "existing@example.com" }
        };

        var accountRepository = new Mock<IRepository<Account>>();
        accountRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Account, bool>>>(), null))
            .ReturnsAsync(existingAccounts);

        var validator = new AccountValidator(accountRepository.Object);
        var account = new Account { Email = "existing@example.com" };

        // Act & Assert
        await Assert.ThrowsAsync<ItemDuplicatedErrorException>(async () => await validator.TestValidateAsync(account));
    }
}