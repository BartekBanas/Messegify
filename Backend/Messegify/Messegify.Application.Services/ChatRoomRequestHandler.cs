using MediatR;
using Messegify.Application.Service.Extensions;
using Messegify.Application.Services.ChatRoomRequests;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Messegify.Application.Services;

public interface IChatRoomRequestHandler 
    : IRequestHandler<CreateChatRoomRequest>
{
}

public class ChatRoomRequestHandler : IChatRoomRequestHandler
{
    private readonly IRepository<ChatRoom> _chatRoomRepository;
    private readonly IRepository<Account> _accountRepository;
    
    private readonly IAuthorizationService _authorizationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChatRoomRequestHandler(
        IRepository<ChatRoom> chatRoomRepository, 
        IRepository<Account> accountRepository, 
        IAuthorizationService authorizationService, 
        IHttpContextAccessor httpContextAccessor)
    {
        _chatRoomRepository = chatRoomRepository;
        _accountRepository = accountRepository;
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Unit> Handle(
        CreateChatRoomRequest request, 
        CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetId();

        var account = await _accountRepository.GetOneRequiredAsync(userId);

        var newChatRoom = new ChatRoom()
        {
            Name = $"{account.Name}'s room"
        };

        await _chatRoomRepository.CreateAsync(newChatRoom);

        newChatRoom.AddDomainEvent(new ChatRoomCreatedDomainEvent(newChatRoom, account));
        
        await _chatRoomRepository.SaveChangesAsync();
        
        return Unit.Value;
    }
}