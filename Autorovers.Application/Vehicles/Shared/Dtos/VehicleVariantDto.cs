using System.Collections.Generic;

namespace Autorovers.Application.Vehicles;

public sealed class VehicleVariantDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public string Slug { get; init; } = null!;

    public int Price { get; init; }

    public bool IsDefault { get; init; }

    public string? Transmission { get; init; }

    public string? FuelType { get; init; }

    public List<VariantAddonDto> Addons { get; init; } = new();
}
