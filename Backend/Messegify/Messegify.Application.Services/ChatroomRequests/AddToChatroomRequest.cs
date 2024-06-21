using MediatR;
using Messegify.Application.Dtos;

namespace Messegify.Application.Services.ChatroomRequests;

public class AddToChatroomRequest : IRequest
{
    public Guid ChatroomId { get; }
    public Guid AccountId { get; }

    public AddToChatroomRequest(Guid chatroomId, Guid accountId)
    {
        ChatroomId = chatroomId;
        AccountId = accountId;
    }

    public AddToChatroomRequest(ChatroomInvite chatroomInvite)
    {
        ChatroomId = chatroomInvite.ChatroomId;
        AccountId = chatroomInvite.AccountId;
    }
}