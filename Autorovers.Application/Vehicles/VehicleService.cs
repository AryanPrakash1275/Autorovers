using Autorovers.Application.Abstractions.Data;
using Autorovers.Domain.Entities;
using Autorovers.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Autorovers.Application.Vehicles;

public class VehicleService : IVehicleService
{
    private readonly IApplicationDbContext _db;

    public VehicleService(IApplicationDbContext db)
    {
        _db = db;
    }

    // =====================================================
    // CREATE (single)
    // =====================================================
    public async Task<int> CreateVehicleAsync(CreateVehicleRequest r)
    {
        ValidateCreatePayload(r);

        var slug = string.IsNullOrWhiteSpace(r.Slug)
            ? Vehicle.GenerateSlug(r.Brand, r.Model, r.Variant ?? string.Empty)
            : r.Slug.Trim();

        var kind = ParseKindOrThrow(r.VehicleType);
        var powertrain = DerivePowertrainOrThrow(r);

        var vehicle = new Vehicle
        {
            Kind = kind,
            Powertrain = powertrain,

            Brand = r.Brand!,
            Model = r.Model!,
            Variant = r.Variant,
            Year = r.Year,
            Category = r.Category!,
            Price = r.Price,
            Transmission = r.Transmission!,
            Slug = slug,
            ImageUrl = r.ImageUrl
        };

        var details = BuildDetails(vehicle, r);

        NormalizeByKind(vehicle.Kind, details);

        // EV cannot have ICE engine specs
        if (vehicle.Powertrain == PowertrainKind.Ev)
            details.Engine = null;

        _db.Vehicles.Add(vehicle);
        _db.VehicleDetails.Add(details);

        await _db.SaveChangesAsync();
        return vehicle.Id;
    }

    // =====================================================
    // CREATE (bulk) - REAL bulk: one SaveChanges
    // =====================================================
    public async Task<IReadOnlyList<int>> BulkCreateAsync(List<CreateVehicleRequest> requests)
    {
        if (requests == null || requests.Count == 0)
            throw new ArgumentException("Request body must be a non-empty array of vehicles.");

        var vehicles = new List<Vehicle>(requests.Count);
        var detailsList = new List<VehicleDetails>(requests.Count);

        foreach (var r in requests)
        {
            ValidateCreatePayload(r);

            var slug = string.IsNullOrWhiteSpace(r.Slug)
                ? Vehicle.GenerateSlug(r.Brand, r.Model, r.Variant ?? string.Empty)
                : r.Slug.Trim();

            var kind = ParseKindOrThrow(r.VehicleType);
            var powertrain = DerivePowertrainOrThrow(r);

            var vehicle = new Vehicle
            {
                Kind = kind,
                Powertrain = powertrain,

                Brand = r.Brand!,
                Model = r.Model!,
                Variant = r.Variant,
                Year = r.Year,
                Category = r.Category!,
                Price = r.Price,
                Transmission = r.Transmission!,
                Slug = slug,
                ImageUrl = r.ImageUrl
            };

            var details = BuildDetails(vehicle, r);

            NormalizeByKind(vehicle.Kind, details);

            if (vehicle.Powertrain == PowertrainKind.Ev)
                details.Engine = null;

            vehicles.Add(vehicle);
            detailsList.Add(details);
        }

        _db.Vehicles.AddRange(vehicles);
        _db.VehicleDetails.AddRange(detailsList);

        await _db.SaveChangesAsync();

        return vehicles.Select(v => v.Id).ToList();
    }

    // =====================================================
    // READ LIST (ADMIN / SUMMARY)
    // =====================================================
    public async Task<IReadOnlyList<VehicleListItemDto>> GetAllAsync()
    {
        return await _db.Vehicles
            .OrderBy(v => v.Brand)
            .ThenBy(v => v.Model)
            .Select(v => new VehicleListItemDto
            {
                Id = v.Id,
                Brand = v.Brand!,
                Model = v.Model!,
                Variant = v.Variant,
                Year = v.Year,
                Price = v.Price,
                Category = v.Category!,
                Transmission = v.Transmission!,
                Slug = v.Slug,
                ImageUrl = v.ImageUrl
            })
            .ToListAsync();
    }

