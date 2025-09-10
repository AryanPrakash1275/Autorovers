namespace Autorovers.Common;

public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}
