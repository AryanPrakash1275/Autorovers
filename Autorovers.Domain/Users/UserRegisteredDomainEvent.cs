using Autorovers.Common;

namespace Autorovers.Domain.Users;

public sealed class UserRegisteredDomainEvent : IDomainEvent
{
    public Guid UserId { get; }
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;

    public UserRegisteredDomainEvent(Guid userId)
    {
        UserId = userId;
    }
}
