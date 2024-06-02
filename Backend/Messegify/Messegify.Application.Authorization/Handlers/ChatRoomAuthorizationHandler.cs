using System.Security.Claims;
using Messegify.Application.Authorization.Requirements;
using Messegify.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Messegify.Application.Authorization.Handlers;

public class ChatRoomAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement, Chatroom>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IAuthorizationRequirement requirement,
        Chatroom resource)
    {
        return requirement switch
        {
            IsMemberOfRequirement isMemberOfRequirement => HandleIsMemberOfRequirementAsync(
                context, isMemberOfRequirement, resource),

            IsOwnerRequirement isOwnerRequirement => HandleIsOwnerOfRequirementAsync(
                context, isOwnerRequirement, resource),

            _ => throw new NotImplementedException()
        };
    }

    private Task HandleIsMemberOfRequirementAsync(
        AuthorizationHandlerContext context,
        IsMemberOfRequirement requirement,
        Chatroom resource)
    {
        var user = context.User;
        var userId = user.Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value;

        if (resource.Members.Any(member => member.AccountId == new Guid(userId)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

    private Task HandleIsOwnerOfRequirementAsync(
        AuthorizationHandlerContext context,
        IsOwnerRequirement requirement,
        Chatroom resource)
    {
        var user = context.User;
        var userId = user.Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value;
        var members = resource.Members.OrderBy(accountChatroom => accountChatroom.DateJoined).ToList();
        var chatroomOwnerId = members.First().AccountId.ToString();

        if (userId == chatroomOwnerId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}