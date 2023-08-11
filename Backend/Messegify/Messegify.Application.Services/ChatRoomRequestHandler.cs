using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using Messegify.Application.Authorization;
using Messegify.Application.Dtos;
using Messegify.Application.Errors;
using Messegify.Application.Service.Extensions;
using Messegify.Application.Services.ChatRoomRequests;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Messegify.Application.Services;

public interface IChatRoomRequestHandler :
    IRequestHandler<CreateChatRoomRequest>,
    IRequestHandler<GetUserChatRooms, IEnumerable<ChatRoomDto>>,
    IRequestHandler<DeleteChatRoomRequest>
{
}

public class ChatRoomRequestHandler : IChatRoomRequestHandler
{
    private readonly IRepository<ChatRoom> _chatRoomRepository;
    private readonly IRepository<Account> _accountRepository;
    private readonly IRepository<Message> _messageRepository;
    
    private readonly IAuthorizationService _authorizationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IMapper _mapper;

    public ChatRoomRequestHandler(
        IRepository<ChatRoom> chatRoomRepository,
        IRepository<Account> accountRepository,
        IRepository<Message> messageRepository,
        IHttpContextAccessor httpContextAccessor,
        IAuthorizationService authorizationService,
        IMapper mapper
        )
    {
        _chatRoomRepository = chatRoomRepository;
        _accountRepository = accountRepository;
        _messageRepository = messageRepository;
        _httpContextAccessor = httpContextAccessor;
        _authorizationService = authorizationService;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(
        CreateChatRoomRequest request,
        CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetId();

        var account = await _accountRepository.GetOneRequiredAsync(userId);

        var newChatRoom = new ChatRoom
        {
            Name = $"{account.Name}'s room"
        };

        await _chatRoomRepository.CreateAsync(newChatRoom);

        newChatRoom.AddDomainEvent(new ChatRoomCreatedDomainEvent(newChatRoom, account));

        await _chatRoomRepository.SaveChangesAsync();

        return Unit.Value;
    }

    public async Task<IEnumerable<ChatRoomDto>> Handle(GetUserChatRooms request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetId();

        Expression<Func<ChatRoom, bool>> filter = chatRoom
            => chatRoom.Members.Any(membership => membership.AccountId == userId);

        var chatRooms = await _chatRoomRepository
            .GetAsync(filter, null, nameof(ChatRoom.Members));

        var dtos = _mapper.Map<IEnumerable<ChatRoomDto>>(chatRooms);

        return dtos;
    }

    public async Task<Unit> Handle(DeleteChatRoomRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext.User;
        
        var chatRoom = await _chatRoomRepository.GetOneRequiredAsync(chatRoom => chatRoom.Id == request.ChatRoomId, 
            nameof(ChatRoom.Members));
        
        await _authorizationService.AuthorizeRequiredAsync(user, chatRoom, AuthorizationPolicies.IsMemberOf);

        if (chatRoom.ChatRoomType != ChatRoomType.Direct)
        {
            throw new ForbiddenError("You can only delete private chatrooms");
        }
        
        var messages = await _messageRepository
            .GetAsync(message => message.ChatRoomId == chatRoom.Id);
        
        foreach (var message in messages)
        {
            await _messageRepository.DeleteAsync(message.Id);
        }
        
        await _chatRoomRepository.DeleteAsync(chatRoom.Id);

        return Unit.Value;
    }
}