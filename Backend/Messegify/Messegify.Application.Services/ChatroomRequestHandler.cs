using System.Linq.Expressions;
using MediatR;
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

public interface IChatroomRequestHandler :
    IRequestHandler<CreateChatroomRequest, ChatRoomDto>,
    IRequestHandler<GetUserChatroomsRequest, IEnumerable<ChatRoomDto>>,
    IRequestHandler<DeleteChatroomRequest>,
    IRequestHandler<InviteToChatroomRequest>
{
}

public class ChatroomRequestHandler : IChatroomRequestHandler
{
    private readonly IRepository<Chatroom> _chatRoomRepository;
    private readonly IRepository<Account> _accountRepository;

    private readonly IAuthorizationService _authorizationService;
    private readonly IMessageRequestHandler _messageRequestHandler;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChatroomRequestHandler(
        IRepository<Chatroom> chatRoomRepository,
        IRepository<Account> accountRepository,
        IHttpContextAccessor httpContextAccessor,
        IAuthorizationService authorizationService,
        IMessageRequestHandler messageRequestHandler)
    {
        _chatRoomRepository = chatRoomRepository;
        _accountRepository = accountRepository;
        _httpContextAccessor = httpContextAccessor;
        _authorizationService = authorizationService;
        _messageRequestHandler = messageRequestHandler;
    }

    public async Task<ChatRoomDto> Handle(
        CreateChatroomRequest request,
        CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetId();

        var account = await _accountRepository.GetOneRequiredAsync(userId);

        var newChatRoom = CreateChatroom(request);

        await _chatRoomRepository.CreateAsync(newChatRoom);

        newChatRoom.AddDomainEvent(new ChatRoomCreatedDomainEvent(newChatRoom, account));

        await _chatRoomRepository.SaveChangesAsync();

        return newChatRoom.ToDto();
    }

    public async Task<IEnumerable<ChatRoomDto>> Handle(GetUserChatroomsRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetId();

        Expression<Func<Chatroom, bool>> filter = chatRoom
            => chatRoom.Members.Any(membership => membership.AccountId == userId);

        var chatRooms = await _chatRoomRepository
            .GetAsync(filter, null, nameof(Chatroom.Members));

        var dtos = chatRooms.ToDto();

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
    
    public async Task Handle(InviteToChatroomRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext.User;
        var chatRoom = await _chatRoomRepository.GetOneRequiredAsync(chatRoom => chatRoom.Id == request.ChatroomId, 
            nameof(Chatroom.Members));
        
        if (chatRoom.ChatRoomType is ChatRoomType.Direct)
        {
            throw new BadRequestError("You cannot invite anyone to a direct messaging chatroom");
        }
        
        await _authorizationService.AuthorizeRequiredAsync(user, chatRoom, AuthorizationPolicies.IsOwnerOf);
        
        var invitedAccount = await _accountRepository.GetOneRequiredAsync(request.AccountId);
        
        chatRoom.Members.Add(new AccountChatroom { AccountId = invitedAccount.Id });
    }

    private static Chatroom CreateChatroom(CreateChatroomRequest request)
    {
        return request.ChatRoomType switch
        {
            ChatRoomType.Direct => new Chatroom { Name = "Private chatroom" },
            ChatRoomType.Regular => request.Name is not null
                ? new Chatroom { Name = request.Name }
                : throw new Exception(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}