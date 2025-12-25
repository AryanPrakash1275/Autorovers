using Autorovers.Domain.Entities;
namespace Autorovers.Application.Vehicles.Admin;

public sealed class UpdateVariantAddonRequest
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Category { get; init; }

    public int? Price { get; init; }
    public bool? IsActive { get; init; }
}
