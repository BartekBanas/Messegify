﻿using AutoMapper;
using MediatR;
using Messegify.Application.Authorization;
using Messegify.Application.Dtos;
using Messegify.Application.Service.Extensions;
using Messegify.Application.Services.ChatRoomRequests;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Messegify.Application.Services;

public interface IMessageRequestHandler : IRequestHandler<SendMessageRequest>, 
    IRequestHandler<GetMessagesRequest, IEnumerable<MessageDto>>
{
    
}

public class MessageRequestHandler : IMessageRequestHandler
{
    private readonly IRepository<ChatRoom> _chatRoomRepository;
    private readonly IRepository<Account> _accountRepository;
    private readonly IRepository<Message> _messageRepository;

    private readonly IMapper _mapper;
    
    private readonly IAuthorizationService _authorizationService;

    private readonly IHttpContextAccessor _httpContextAccessor;


    public MessageRequestHandler(IRepository<ChatRoom> chatRoomRepository,
        IRepository<Account> accountRepository,
        IRepository<Message> messageRepository,
        IAuthorizationService authorizationService,
        IHttpContextAccessor httpContextAccessor, 
        IMapper mapper)
    {
        _chatRoomRepository = chatRoomRepository;
        _accountRepository = accountRepository;
        _messageRepository = messageRepository;
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(SendMessageRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext.User;

        var chatRoom = await _chatRoomRepository.GetOneRequiredAsync(x => x.Id == request.ChatRoomId, 
            nameof(ChatRoom.Members));

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
        
        return Unit.Value;
    }

    public async Task<IEnumerable<MessageDto>> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext.User;

        var chatRoom = await _chatRoomRepository.GetOneRequiredAsync(x => x.Id == request.ChatRoomId, 
            nameof(ChatRoom.Members));

        await _authorizationService.AuthorizeRequiredAsync(user, chatRoom, AuthorizationPolicies.IsMemberOf);

        var messages = await _messageRepository
            .GetAsync(message => message.ChatRoomId == request.ChatRoomId);

        var dtos = _mapper.Map<IEnumerable<MessageDto>>(messages);
        
        return dtos;
    }
}