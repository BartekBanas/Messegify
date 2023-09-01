using System.Linq.Expressions;
using FluentValidation.TestHelper;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Validators;
using Messegify.Infrastructure.Error;
using Moq;
using Xunit;

namespace Domain.Validators.Tests;

public class ContactValidatorTests
{
    [Fact]
    public async Task Validate_UniqueContact_NoValidationErrors()
    {
        // Arrange
        var contactRepository = new Mock<IRepository<Contact>>();
        contactRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Contact, bool>>>(), null))
            .ReturnsAsync(new List<Contact>());

        var validator = new ContactValidator(contactRepository.Object);
        var contact = new Contact
        {
            FirstAccountId = Guid.NewGuid(),
            SecondAccountId = Guid.NewGuid()
        };

        // Act
        var result = await validator.TestValidateAsync(contact);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c);
    }

    [Fact]
    public async Task Validate_DuplicateContact_ThrowsItemDuplicatedErrorException()
    {
        // Arrange
        var existingContacts = new List<Contact>
        {
            new Contact
            {
                FirstAccountId = Guid.NewGuid(),
                SecondAccountId = Guid.NewGuid()
            }
        };

        var contactRepository = new Mock<IRepository<Contact>>();
        contactRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Contact, bool>>>(), null))
            .ReturnsAsync(existingContacts);

        var validator = new ContactValidator(contactRepository.Object);
        var contact = new Contact
        {
            FirstAccountId = existingContacts.First().FirstAccountId,
            SecondAccountId = existingContacts.First().SecondAccountId
        };

        // Act & Assert
        await Assert.ThrowsAsync<ItemDuplicatedErrorException>(async () => await validator.TestValidateAsync(contact));
    }
}