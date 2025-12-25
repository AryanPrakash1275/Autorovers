using Autorovers.Domain.Entities;
namespace Autorovers.Application.Vehicles.Admin;

public sealed class UpsertVariantAddonLinkRequest
{
    public bool IsIncluded { get; init; } = false;
    public int SortOrder { get; init; } = 0;
    public int? OverridePrice { get; init; }
}
