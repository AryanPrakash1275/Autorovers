using System.Collections.Generic;

namespace Autorovers.Application.Vehicles;

public sealed class VehicleWithDetailsDto
{
    public int Id { get; init; }

    public string Brand { get; init; } = "";
    public string Model { get; init; } = "";
    public string? Variant { get; init; }

    public int Year { get; init; }

    // List card price (starting price). Variant price comes from Variants[]
    public int Price { get; init; }

    public string Category { get; init; } = "";
    public string Transmission { get; init; } = "";

    public string? Slug { get; init; }
    public string? ImageUrl { get; init; }

    public string VehicleType { get; init; } = null!;
    public string FuelType { get; init; } = null!;

    public VehicleDetailsDto Details { get; init; } = new();

    public List<VehicleVariantDto> Variants { get; init; } = new();
}
