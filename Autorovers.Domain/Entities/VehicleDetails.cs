using System.ComponentModel.DataAnnotations.Schema;

namespace Autorovers.Domain.Entities;

public class VehicleDetails
{
    // Primary key AND foreign key to Vehicle, this will setup my EF to 1:1 relation with vehicle
    [ForeignKey("Vehicle")]
    public int VehicleId { get; set; }

    public Vehicle Vehicle { get; set; }

    // Engine Specs
    public string EngineType { get; set; } = null!;
    public string Specification { get; set; } = null!;
    public string InductionType { get; set; } = null!;
    public int Power { get; set; }
    public int Torque { get; set; }
    public string Emission { get; set; } = null!;
    public int Mileage { get; set; }
    public string AutoStartStop { get; set; } = "";

    public int Range { get; set; }
    public string Transmission { get; set; } = null!;

    // Size
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public int GroundClearance { get; set; }
    public int WheelBase { get; set; }

    // Capacity
    public int PersonCapacity { get; set; }
    public int Rows { get; set; }
    public int Doors { get; set; }
    public int BootSpace { get; set; }
    public int TankSize { get; set; }

    // Dynamics
    public string FrontTyre { get; set; } = null!;
    public string BackType { get; set; } = null!;
    public string FrontBrake { get; set; } = null!;
    public string BackBrake { get; set; } = null!;
    public string PoweredSteering { get; set; } = null!;
    public string TyreSizeFront { get; set; } = null!;
    public string TyreSizeBack { get; set; } = null!;
    public string TyreType { get; set; } = null!;
    public string WheelMaterial { get; set; } = null!;
    public string Spare { get; set; } = null!;
}
