namespace Autorovers.Domain.Entities;

public class VehicleVariantAddon
{
    public int VehicleVariantId { get; set; }
    public VehicleVariant VehicleVariant { get; set; } = null!;

    public int VariantAddonId { get; set; }
    public VariantAddon VariantAddon { get; set; } = null!;

    // included vs optional
    public bool IsIncluded { get; set; } = false;

    public int SortOrder { get; set; } = 0;

    // if price differs per variant
    public int? OverridePrice { get; set; }
}
