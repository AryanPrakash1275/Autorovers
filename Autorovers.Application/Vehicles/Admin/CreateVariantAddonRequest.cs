using Autorovers.Domain.Entities;
namespace Autorovers.Application.Vehicles.Admin;

public sealed class CreateVariantAddonRequest
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string? Category { get; init; }

    public int? Price { get; init; }
    public bool IsActive { get; init; } = true;
}