    public async Task<VehicleListItemDto?> GetByIdAsync(int id)
    {
        return await _db.Vehicles
            .Where(v => v.Id == id)
            .Select(v => new VehicleListItemDto
            {
                Id = v.Id,
                Brand = v.Brand!,
                Model = v.Model!,
                Variant = v.Variant,
                Year = v.Year,
                Price = v.Price,
                Category = v.Category!,
                Transmission = v.Transmission!,
                Slug = v.Slug,
                ImageUrl = v.ImageUrl
            })
            .FirstOrDefaultAsync();
    }

    public async Task<VehicleListItemDto?> GetBySlugAsync(string slug)
    {
        return await _db.Vehicles
            .Where(v => v.Slug == slug)
            .Select(v => new VehicleListItemDto
            {
                Id = v.Id,
                Brand = v.Brand!,
                Model = v.Model!,
                Variant = v.Variant,
                Year = v.Year,
                Price = v.Price,
                Category = v.Category!,
                Transmission = v.Transmission!,
                Slug = v.Slug,
                ImageUrl = v.ImageUrl
            })
            .FirstOrDefaultAsync();
    }

    // =====================================================
    // UPDATE CORE
    // =====================================================
    public async Task UpdateVehicleAsync(int id, UpdateVehicleRequest request)
    {
        var entity = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
        if (entity is null) throw new KeyNotFoundException();

        entity.Brand = request.Brand!;
        entity.Model = request.Model!;
        entity.Variant = request.Variant;
        entity.Year = request.Year;
        entity.Price = request.Price;
        entity.Category = request.Category!;
        entity.Transmission = request.Transmission!;
        entity.ImageUrl = request.ImageUrl;

        // ✅ Stop changing slug format on update.
        // Keep stable; only generate if missing.
        if (string.IsNullOrWhiteSpace(entity.Slug))
            entity.Slug = Vehicle.GenerateSlug(entity.Brand, entity.Model, entity.Variant ?? string.Empty);

        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteVehicleAsync(int id)
    {
        var entity = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
        if (entity is null) return false;

        _db.Vehicles.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }

    // =====================================================
    // READ WITH DETAILS
    // =====================================================
    public async Task<VehicleWithDetailsDto?> GetWithDetailsByIdAsync(int id)
    {
        return await _db.Vehicles
            .Include(v => v.Details)
            .Where(v => v.Id == id)
            .Select(v => new VehicleWithDetailsDto
            {
                Id = v.Id,
                Brand = v.Brand!,
                Model = v.Model!,
                Variant = v.Variant,
                Year = v.Year,
                Price = v.Price,
                Category = v.Category!,
                Transmission = v.Transmission!,
                Slug = v.Slug,
                ImageUrl = v.ImageUrl,
                Details = MapDetailsToFlatDto(v, v.Details)
            })
            .FirstOrDefaultAsync();
    }

    public async Task<VehicleWithDetailsDto?> GetWithDetailsBySlugAsync(string slug)
    {
        return await _db.Vehicles
            .Include(v => v.Details)
            .Where(v => v.Slug == slug)
            .Select(v => new VehicleWithDetailsDto
            {
                Id = v.Id,
                Brand = v.Brand!,
                Model = v.Model!,
                Variant = v.Variant,
                Year = v.Year,
                Price = v.Price,
                Category = v.Category!,
                Transmission = v.Transmission!,
                Slug = v.Slug,
                ImageUrl = v.ImageUrl,
                Details = MapDetailsToFlatDto(v, v.Details)
            })
            .FirstOrDefaultAsync();
    }

    // =====================================================
    // UPDATE DETAILS (PATCH nested contract)
    // =====================================================
    public async Task UpdateVehicleDetailsAsync(int id, UpdateVehicleDetailsRequest req)
    {
        var details = await _db.VehicleDetails
            .Include(d => d.Vehicle)
            .Include(d => d.Engine)
            .Include(d => d.Ev)
            .Include(d => d.Dimensions)
            .Include(d => d.Dynamics)
            .Include(d => d.Bike)
            .Include(d => d.Car)
            .FirstOrDefaultAsync(d => d.VehicleId == id);

        if (details is null) throw new KeyNotFoundException();

        // ===== META patch =====
        if (req.ColorsAvailableJson is not null) details.ColorsAvailableJson = req.ColorsAvailableJson;
        if (req.WarrantyYears.HasValue) details.WarrantyYears = req.WarrantyYears;
        if (req.ServiceIntervalKm.HasValue) details.ServiceIntervalKm = req.ServiceIntervalKm;

        // ===== RULES: Engine XOR EV =====
        if (req.Engine is not null && req.Ev is not null)
            throw new ArgumentException("Invalid payload: provide either Engine or Ev, not both.");

        if (req.Engine is not null)
        {
            details.Engine ??= new EngineSpecs();
            var e = req.Engine;

            if (e.EngineType is not null) details.Engine.EngineType = e.EngineType;
            if (e.EngineDisplacement.HasValue) details.Engine.EngineDisplacement = e.EngineDisplacement;
            if (e.InductionType is not null) details.Engine.InductionType = e.InductionType;
            if (e.Emission is not null) details.Engine.Emission = e.Emission;

            if (e.Power.HasValue) details.Engine.Power = e.Power;
            if (e.PowerRpm.HasValue) details.Engine.PowerRpm = e.PowerRpm;
            if (e.Torque.HasValue) details.Engine.Torque = e.Torque;
            if (e.TorqueRpm.HasValue) details.Engine.TorqueRpm = e.TorqueRpm;

            if (e.Mileage.HasValue) details.Engine.Mileage = e.Mileage;
            if (e.Range.HasValue) details.Engine.Range = e.Range;
            if (e.FuelType is not null) details.Engine.FuelType = e.FuelType;

            // switching to engine => wipe EV
            details.Ev = null;

            // update vehicle powertrain
            if (details.Vehicle != null)
            {
                var fuel = (details.Engine.FuelType ?? "").Trim().ToLowerInvariant();
                details.Vehicle.Powertrain = fuel.Contains("hybrid")
                    ? PowertrainKind.Hybrid
                    : PowertrainKind.Ice;
            }
        }

        if (req.Ev is not null)
        {
            details.Ev ??= new EvSpecs();
            var e = req.Ev;

            if (e.BatteryCapacity.HasValue) details.Ev.BatteryCapacity = e.BatteryCapacity;
            if (e.ChargingTimeFast.HasValue) details.Ev.ChargingTimeFast = e.ChargingTimeFast;
            if (e.ChargingTimeNormal.HasValue) details.Ev.ChargingTimeNormal = e.ChargingTimeNormal;
            if (e.MotorPower.HasValue) details.Ev.MotorPower = e.MotorPower;
            if (e.MotorTorque.HasValue) details.Ev.MotorTorque = e.MotorTorque;
            if (e.FastChargingPort.HasValue) details.Ev.FastChargingPort = e.FastChargingPort;
            if (e.Range.HasValue) details.Ev.Range = e.Range;

            // switching to EV => wipe engine
            details.Engine = null;

            if (details.Vehicle != null)
                details.Vehicle.Powertrain = PowertrainKind.Ev;
        }

        // ===== DIMENSIONS patch =====
        if (req.Dimensions is not null)
        {
            details.Dimensions ??= new DimensionsSpecs();
            var d = req.Dimensions;

            if (d.Length.HasValue) details.Dimensions.Length = d.Length;
            if (d.Width.HasValue) details.Dimensions.Width = d.Width;
            if (d.Height.HasValue) details.Dimensions.Height = d.Height;
            if (d.WheelBase.HasValue) details.Dimensions.WheelBase = d.WheelBase;
            if (d.GroundClearance.HasValue) details.Dimensions.GroundClearance = d.GroundClearance;
            if (d.Weight.HasValue) details.Dimensions.Weight = d.Weight;
            if (d.TurningRadius.HasValue) details.Dimensions.TurningRadius = d.TurningRadius;
        }

        // ===== DYNAMICS patch =====
        if (req.Dynamics is not null)
        {
            details.Dynamics ??= new TyresBrakesSuspensionSpecs();
            var d = req.Dynamics;

            if (d.FrontType is not null) details.Dynamics.FrontType = d.FrontType;
            if (d.BackType is not null) details.Dynamics.BackType = d.BackType;
            if (d.FrontBrake is not null) details.Dynamics.FrontBrake = d.FrontBrake;
            if (d.BackBrake is not null) details.Dynamics.BackBrake = d.BackBrake;
            if (d.FrontSuspension is not null) details.Dynamics.FrontSuspension = d.FrontSuspension;
            if (d.RearSuspension is not null) details.Dynamics.RearSuspension = d.RearSuspension;
            if (d.TyreSizeFront is not null) details.Dynamics.TyreSizeFront = d.TyreSizeFront;
            if (d.TyreSizeBack is not null) details.Dynamics.TyreSizeBack = d.TyreSizeBack;
            if (d.TyreType is not null) details.Dynamics.TyreType = d.TyreType;
            if (d.WheelMaterial is not null) details.Dynamics.WheelMaterial = d.WheelMaterial;
        }

        // ===== RULES: Bike XOR Car =====
        if (req.Bike is not null && req.Car is not null)
            throw new ArgumentException("Invalid payload: provide either Bike or Car, not both.");

        if (req.Bike is not null)
        {
            details.Bike ??= new BikeSpecs();
            var b = req.Bike;

            if (b.NumberOfGears.HasValue) details.Bike.NumberOfGears = b.NumberOfGears;
            if (b.TankSize.HasValue) details.Bike.TankSize = b.TankSize;

            if (b.Abs.HasValue) details.Bike.Abs = b.Abs;
            if (b.TractionControl.HasValue) details.Bike.TractionControl = b.TractionControl;

            if (b.DisplaySize.HasValue) details.Bike.DisplaySize = b.DisplaySize;
            if (b.Bluetooth.HasValue) details.Bike.Bluetooth = b.Bluetooth;
            if (b.Navigation.HasValue) details.Bike.Navigation = b.Navigation;
            if (b.SmartConnectivity.HasValue) details.Bike.SmartConnectivity = b.SmartConnectivity;

            // wipe car specs if switching to bike
            details.Car = null;
        }

        if (req.Car is not null)
        {
            details.Car ??= new CarSpecs();
            var c = req.Car;

            if (c.DriveType is not null) details.Car.DriveType = c.DriveType;
            if (c.ZeroToHundred.HasValue) details.Car.ZeroToHundred = c.ZeroToHundred;
            if (c.TopSpeed.HasValue) details.Car.TopSpeed = c.TopSpeed;

            if (c.PersonCapacity.HasValue) details.Car.PersonCapacity = c.PersonCapacity;
            if (c.Rows.HasValue) details.Car.Rows = c.Rows;
            if (c.Doors.HasValue) details.Car.Doors = c.Doors;
            if (c.BootSpace.HasValue) details.Car.BootSpace = c.BootSpace;

            if (c.PoweredSteering.HasValue) details.Car.PoweredSteering = c.PoweredSteering;
            if (c.SteeringType is not null) details.Car.SteeringType = c.SteeringType;

            if (c.HasSpareWheel.HasValue) details.Car.HasSpareWheel = c.HasSpareWheel;

            if (c.Airbags.HasValue) details.Car.Airbags = c.Airbags;
            if (c.HillAssist.HasValue) details.Car.HillAssist = c.HillAssist;
            if (c.RearViewCamera.HasValue) details.Car.RearViewCamera = c.RearViewCamera;
            if (c.ParkingSensors.HasValue) details.Car.ParkingSensors = c.ParkingSensors;
            if (c.CruiseControl.HasValue) details.Car.CruiseControl = c.CruiseControl;

            if (c.DisplaySize.HasValue) details.Car.DisplaySize = c.DisplaySize;
            if (c.Bluetooth.HasValue) details.Car.Bluetooth = c.Bluetooth;
            if (c.Navigation.HasValue) details.Car.Navigation = c.Navigation;
            if (c.SmartConnectivity.HasValue) details.Car.SmartConnectivity = c.SmartConnectivity;

            // wipe bike specs if switching to car
            details.Bike = null;
        }

        // Normalize by kind (your existing rule)
        if (details.Vehicle != null)
            NormalizeByKind(details.Vehicle.Kind, details);

        // EV cannot have ICE engine specs (your rule)
        if (details.Vehicle?.Powertrain == PowertrainKind.Ev)
            details.Engine = null;

        await _db.SaveChangesAsync();
    }

    // =====================================================
    // MAPPING (IMMUTABLE DTO)
    // =====================================================
    private static VehicleDetailsDto MapDetailsToFlatDto(Vehicle v, VehicleDetails? d)
    {
        return new VehicleDetailsDto
        {
            ColorsAvailableJson = d?.ColorsAvailableJson,
            WarrantyYears = d?.WarrantyYears,
            ServiceIntervalKm = d?.ServiceIntervalKm,

            EngineType = d?.Engine?.EngineType,
            EngineDisplacement = d?.Engine?.EngineDisplacement,
            InductionType = d?.Engine?.InductionType,
            Power = d?.Engine?.Power,
            PowerRpm = d?.Engine?.PowerRpm,
            Torque = d?.Engine?.Torque,
            TorqueRpm = d?.Engine?.TorqueRpm,
            Emission = d?.Engine?.Emission,
            Mileage = d?.Engine?.Mileage,
            Range = d?.Engine?.Range,
            FuelType = d?.Engine?.FuelType,

            BatteryCapacity = d?.Ev?.BatteryCapacity,
            ChargingTimeFast = d?.Ev?.ChargingTimeFast,
            ChargingTimeNormal = d?.Ev?.ChargingTimeNormal,
            MotorPower = d?.Ev?.MotorPower,
            MotorTorque = d?.Ev?.MotorTorque,
            FastChargingPort = d?.Ev?.FastChargingPort,
            EvRange = d?.Ev?.Range,

            Length = d?.Dimensions?.Length,
            Width = d?.Dimensions?.Width,
            Height = d?.Dimensions?.Height,
            Weight = d?.Dimensions?.Weight,
            GroundClearance = d?.Dimensions?.GroundClearance,
            WheelBase = d?.Dimensions?.WheelBase,
            TurningRadius = d?.Dimensions?.TurningRadius,

            FrontType = d?.Dynamics?.FrontType,
            BackType = d?.Dynamics?.BackType,
            FrontBrake = d?.Dynamics?.FrontBrake,
            BackBrake = d?.Dynamics?.BackBrake,
            FrontSuspension = d?.Dynamics?.FrontSuspension,
            RearSuspension = d?.Dynamics?.RearSuspension,
            TyreSizeFront = d?.Dynamics?.TyreSizeFront,
            TyreSizeBack = d?.Dynamics?.TyreSizeBack,
            TyreType = d?.Dynamics?.TyreType,
            WheelMaterial = d?.Dynamics?.WheelMaterial,

            NumberOfGears = d?.Bike?.NumberOfGears,
            TankSize = d?.Bike?.TankSize,
            Abs = d?.Bike?.Abs,
            TractionControl = d?.Bike?.TractionControl,
            BikeDisplaySize = d?.Bike?.DisplaySize,
            BikeBluetooth = d?.Bike?.Bluetooth,
            BikeNavigation = d?.Bike?.Navigation,
            BikeSmartConnectivity = d?.Bike?.SmartConnectivity,

            DriveType = d?.Car?.DriveType,
            ZeroToHundred = d?.Car?.ZeroToHundred,
            TopSpeed = d?.Car?.TopSpeed,
            PersonCapacity = d?.Car?.PersonCapacity,
            Rows = d?.Car?.Rows,
            Doors = d?.Car?.Doors,
            BootSpace = d?.Car?.BootSpace,
            PoweredSteering = d?.Car?.PoweredSteering,
            SteeringType = d?.Car?.SteeringType,
            HasSpareWheel = d?.Car?.HasSpareWheel,
            Airbags = d?.Car?.Airbags,
            HillAssist = d?.Car?.HillAssist,
            RearViewCamera = d?.Car?.RearViewCamera,
            ParkingSensors = d?.Car?.ParkingSensors,
            CruiseControl = d?.Car?.CruiseControl,
            CarDisplaySize = d?.Car?.DisplaySize,
            CarBluetooth = d?.Car?.Bluetooth,
            CarNavigation = d?.Car?.Navigation,
            CarSmartConnectivity = d?.Car?.SmartConnectivity
        };
    }

    // =====================================================
    // RULES / HELPERS
    // =====================================================
    private static void NormalizeByKind(VehicleKind kind, VehicleDetails details)
    {
        if (kind == VehicleKind.Bike) details.Car = null;
        if (kind == VehicleKind.Car) details.Bike = null;
    }

    private static VehicleKind ParseKindOrThrow(string? vehicleType)
    {
        if (string.IsNullOrWhiteSpace(vehicleType))
            throw new ArgumentException("VehicleType is required and must be 'Bike' or 'Car'.");

        return vehicleType.Trim().ToLowerInvariant() switch
        {
            "bike" => VehicleKind.Bike,
            "car" => VehicleKind.Car,
            _ => throw new ArgumentException("VehicleType must be 'Bike' or 'Car'.")
        };
    }

    private static void ValidateCreatePayload(CreateVehicleRequest r)
    {
        var kind = ParseKindOrThrow(r.VehicleType);

        if (kind == VehicleKind.Bike && r.Car != null)
            throw new ArgumentException("Invalid payload: Bike vehicles must not include Car specs.");

        if (kind == VehicleKind.Car && r.Bike != null)
            throw new ArgumentException("Invalid payload: Car vehicles must not include Bike specs.");

        var hasEv = r.Ev != null;
        var hasEngine = r.Engine != null;

        if (hasEv && hasEngine)
            throw new ArgumentException("Invalid payload: provide either Ev specs OR Engine specs, not both.");

        if (!hasEv && !hasEngine)
            throw new ArgumentException("Invalid payload: provide Ev specs or Engine specs.");

        if (hasEngine && string.IsNullOrWhiteSpace(r.Engine!.FuelType))
            throw new ArgumentException("Invalid payload: Engine.FuelType is required for non-EV vehicles.");
    }

    private static PowertrainKind DerivePowertrainOrThrow(CreateVehicleRequest r)
    {
        var hasEv = r.Ev != null;
        var hasEngine = r.Engine != null;

        if (hasEv && hasEngine)
            throw new ArgumentException("Invalid powertrain payload: provide either Ev specs OR Engine specs, not both.");

        if (!hasEv && !hasEngine)
            throw new ArgumentException("Powertrain cannot be derived. Provide Ev specs or Engine specs.");

        if (hasEv)
            return PowertrainKind.Ev;

        var fuel = r.Engine?.FuelType?.Trim().ToLowerInvariant();
        if (!string.IsNullOrWhiteSpace(fuel) && fuel.Contains("hybrid"))
            return PowertrainKind.Hybrid;

        if (string.IsNullOrWhiteSpace(r.Engine?.FuelType))
            throw new ArgumentException("ICE powertrain requires Engine.FuelType.");

        return PowertrainKind.Ice;
    }

    private static VehicleDetails BuildDetails(Vehicle vehicle, CreateVehicleRequest r)
    {
        return new VehicleDetails
        {
            Vehicle = vehicle,

            Engine = r.Engine == null ? null : new EngineSpecs
            {
                EngineType = r.Engine.EngineType,
                EngineDisplacement = r.Engine.EngineDisplacement,
                InductionType = r.Engine.InductionType,
                Emission = r.Engine.Emission,
                Power = r.Engine.Power,
                PowerRpm = r.Engine.PowerRpm,
                Torque = r.Engine.Torque,
                TorqueRpm = r.Engine.TorqueRpm,
                Mileage = r.Engine.Mileage,
                Range = r.Engine.Range,
                FuelType = r.Engine.FuelType
            },

            Ev = r.Ev == null ? null : new EvSpecs
            {
                BatteryCapacity = r.Ev.BatteryCapacity,
                ChargingTimeFast = r.Ev.ChargingTimeFast,
                ChargingTimeNormal = r.Ev.ChargingTimeNormal,
                MotorPower = r.Ev.MotorPower,
                MotorTorque = r.Ev.MotorTorque,
                FastChargingPort = r.Ev.FastChargingPort,
                Range = r.Ev.Range
            },

            Dimensions = r.Dimensions == null ? null : new DimensionsSpecs
            {
                Length = r.Dimensions.Length,
                Width = r.Dimensions.Width,
                Height = r.Dimensions.Height,
                Weight = r.Dimensions.Weight,
                GroundClearance = r.Dimensions.GroundClearance,
                WheelBase = r.Dimensions.WheelBase,
                TurningRadius = r.Dimensions.TurningRadius
            },

            Dynamics = r.Dynamics == null ? null : new TyresBrakesSuspensionSpecs
            {
                FrontType = r.Dynamics.FrontType,
                BackType = r.Dynamics.BackType,
                FrontBrake = r.Dynamics.FrontBrake,
                BackBrake = r.Dynamics.BackBrake,
                FrontSuspension = r.Dynamics.FrontSuspension,
                RearSuspension = r.Dynamics.RearSuspension,
                TyreSizeFront = r.Dynamics.TyreSizeFront,
                TyreSizeBack = r.Dynamics.TyreSizeBack,
                TyreType = r.Dynamics.TyreType,
                WheelMaterial = r.Dynamics.WheelMaterial
            },

            Bike = r.Bike == null ? null : new BikeSpecs
            {
                NumberOfGears = r.Bike.NumberOfGears,
                TankSize = r.Bike.TankSize,
                Abs = r.Bike.Abs,
                TractionControl = r.Bike.TractionControl,
                DisplaySize = r.Bike.DisplaySize,
                Bluetooth = r.Bike.Bluetooth,
                Navigation = r.Bike.Navigation,
                SmartConnectivity = r.Bike.SmartConnectivity
            },

            Car = r.Car == null ? null : new CarSpecs
            {
                DriveType = r.Car.DriveType,
                ZeroToHundred = r.Car.ZeroToHundred,
                TopSpeed = r.Car.TopSpeed,
                PersonCapacity = r.Car.PersonCapacity,
                Rows = r.Car.Rows,
                Doors = r.Car.Doors,
                BootSpace = r.Car.BootSpace,
                PoweredSteering = r.Car.PoweredSteering,
                SteeringType = r.Car.SteeringType,
                HasSpareWheel = r.Car.HasSpareWheel,
                Airbags = r.Car.Airbags,
                HillAssist = r.Car.HillAssist,
                RearViewCamera = r.Car.RearViewCamera,
                ParkingSensors = r.Car.ParkingSensors,
                CruiseControl = r.Car.CruiseControl,
                DisplaySize = r.Car.DisplaySize,
                Bluetooth = r.Car.Bluetooth,
                Navigation = r.Car.Navigation,
                SmartConnectivity = r.Car.SmartConnectivity
            },

            ColorsAvailableJson = r.ColorsAvailableJson,
            WarrantyYears = r.WarrantyYears,
            ServiceIntervalKm = r.ServiceIntervalKm
        };
    }
}
