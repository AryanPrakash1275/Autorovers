using System.Collections.Generic;

namespace Autorovers.Application.Vehicles;

public sealed class VehicleVariantDto
{
    public int Id { get; init; }

    // e.g. "Base", "Top", "ZX", "GT Line", "Single ABS"
    public string Name { get; init; } = null!;

    // You decide whether this is absolute price or delta.
    // Right now keep it simple: absolute variant price.
    public int Price { get; init; }

    public bool IsDefault { get; init; }

    public List<VariantAddonDto> Addons { get; init; } = new();
}
