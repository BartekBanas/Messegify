using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;

namespace Messegify.Application.Authorization;

public class IsMemberOfRequirement : IAuthorizationRequirement
{
    // Intentionally empty
}