using MediatR;
using Messegify.Application.Dtos;

namespace Messegify.Application.Services.MessageRequests;

public class SendMessageRequest : IRequest
{
    public SendMessageDto Dto { get; }
    public Guid ChatRoomId { get; }

    public SendMessageRequest(SendMessageDto dto, Guid chatRoomId)
    {
        Dto = dto;
        ChatRoomId = chatRoomId;
    }
}