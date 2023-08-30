using MediatR;
using Messegify.Application.Dtos;

namespace Messegify.Application.Services.MessageRequests;

public class GetPagedMessagesRequest : IRequest<IEnumerable<MessageDto>>
{
    public Guid ChatRoomId { get; }
    public int PageSize { get; }
    public int PageNumber { get; }

    public GetPagedMessagesRequest(Guid chatRoomId, int pageSize, int pageNumber)
    {
        ChatRoomId = chatRoomId;
        PageSize = pageSize;
        PageNumber = pageNumber;
    }
}