namespace Autorovers.Application.Vehicles;

public class UpdateVehicleDetailsRequest
{
    public string? ImageUrl { get; init; }
    public string? EngineType { get; set; }
    public string? Specification { get; set; }
    public string? InductionType { get; set; }
    public double? Power { get; set; }
    public double? Torque { get; set; }
    public string? Emission { get; set; }
    public double? Mileage { get; set; }
    public string? AutoStartStop { get; set; }
    public int? Range { get; set; }

    public int? Length { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Weight { get; set; }
    public int? GroundClearance { get; set; }
    public int? WheelBase { get; set; }

    public int? PersonCapacity { get; set; }
    public int? Rows { get; set; }
    public int? Doors { get; set; }
    public int? BootSpace { get; set; }
    public double? TankSize { get; set; }

    public string? FrontTyre { get; set; }
    public string? BackType { get; set; }
    public string? FrontBrake { get; set; }
    public string? BackBrake { get; set; }
    public string? PoweredSteering { get; set; }
    public string? TyreSizeFront { get; set; }
    public string? TyreSizeBack { get; set; }
    public string? TyreType { get; set; }
    public string? WheelMaterial { get; set; }
    public string? Spare { get; set; }
}
