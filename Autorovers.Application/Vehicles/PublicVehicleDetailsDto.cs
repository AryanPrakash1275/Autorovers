namespace Autorovers.Application.Vehicles;

public sealed class PublicVehicleDetailsDto
{
    public string Slug { get; init; } = null!;
    public string Brand { get; init; } = null!;
    public string Model { get; init; } = null!;
    public string? Variant { get; init; }

    public int Year { get; init; }
    public decimal Price { get; init; }

    public string Category { get; init; } = null!;
    public string Transmission { get; init; } = null!;
    public string? ImageUrl { get; init; }

    // ENGINE
    public double? Power { get; init; }
    public int? PowerRpm { get; init; }
    public double? Torque { get; init; }
    public int? TorqueRpm { get; init; }
    public string? EngineType { get; init; }
    public string? Specification { get; init; }
}
