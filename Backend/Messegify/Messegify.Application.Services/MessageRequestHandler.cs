using AutoMapper;
using MediatR;
using Messegify.Application.Authorization;
using Messegify.Application.Dtos;
using Messegify.Application.Service.Extensions;
using Messegify.Application.Services.MessageRequests;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Messegify.Application.Services;

public interface IMessageRequestHandler : 
    IRequestHandler<SendMessageRequest>, 
    IRequestHandler<GetMessagesRequest, IEnumerable<MessageDto>>,
    IRequestHandler<GetPagedMessagesRequest, IEnumerable<MessageDto>>,
    IRequestHandler<DeleteMessageRequest>,
    IRequestHandler<DeleteMessagesRequest>
{
    
}

public class MessageRequestHandler : IMessageRequestHandler
{
    private readonly IRepository<Chatroom> _chatRoomRepository;
    private readonly IRepository<Message> _messageRepository;

    private readonly IMapper _mapper;
    
    private readonly IAuthorizationService _authorizationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public MessageRequestHandler(
        IRepository<Chatroom> chatRoomRepository,
        IRepository<Message> messageRepository,
        IAuthorizationService authorizationService,
        IHttpContextAccessor httpContextAccessor, 
        IMapper mapper)
    {
        _chatRoomRepository = chatRoomRepository;
        _messageRepository = messageRepository;
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task Handle(SendMessageRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext.User;

        var chatRoom = await _chatRoomRepository.GetOneRequiredAsync(chatRoom => chatRoom.Id == request.ChatRoomId, 
            nameof(Chatroom.Members));

        await _authorizationService.AuthorizeRequiredAsync(user, chatRoom, AuthorizationPolicies.IsMemberOf);

        var newMessage = new Message()
        {
            AccountId = user.GetId(),
            TextContent = request.Dto.TextContent,
            
            ChatRoomId = chatRoom.Id
        };
        await _messageRepository.CreateAsync(newMessage);

        newMessage.AddDomainEvent(new MessageSentDomainEvent(newMessage));

        await _messageRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<MessageDto>> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext.User;

        var chatRoom = await _chatRoomRepository.GetOneRequiredAsync(chatRoom => chatRoom.Id == request.ChatRoomId, 
            nameof(Chatroom.Members));

        await _authorizationService.AuthorizeRequiredAsync(user, chatRoom, AuthorizationPolicies.IsMemberOf);

    var messages = await _messageRepository
        .GetAsync(
            message => message.ChatRoomId == request.ChatRoomId,
            query => query.OrderBy(message => message.SentDate) // Sent Date ascending
        );

        var dtos = _mapper.Map<IEnumerable<MessageDto>>(messages);
        
        return dtos;
    }
    
    public async Task<IEnumerable<MessageDto>> Handle(GetPagedMessagesRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext.User;

        var chatRoom = await _chatRoomRepository.GetOneRequiredAsync(chatRoom => chatRoom.Id == request.ChatRoomId, 
            nameof(Chatroom.Members));

        await _authorizationService.AuthorizeRequiredAsync(user, chatRoom, AuthorizationPolicies.IsMemberOf);

        var messages = await _messageRepository
            .GetAsync(
                request.PageSize, request.PageNumber,
                message => message.ChatRoomId == request.ChatRoomId,
                query => query.OrderBy(message => message.SentDate)
            );

        var dtos = _mapper.Map<IEnumerable<MessageDto>>(messages);
        
        return dtos;
    }
    
    public async Task Handle(DeleteMessageRequest request, CancellationToken cancellationToken)
    {
        await _messageRepository.DeleteOneAsync(request.MessageId);
    }

    public async Task Handle(DeleteMessagesRequest request, CancellationToken cancellationToken)
    {
        foreach (var messageId in request.MessageIds)
        {
            await _messageRepository.DeleteOneAsync(messageId);
        }
    }
}