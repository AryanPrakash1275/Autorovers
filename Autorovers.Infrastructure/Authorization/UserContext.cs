using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Autorovers.Application.Abstractions.Authentication;

namespace Autorovers.Infrastructure.Authorization;

public sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _accessor;
    public UserContext(IHttpContextAccessor accessor) => _accessor = accessor;

    public Guid UserId
    {
        get
        {
            var id = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var g) ? g : Guid.Empty;
        }
    }

    public string? Email =>
        _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public bool IsAuthenticated =>
        _accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
