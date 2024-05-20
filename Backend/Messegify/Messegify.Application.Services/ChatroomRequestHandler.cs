using System.Linq.Expressions;
using AutoMapper;
using Messegify.Application.Authorization;
using Messegify.Application.Dtos;
using Messegify.Application.Errors;
using Messegify.Application.Service.Extensions;
using Messegify.Application.Services.ChatroomRequests;
using Messegify.Application.Services.MessageRequests;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Messegify.Application.Services;

public interface IChatroomRequestHandler
{
    Task Handle(CreateChatroomRequest request, CancellationToken cancellationToken);
    Task Handle(DeleteChatroomRequest request, CancellationToken cancellationToken);
    Task<IEnumerable<ChatRoomDto>> Handle(GetUserChatroomsRequest request, CancellationToken cancellationToken);
}

public class ChatroomRequestHandler : IChatroomRequestHandler
{
    private readonly IRepository<Chatroom> _chatRoomRepository;
    private readonly IRepository<Account> _accountRepository;

    private readonly IAuthorizationService _authorizationService;
    private readonly IMessageRequestHandler _messageRequestHandler;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IMapper _mapper;

    public ChatroomRequestHandler(
        IRepository<Chatroom> chatRoomRepository,
        IRepository<Account> accountRepository,
        IHttpContextAccessor httpContextAccessor,
        IAuthorizationService authorizationService,
        IMessageRequestHandler messageRequestHandler,
        IMapper mapper)
    {
        _chatRoomRepository = chatRoomRepository;
        _accountRepository = accountRepository;
        _httpContextAccessor = httpContextAccessor;
        _authorizationService = authorizationService;
        _messageRequestHandler = messageRequestHandler;
        _mapper = mapper;
    }

    public async Task Handle(CreateChatroomRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetId();

        var account = await _accountRepository.GetOneRequiredAsync(userId);

        var newChatRoom = new Chatroom
        {
            Name = $"{account.Name}'s room"
        };

        await _chatRoomRepository.CreateAsync(newChatRoom);

        newChatRoom.AddDomainEvent(new ChatRoomCreatedDomainEvent(newChatRoom, account));

        await _chatRoomRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<ChatRoomDto>> Handle(GetUserChatroomsRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetId();

        Expression<Func<Chatroom, bool>> filter = chatRoom
            => chatRoom.Members.Any(membership => membership.AccountId == userId);

        var chatRooms = await _chatRoomRepository
            .GetAsync(filter, null, nameof(Chatroom.Members));

        var dtos = _mapper.Map<IEnumerable<ChatRoomDto>>(chatRooms);

        return dtos;
    }

    public async Task Handle(DeleteChatroomRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext.User;
        
        var chatRoom = await _chatRoomRepository.GetOneRequiredAsync(chatRoom => chatRoom.Id == request.ChatRoomId, 
            nameof(Chatroom.Members));
        
        await _authorizationService.AuthorizeRequiredAsync(user, chatRoom, AuthorizationPolicies.IsMemberOf);

        if (chatRoom.ChatRoomType != ChatRoomType.Direct)
        {
            throw new ForbiddenError("You can only delete private chatrooms");
        }

        var token = new CancellationToken();
        
        var getMessagesRequest = new GetMessagesRequest(request.ChatRoomId);
        var messages = _messageRequestHandler.Handle(getMessagesRequest, token);

        foreach (var message in messages.Result)
        {
            var deleteMessageRequest = new DeleteMessageRequest(message.Id);

            await _messageRequestHandler.Handle(deleteMessageRequest, token);
        }
        
        await _chatRoomRepository.DeleteAsync(chatRoom.Id);
    }
}