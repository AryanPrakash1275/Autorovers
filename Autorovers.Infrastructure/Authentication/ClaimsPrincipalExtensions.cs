using System;
using System.Security.Claims;

namespace Autorovers.Infrastructure.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        if (principal is null)
            throw new InvalidOperationException("No user principal is available.");

        var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
        if (claim is null || string.IsNullOrWhiteSpace(claim.Value))
            throw new InvalidOperationException("User id claim is missing.");

        if (!Guid.TryParse(claim.Value, out var userId))
            throw new InvalidOperationException("User id claim is not a valid GUID.");

        return userId;
    }
}
