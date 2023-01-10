namespace Messegify.Application.Authorization;

public static class AuthorizationPolicies
{
    public static string IsMemberOf => nameof(IsMemberOf);
    public static string IsOwnerOf => nameof(IsOwnerOf);
}