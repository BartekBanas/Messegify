using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Messegify.Application.Service.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetId(this ClaimsPrincipal claimsPrincipal) =>
        new(claimsPrincipal.Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value);

    public static Guid GetId(this IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext is not null)
        {
            var user = httpContextAccessor.HttpContext.User;

            if (user is { Identity.IsAuthenticated: false })
            {
                throw new UnauthorizedAccessException();
            }

            var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid);

            if (claim == null || string.IsNullOrEmpty(claim.Value))
            {
                throw new Exception("Could not establish the user's ID");
            }

            return Guid.TryParse(claim.Value, out var userId)
                ? userId
                : throw new Exception("Could not establish the user's ID");
        }
        else
        {
            throw new UnauthorizedAccessException();
        }
    }
}