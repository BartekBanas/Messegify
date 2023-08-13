using System.Security.Claims;
using Messegify.Application.Authorization.Requirements;
using Messegify.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Messegify.Application.Authorization.Handlers;

public class ChatRoomAuthorizationHandler : AuthorizationHandler<IsMemberOfRequirement, Chatroom>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        IsMemberOfRequirement requirement,
        Chatroom resource)
    {
        var user = context.User;
        var userId = user.Claims.First(x => x.Type == ClaimTypes.PrimarySid).Value;

        if (resource.Members.Any(member => member.AccountId == new Guid(userId)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}