using Microsoft.AspNetCore.Authorization;

namespace Autorovers.Infrastructure.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
        : base(permission)
    {
    }
}
