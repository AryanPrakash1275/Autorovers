namespace Autorovers.Application.Vehicles;

public sealed class VehicleDetailsDto
{
    // META
    public string? ColorsAvailableJson { get; init; }
    public int? WarrantyYears { get; init; }
    public int? ServiceIntervalKm { get; init; }

    // ENGINE (ICE)
    public string? EngineType { get; init; }
    public int? EngineDisplacement { get; init; }
    public string? InductionType { get; init; }
    public double? Power { get; init; }
    public int? PowerRpm { get; init; }
    public double? Torque { get; init; }
    public int? TorqueRpm { get; init; }
    public string? Emission { get; init; }
    public double? Mileage { get; init; }
    public double? Range { get; init; }
    public string? FuelType { get; init; }

    // EV
    public double? BatteryCapacity { get; init; }
    public double? ChargingTimeFast { get; init; }
    public double? ChargingTimeNormal { get; init; }
    public int? MotorPower { get; init; }
    public int? MotorTorque { get; init; }
    public bool? FastChargingPort { get; init; }
    public double? EvRange { get; init; }

    // DIMENSIONS
    public int? Length { get; init; }
    public int? Width { get; init; }
    public int? Height { get; init; }
    public int? Weight { get; init; }
    public int? GroundClearance { get; init; }
    public int? WheelBase { get; init; }
    public double? TurningRadius { get; init; }

    // DYNAMICS / TYRES / BRAKES / SUSPENSION
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

    // BIKE-ONLY
    public int? NumberOfGears { get; init; }
    public double? TankSize { get; init; }
    public bool? Abs { get; init; }
    public bool? TractionControl { get; init; }
    public double? BikeDisplaySize { get; init; }
    public bool? BikeBluetooth { get; init; }
    public bool? BikeNavigation { get; init; }
    public bool? BikeSmartConnectivity { get; init; }

    // CAR-ONLY
    public string? DriveType { get; init; }
    public double? ZeroToHundred { get; init; }
    public int? TopSpeed { get; init; }
    public int? PersonCapacity { get; init; }
    public int? Rows { get; init; }
    public int? Doors { get; init; }
    public int? BootSpace { get; init; }
    public bool? PoweredSteering { get; init; }
    public string? SteeringType { get; init; }
    public bool? HasSpareWheel { get; init; }
    public int? Airbags { get; init; }
    public bool? HillAssist { get; init; }
    public bool? RearViewCamera { get; init; }
    public bool? ParkingSensors { get; init; }
    public bool? CruiseControl { get; init; }
    public double? CarDisplaySize { get; init; }
    public bool? CarBluetooth { get; init; }
    public bool? CarNavigation { get; init; }
    public bool? CarSmartConnectivity { get; init; }
}
