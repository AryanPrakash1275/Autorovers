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
    public string? ImageUrl { get; init; }

    // ENGINE
    public string? EngineType { get; init; }
    public int? EngineDisplacement { get; init; }
    public string? Specification { get; init; }
    public string? InductionType { get; init; }
    public double? Power { get; init; }
    public int? PowerRpm { get; init; }   // NEW
    public double? Torque { get; init; }
    public int? TorqueRpm { get; init; }
    public string? Emission { get; init; }
    public double? Mileage { get; init; }

    public string? AutoStartStop { get; init; }
    public int? Range { get; init; }
    public string? FuelType { get; init; }

    public double? BatteryCapacity { get; init; }
    public double? ChargingTimeFast { get; init; }
    public double? ChargingTimeNormal { get; init; }
    public int? MotorPower { get; init; }
    public int? MotorTorque { get; init; }

    public int? NumberOfGears { get; init; }
    public string? DriveType { get; init; }
    public double? ZeroToHundred { get; init; }
    public int? TopSpeed { get; init; }

    // DIMENSIONS
    public int? Length { get; init; }
    public int? Width { get; init; }
    public int? Height { get; init; }
    public int? Weight { get; init; }
    public int? GroundClearance { get; init; }
    public int? WheelBase { get; init; }
    public double? TurningRadius { get; init; }

    // CAPACITY
    public int? PersonCapacity { get; init; }
    public int? Rows { get; init; }
    public int? Doors { get; init; }
    public int? BootSpace { get; init; }
    public double? TankSize { get; init; }

    // DYNAMICS
    public string? FrontType { get; init; }
    public string? BackType { get; init; }
    public string? FrontBrake { get; init; }
    public string? BackBrake { get; init; }
    public string? FrontSuspension { get; init; }
    public string? RearSuspension { get; init; }
    public string? TyreSizeFront { get; init; }
    public string? TyreSizeBack { get; init; }
    public string? TyreType { get; init; }
    public string? WheelMaterial { get; init; }
    public string? Spare { get; init; }

    public string? PoweredSteering { get; init; }
    public string? SteeringType { get; init; }

    // SAFETY
    public bool? Abs { get; init; }
    public bool? TractionControl { get; init; }
    public int? Airbags { get; init; }
    public bool? HillAssist { get; init; }
    public bool? RearViewCamera { get; init; }
    public bool? ParkingSensors { get; init; }
    public bool? CruiseControl { get; init; }

    // FEATURES
    public double? DisplaySize { get; init; }
    public bool? Bluetooth { get; init; }
    public bool? Navigation { get; init; }
    public bool? SmartConnectivity { get; init; }
    public bool? FastChargingPort { get; init; }
}
