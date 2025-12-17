namespace Autorovers.Application.Vehicles;

// =====================================================
// CreateVehicleRequest
// - Core Vehicle fields on root
// - Specs are nested and MATCH Domain
// =====================================================
public class CreateVehicleRequest
{
    // ===== Vehicle core =====
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? Variant { get; set; }
    public int Year { get; set; }
    public int Price { get; set; }

    public string Category { get; set; } = null!;
    public string Transmission { get; set; } = null!;
    public string? Slug { get; set; }
    public string? ImageUrl { get; init; }

    // Legacy input (DO NOT persist to Vehicle; we map it to VehicleKind)
    public string? VehicleType { get; set; }

    // ===== Details/specs =====
    public EngineSpecsRequest? Engine { get; set; }
    public EvSpecsRequest? Ev { get; set; }
    public DimensionsSpecsRequest? Dimensions { get; set; }
    public DynamicsSpecsRequest? Dynamics { get; set; }
    public BikeSpecsRequest? Bike { get; set; }
    public CarSpecsRequest? Car { get; set; }

    // META
    public string? ColorsAvailableJson { get; set; }
    public int? WarrantyYears { get; set; }
    public int? ServiceIntervalKm { get; set; }
}

// ==========================
// Nested requests (MATCH Domain)
// ==========================
public class EngineSpecsRequest
{
    public string? EngineType { get; set; }
    public int? EngineDisplacement { get; set; }
    public string? InductionType { get; set; }
    public string? Emission { get; set; }

    public double? Power { get; set; }
    public int? PowerRpm { get; set; }
    public double? Torque { get; set; }
    public int? TorqueRpm { get; set; }

    public double? Mileage { get; set; }
    public double? Range { get; set; }
    public string? FuelType { get; set; }
}

public class EvSpecsRequest
{
    public double? BatteryCapacity { get; set; }
    public double? ChargingTimeFast { get; set; }
    public double? ChargingTimeNormal { get; set; }
    public int? MotorPower { get; set; }
    public int? MotorTorque { get; set; }

    public bool? FastChargingPort { get; set; }
    public double? Range { get; set; }
}

public class DimensionsSpecsRequest
{
    public int? Length { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? WheelBase { get; set; }
    public int? GroundClearance { get; set; }
    public int? Weight { get; set; }
    public double? TurningRadius { get; set; }
}

public class DynamicsSpecsRequest
{
    public string? FrontType { get; set; }
    public string? BackType { get; set; }

    public string? FrontBrake { get; set; }
    public string? BackBrake { get; set; }

    public string? FrontSuspension { get; set; }
    public string? RearSuspension { get; set; }

    public string? TyreSizeFront { get; set; }
    public string? TyreSizeBack { get; set; }
    public string? TyreType { get; set; }
    public string? WheelMaterial { get; set; }
}

public class BikeSpecsRequest
{
    public int? NumberOfGears { get; set; }
    public double? TankSize { get; set; }

    public bool? Abs { get; set; }
    public bool? TractionControl { get; set; }

    public double? DisplaySize { get; set; }
    public bool? Bluetooth { get; set; }
    public bool? Navigation { get; set; }
    public bool? SmartConnectivity { get; set; }
}

public class CarSpecsRequest
{
    public string? DriveType { get; set; }
    public double? ZeroToHundred { get; set; }
    public int? TopSpeed { get; set; }

    public int? PersonCapacity { get; set; }
    public int? Rows { get; set; }
    public int? Doors { get; set; }
    public int? BootSpace { get; set; }

    public bool? PoweredSteering { get; set; }
    public string? SteeringType { get; set; }

    public bool? HasSpareWheel { get; set; }

    public int? Airbags { get; set; }
    public bool? HillAssist { get; set; }
    public bool? RearViewCamera { get; set; }
    public bool? ParkingSensors { get; set; }
    public bool? CruiseControl { get; set; }

    public double? DisplaySize { get; set; }
    public bool? Bluetooth { get; set; }
    public bool? Navigation { get; set; }
    public bool? SmartConnectivity { get; set; }
}
