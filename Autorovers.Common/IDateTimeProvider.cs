namespace Autorovers.Common;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
