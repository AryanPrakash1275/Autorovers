namespace Autorovers.Application.Vehicles;

public sealed class PublicVehicleListDto
{
    public string Slug { get; init; } = null!;
    public string Brand { get; init; } = null!;
    public string Model { get; init; } = null!;
    public string? Variant { get; init; }

    public int Year { get; init; }
    public decimal Price { get; init; }

    public string Transmission { get; init; } = null!;
    public string Category { get; init; } = null!;

    public string? ImageUrl { get; init; }
}
