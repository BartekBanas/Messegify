using MediatR;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Events;

namespace Messegify.Application.DomainEventHandlers;

public class AccountCreatedDomainEventHandler : INotificationHandler<AccountCreatedDomainEvent>
{
    private readonly IRepository<User> _userRepository;

    public AccountCreatedDomainEventHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(AccountCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var newUser = new User()
        {
            AccountId = notification.Entity.Id,
            Username = notification.Entity.Name,
        };

        await _userRepository.CreateAsync(newUser);
    }
}