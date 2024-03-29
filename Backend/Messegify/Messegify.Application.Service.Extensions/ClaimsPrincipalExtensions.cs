﻿using System.Security.Claims;

namespace Messegify.Application.Service.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetId(this ClaimsPrincipal claimsPrincipal) =>
        new(claimsPrincipal.Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value);
}