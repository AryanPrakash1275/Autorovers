namespace Autorovers.Application.Vehicles;

public sealed class VehicleDetailsDto
{
    public int Id { get; init; }
    public string Brand { get; init; } = null!;
    public string Model { get; init; } = null!;
    public string? Variant { get; init; }
    public string Slug { get; init; } = null!;

    public int Year { get; init; }
    public decimal Price { get; init; }
    public string Category { get; init; } = null!;
    public string Transmission { get; init; } = null!;
    public string EngineType { get; init; } = null!;
    public string Specification { get; init; } = null!;
    public string InductionType { get; init; } = null!;
    public double Power { get; init; }
    public double Torque { get; init; }
    public string Emission { get; init; } = null!;
    public double Mileage { get; init; }
    public string AutoStartStop { get; init; } = null!;
    public int Range { get; init; }

    public int Length { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
    public int Weight { get; init; }
    public int GroundClearance { get; init; }
    public int WheelBase { get; init; }

    public int PersonCapacity { get; init; }
    public int Rows { get; init; }
    public int Doors { get; init; }
    public int BootSpace { get; init; }
    public double TankSize { get; init; }

    public string? FrontTyre { get; init; }
    public string? BackType { get; init; }        // yes, match the exact name used in VehicleService
    public string? FrontBrake { get; init; }
    public string? BackBrake { get; init; }
    public string? PoweredSteering { get; init; } // or bool if your entity uses bool
    public string? TyreSizeFront { get; init; }
    public string? TyreSizeBack { get; init; }
    public string? TyreType { get; init; }
    public string? WheelMaterial { get; init; }
    public string? Spare { get; init; }

    // if you already added ImageUrl on entity, include it:
    public string? ImageUrl { get; init; }
}
