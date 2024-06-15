using MediatR;
using Messegify.Application.Dtos;

namespace Messegify.Application.Services.ChatroomRequests;

public class InviteToChatroomRequest : IRequest
{
    public Guid ChatroomId { get; }
    public Guid AccountId { get; }

    public InviteToChatroomRequest(Guid chatroomId, Guid accountId)
    {
        ChatroomId = chatroomId;
        AccountId = accountId;
    }

    public InviteToChatroomRequest(ChatroomInvite chatroomInvite)
    {
        ChatroomId = chatroomInvite.ChatroomId;
        AccountId = chatroomInvite.AccountId;
    }
}