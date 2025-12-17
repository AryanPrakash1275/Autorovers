using Autorovers.Domain.Entities;

namespace Autorovers.Application.Vehicles;

public class UpdateVehicleRequest
{
    // Allow updating core fields
    public VehicleKind Kind { get; set; } = VehicleKind.Car;
    public PowertrainKind Powertrain { get; set; } = PowertrainKind.Ice;

    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? Variant { get; set; }

    public int Year { get; set; }
    public int Price { get; set; }

    public string Category { get; set; } = null!;
    public string Transmission { get; set; } = null!;
    public string? ImageUrl { get; init; }

    // Full replace of details (keep it simple + consistent)
    public EngineSpecs? Engine { get; set; }
    public EvSpecs? Ev { get; set; }

    public DimensionsSpecs? Dimensions { get; set; }
    public TyresBrakesSuspensionSpecs? Dynamics { get; set; }

    public BikeSpecs? Bike { get; set; }
    public CarSpecs? Car { get; set; }

    public string? ColorsAvailableJson { get; set; }
    public int? WarrantyYears { get; set; }
    public int? ServiceIntervalKm { get; set; }
}
