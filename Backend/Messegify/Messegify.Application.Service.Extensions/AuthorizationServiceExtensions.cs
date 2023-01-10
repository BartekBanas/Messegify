using System.Security.Claims;
using Messegify.Application.Errors;
using Microsoft.AspNetCore.Authorization;

namespace Messegify.Application.Service.Extensions;

public static class AuthorizationServiceExtensions
{
    public static async Task<AuthorizationResult> AuthorizeRequiredAsync(
        this IAuthorizationService authorizationService, 
        ClaimsPrincipal claimsPrincipal, object? resource, string policy)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, resource, policy);

        if (!authorizationResult.Succeeded)
            throw new ForbiddenError();
        
        return authorizationResult;
    }
}