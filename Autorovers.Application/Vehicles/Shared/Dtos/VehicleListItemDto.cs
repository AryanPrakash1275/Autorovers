namespace Autorovers.Application.Vehicles;

public sealed class VehicleListItemDto
{
    public int Id { get; init; }

    public string Brand { get; init; } = null!;
    public string Model { get; init; } = null!;
    public string? Variant { get; init; }

    public int Year { get; init; }
    public decimal Price { get; init; }

    public string Category { get; init; } = null!;
    public string Transmission { get; init; } = null!;

    public string Slug { get; init; } = null!;
    public string? ImageUrl { get; init; }

    // Added: blockers removal for frontend (compare/filter/type gating)
    public string VehicleType { get; init; } = null!; // "bike" | "car"
    public string FuelType { get; init; } = null!;    // "EV" | "Hybrid" | "Petrol" | "Diesel" | ...
}
