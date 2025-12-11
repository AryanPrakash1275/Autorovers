public class CreateVehicleRequest
{
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? Variant { get; set; }
    public int Year { get; set; }
    public int Price { get; set; }
    public string Category { get; set; } = null!;
    public string Transmission { get; set; } = null!;
    public string? Slug { get; set; }
    public string? ImageUrl { get; init; }

    // ENGINE & POWERTRAIN
    public string? EngineType { get; set; }
    public int? EngineDisplacement { get; set; }
    public string? Specification { get; set; }
    public string? InductionType { get; set; }
    public double? Power { get; set; }
    public int? PowerRpm { get; set; }   // NEW FIELD
    public double? Torque { get; set; }
    public int? TorqueRpm { get; set; }
    public string? Emission { get; set; }
    public double? Mileage { get; set; }

    public string? AutoStartStop { get; set; }
    public int? Range { get; set; }
    public string? FuelType { get; set; }

    // EV
    public double? BatteryCapacity { get; set; }
    public double? ChargingTimeFast { get; set; }
    public double? ChargingTimeNormal { get; set; }
    public int? MotorPower { get; set; }
    public int? MotorTorque { get; set; }

    // Bike
    public int? NumberOfGears { get; set; }

    // Car performance
    public string? DriveType { get; set; }
    public double? ZeroToHundred { get; set; }
    public int? TopSpeed { get; set; }

    // DIMENSIONS
    public int? Length { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Weight { get; set; }
    public int? GroundClearance { get; set; }
    public int? WheelBase { get; set; }
    public double? TurningRadius { get; set; }

    // CAPACITY
    public int? PersonCapacity { get; set; }
    public int? Rows { get; set; }
    public int? Doors { get; set; }
    public int? BootSpace { get; set; }
    public double? TankSize { get; set; }

    // DYNAMICS
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
    public string? Spare { get; set; }

    public string? PoweredSteering { get; set; }
    public string? SteeringType { get; set; }

    // SAFETY
    public bool? Abs { get; set; }
    public bool? TractionControl { get; set; }
    public int? Airbags { get; set; }
    public bool? HillAssist { get; set; }
    public bool? RearViewCamera { get; set; }
    public bool? ParkingSensors { get; set; }
    public bool? CruiseControl { get; set; }

    // FEATURES
    public double? DisplaySize { get; set; }
    public bool? Bluetooth { get; set; }
    public bool? Navigation { get; set; }
    public bool? SmartConnectivity { get; set; }
    public bool? FastChargingPort { get; set; }
}
