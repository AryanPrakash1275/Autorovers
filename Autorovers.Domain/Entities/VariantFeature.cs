using Autorovers.Common;

namespace Autorovers.Domain.Entities;

public sealed class VariantFeature : Entity
{
    public int VariantId { get; set; }
    public VehicleVariant Variant { get; set; } = null!;

    public required string FieldKey { get; set; }
    public bool IsAvailable { get; set; }
}
