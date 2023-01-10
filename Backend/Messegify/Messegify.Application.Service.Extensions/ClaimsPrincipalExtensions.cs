using System.Runtime;
using System.Security.Claims;

namespace Messegify.Application.Service.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetId(this ClaimsPrincipal claimsPrincipal)
    {
        var idClaim = claimsPrincipal.Claims.First(x => x.Type == ClaimTypes.PrimarySid);
        var id = new Guid(idClaim.Value);

        return id;
    }
}