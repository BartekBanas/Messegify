using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using Messegify.Application.Dtos;
using Messegify.Application.Service.Extensions;
using Messegify.Application.Services.ChatRoomRequests;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Events;
using Microsoft.AspNetCore.Http;

namespace Messegify.Application.Services;

public interface IChatRoomRequestHandler :
    IRequestHandler<CreateChatRoomRequest>,
    IRequestHandler<GetUserChatRooms, IEnumerable<ChatRoomDto>>
{
}

public class ChatRoomRequestHandler : IChatRoomRequestHandler
{
    private readonly IRepository<ChatRoom> _chatRoomRepository;
    private readonly IRepository<Account> _accountRepository;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IMapper _mapper;

    public ChatRoomRequestHandler(
        IRepository<ChatRoom> chatRoomRepository,
        IRepository<Account> accountRepository,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        _chatRoomRepository = chatRoomRepository;
        _accountRepository = accountRepository;
        _httpContextAccessor = httpContextAccessor;
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
}