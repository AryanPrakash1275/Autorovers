using System.ComponentModel.DataAnnotations.Schema;

namespace Autorovers.Domain.Entities;

public class VehicleDetails
{
    [ForeignKey(nameof(Vehicle))]
    public int VehicleId { get; set; }

    public Vehicle Vehicle { get; set; } = null!;

    // =========================
    // SHARED (both bike & car)
    // =========================
    public EngineSpecs? Engine { get; set; }          // ICE / Hybrid engine info
    public EvSpecs? Ev { get; set; }                  // EV / Hybrid electrical info

    public DimensionsSpecs? Dimensions { get; set; }
    public TyresBrakesSuspensionSpecs? Dynamics { get; set; }

    // =========================
    // BIKE-ONLY
    // =========================
    public BikeSpecs? Bike { get; set; }

    // =========================
    // CAR-ONLY
    // =========================
    public CarSpecs? Car { get; set; }

    // =========================
    // META
    // =========================
    public string? ColorsAvailableJson { get; set; }
    public int? WarrantyYears { get; set; }
    public int? ServiceIntervalKm { get; set; }

    // -------------------------
    // Validation helper (optional to call from app layer)
    // -------------------------
    public void AssertMatchesVehicleKind()
    {
        if (Vehicle == null) return;

        if (Vehicle.Kind == VehicleKind.Bike)
        {
            if (Car != null) throw new InvalidOperationException("Car specs must be null for Bike vehicles.");
        }

        if (Vehicle.Kind == VehicleKind.Car)
        {
            if (Bike != null) throw new InvalidOperationException("Bike specs must be null for Car vehicles.");
        }
    }

    public void AssertMatchesPowertrain()
    {
        if (Vehicle == null) return;

        if (Vehicle.Powertrain == PowertrainKind.Ev)
        {
            if (Ev == null) throw new InvalidOperationException("EV vehicles must have Ev specs.");
            // Engine can still be null (pure EV)
        }

        if (Vehicle.Powertrain == PowertrainKind.Ice)
        {
            // ICE can have Engine, but should not require Ev
        }
    }
}

// ========== SHARED ==========
public class EngineSpecs
{
    public string? EngineType { get; set; }           // e.g., "Single-cylinder", "Inline-4"
    public int? EngineDisplacement { get; set; }      // cc
    public string? InductionType { get; set; }        // NA / Turbo / Supercharged
    public string? Emission { get; set; }             // BS6.2 etc.

    public double? Power { get; set; }                // bhp/ps
    public int? PowerRpm { get; set; }
    public double? Torque { get; set; }
    public int? TorqueRpm { get; set; }

    public double? Mileage { get; set; }              // km/l (or your unit)
    public double? Range { get; set; }                // km (can be used for ICE too if you want)
    public string? FuelType { get; set; }             // Petrol/Diesel/CNG
}

public class EvSpecs
{
    public double? BatteryCapacity { get; set; }      // kWh
    public double? ChargingTimeFast { get; set; }     // hours
    public double? ChargingTimeNormal { get; set; }   // hours

    public int? MotorPower { get; set; }              // (choose unit & stick to it)
    public int? MotorTorque { get; set; }

    public bool? FastChargingPort { get; set; }
    public double? Range { get; set; }                // km
}

public class DimensionsSpecs
{
    public int? Length { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? WheelBase { get; set; }
    public int? GroundClearance { get; set; }
    public int? Weight { get; set; }
    public double? TurningRadius { get; set; }
}

public class TyresBrakesSuspensionSpecs
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

// ========== BIKE ==========
public class BikeSpecs
{
    public int? NumberOfGears { get; set; }
    public double? TankSize { get; set; }             // liters

    // Bike safety / assists
    public bool? Abs { get; set; }
    public bool? TractionControl { get; set; }

    // Bike features (keep small; don’t copy car stuff)
    public double? DisplaySize { get; set; }
    public bool? Bluetooth { get; set; }
    public bool? Navigation { get; set; }
    public bool? SmartConnectivity { get; set; }
}

// ========== CAR ==========
public class CarSpecs
{
    // Performance
    public string? DriveType { get; set; }            // FWD/RWD/AWD
    public double? ZeroToHundred { get; set; }
    public int? TopSpeed { get; set; }

    // Capacity
    public int? PersonCapacity { get; set; }
    public int? Rows { get; set; }
    public int? Doors { get; set; }
    public int? BootSpace { get; set; }               // liters

    // Steering
    public bool? PoweredSteering { get; set; }
    public string? SteeringType { get; set; }

    // Spare (car-only)
    public bool? HasSpareWheel { get; set; }

    // Safety
    public int? Airbags { get; set; }
    public bool? HillAssist { get; set; }
    public bool? RearViewCamera { get; set; }
    public bool? ParkingSensors { get; set; }
    public bool? CruiseControl { get; set; }

    // Infotainment/features
    public double? DisplaySize { get; set; }
    public bool? Bluetooth { get; set; }
    public bool? Navigation { get; set; }
    public bool? SmartConnectivity { get; set; }
}
