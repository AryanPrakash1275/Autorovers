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
    // CREATE
    // =====================================================
    public async Task<int> CreateVehicleAsync(CreateVehicleRequest r)
    {
        var slug = string.IsNullOrWhiteSpace(r.Slug)
            ? Vehicle.GenerateSlug(r.Brand, r.Model, r.Variant ?? string.Empty)
            : r.Slug.Trim();

        var kind = ParseKindOrThrow(r.VehicleType);

        var vehicle = new Vehicle
        {
            Kind = kind,

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

        _db.Vehicles.Add(vehicle);

        var details = new VehicleDetails
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

        NormalizeByKind(vehicle.Kind, details);

        _db.VehicleDetails.Add(details);
        await _db.SaveChangesAsync();

        return vehicle.Id;
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
                Slug = v.Slug!,
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
                Slug = v.Slug!,
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
                Slug = v.Slug!,
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

        entity.Slug = SlugHelper.ToSlug(
            request.Brand!,
            request.Model!,
            request.Variant ?? string.Empty
        ) + "-" + id;

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
                Slug = v.Slug!,
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
                Slug = v.Slug!,
                ImageUrl = v.ImageUrl,
                Details = MapDetailsToFlatDto(v, v.Details)
            })
            .FirstOrDefaultAsync();
    }

    // =====================================================
    // UPDATE DETAILS
    // =====================================================
    public async Task UpdateVehicleDetailsAsync(int id, UpdateVehicleDetailsRequest req)
    {
        var details = await _db.VehicleDetails
            .Include(d => d.Vehicle)
            .FirstOrDefaultAsync(d => d.VehicleId == id);

        if (details is null) throw new KeyNotFoundException();

        details.ColorsAvailableJson = req.ColorsAvailableJson ?? details.ColorsAvailableJson;
        details.WarrantyYears = req.WarrantyYears ?? details.WarrantyYears;
        details.ServiceIntervalKm = req.ServiceIntervalKm ?? details.ServiceIntervalKm;

        if (details.Vehicle != null)
            NormalizeByKind(details.Vehicle.Kind, details);

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
}
