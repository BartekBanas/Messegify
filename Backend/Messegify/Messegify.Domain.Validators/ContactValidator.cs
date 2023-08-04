using FluentValidation;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Infrastructure.Error;

namespace Messegify.Domain.Validators;

public class ContactValidator : AbstractValidator<Contact>
{
    private readonly IRepository<Contact> _contactRepository;

    public ContactValidator(IRepository<Contact> contactRepository)
    {
        _contactRepository = contactRepository;

        RuleFor(contact => contact).CustomAsync(ValidateUniqueContact);
    }
    
    private async Task ValidateUniqueContact(Contact contact, ValidationContext<Contact> context, CancellationToken token)
    {
        var contacts = await _contactRepository
            .GetAsync(foundContact => (foundContact.FirstAccountId == contact.FirstAccountId &&
                                       foundContact.SecondAccountId == contact.SecondAccountId)
                                 || (foundContact.FirstAccountId == contact.SecondAccountId &&
                                     foundContact.SecondAccountId == contact.FirstAccountId));

        if (contacts.Any())
        {
            throw new ItemDuplicatedErrorException("Contact already exists");
        }
    }
}