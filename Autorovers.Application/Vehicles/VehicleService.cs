using Autorovers.Application.Abstractions.Data;
using Autorovers.Domain.Entities;
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
            ? Vehicle.GenerateSlug(r.Brand, r.Model, null)
            : r.Slug.Trim();

        var kind = ParseKindOrThrow(r.VehicleType);
        var powertrain = DerivePowertrainOrThrow(r);

        var vehicle = new Vehicle
        {
            Kind = kind,
            Powertrain = powertrain,
            Brand = r.Brand!,
            Model = r.Model!,
            Year = r.Year,
            Category = r.Category!,
            StartingPrice = r.Price,
            Transmission = r.Transmission!,
            Slug = slug,
            ImageUrl = r.ImageUrl
        };

        var details = BuildDetails(vehicle, r);
        NormalizeByKind(vehicle.Kind, details);

        // if EV, engine must be null
        if (vehicle.Powertrain == PowertrainKind.Ev)
            details.Engine = null;

        _db.Vehicles.Add(vehicle);
        _db.VehicleDetails.Add(details);

        await _db.SaveChangesAsync();
        return vehicle.Id;
    }

    // =====================================================
    // CREATE (bulk)
    // =====================================================
    public async Task<IReadOnlyList<int>> BulkCreateAsync(List<CreateVehicleRequest> requests)
    {
        if (requests == null || requests.Count == 0)
            throw new ArgumentException("Request body must be a non-empty array.");

        var vehicles = new List<Vehicle>(requests.Count);
        var detailsList = new List<VehicleDetails>(requests.Count);

        foreach (var r in requests)
        {
            ValidateCreatePayload(r);

            var slug = string.IsNullOrWhiteSpace(r.Slug)
                ? Vehicle.GenerateSlug(r.Brand, r.Model, null)
                : r.Slug.Trim();

            var kind = ParseKindOrThrow(r.VehicleType);
            var powertrain = DerivePowertrainOrThrow(r);

            var vehicle = new Vehicle
            {
                Kind = kind,
                Powertrain = powertrain,
                Brand = r.Brand!,
                Model = r.Model!,
                Year = r.Year,
                Category = r.Category!,
                StartingPrice = r.Price,
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
    // READ LIST
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
                Year = v.Year,

                Price =
                    v.Variants
                        .Where(vr => vr.IsActive && vr.ExShowroomPrice != null)
                        .OrderByDescending(vr => vr.IsDefault)
                        .ThenBy(vr => vr.ExShowroomPrice)
                        .Select(vr => (int?)vr.ExShowroomPrice)
                        .FirstOrDefault()
                    ?? v.StartingPrice
                    ?? 0,

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
                Year = v.Year,

                Price =
                    v.Variants
                        .Where(vr => vr.IsActive && vr.ExShowroomPrice != null)
                        .OrderByDescending(vr => vr.IsDefault)
                        .ThenBy(vr => vr.ExShowroomPrice)
                        .Select(vr => (int?)vr.ExShowroomPrice)
                        .FirstOrDefault()
                    ?? v.StartingPrice
                    ?? 0,

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
                Year = v.Year,

                Price =
                    v.Variants
                        .Where(vr => vr.IsActive && vr.ExShowroomPrice != null)
                        .OrderByDescending(vr => vr.IsDefault)
                        .ThenBy(vr => vr.ExShowroomPrice)
                        .Select(vr => (int?)vr.ExShowroomPrice)
                        .FirstOrDefault()
                    ?? v.StartingPrice
                    ?? 0,

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
        entity.Year = request.Year;
        entity.StartingPrice = request.Price;
        entity.Category = request.Category!;
        entity.Transmission = request.Transmission!;
        entity.ImageUrl = request.ImageUrl;

        if (string.IsNullOrWhiteSpace(entity.Slug))
            entity.Slug = Vehicle.GenerateSlug(entity.Brand, entity.Model, null);

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
    // READ WITH DETAILS + VARIANTS
    // =====================================================
    public async Task<VehicleWithDetailsDto?> GetWithDetailsByIdAsync(int id)
    {
        return await _db.Vehicles
            .Where(v => v.Id == id)
            .Select(v => new VehicleWithDetailsDto
            {
                Id = v.Id,
                Brand = v.Brand!,
                Model = v.Model!,
                Year = v.Year,
                Price = v.StartingPrice ?? 0,
                Category = v.Category!,
                Transmission = v.Transmission!,
                Slug = v.Slug,
                ImageUrl = v.ImageUrl,

                Details = MapDetailsToDto(v.Details),

                Variants = v.Variants
                    .Where(vr => vr.IsActive)
                    .OrderByDescending(vr => vr.IsDefault)
                    .ThenBy(vr => vr.ExShowroomPrice ?? int.MaxValue)
                    .ThenBy(vr => vr.Name)
                    .Select(vr => new VehicleVariantDto
                    {
                        Id = vr.Id,
                        Name = vr.Name,
                        Price = vr.ExShowroomPrice ?? 0,
                        IsDefault = vr.IsDefault,
                        Addons = vr.Addons
                            .OrderBy(x => x.SortOrder)
                            .Select(x => new VariantAddonDto
                            {
                                Id = x.VariantAddonId,
                                Name = x.VariantAddon.Name,
                                Price = x.OverridePrice ?? x.VariantAddon.Price ?? 0
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<VehicleWithDetailsDto?> GetWithDetailsBySlugAsync(string slug)
    {
        return await _db.Vehicles
            .Where(v => v.Slug == slug)
            .Select(v => new VehicleWithDetailsDto
            {
                Id = v.Id,
                Brand = v.Brand!,
                Model = v.Model!,
                Year = v.Year,
                Price = v.StartingPrice ?? 0,
                Category = v.Category!,
                Transmission = v.Transmission!,
                Slug = v.Slug,
                ImageUrl = v.ImageUrl,

                Details = MapDetailsToDto(v.Details),

                Variants = v.Variants
                    .Where(vr => vr.IsActive)
                    .OrderByDescending(vr => vr.IsDefault)
                    .ThenBy(vr => vr.ExShowroomPrice ?? int.MaxValue)
                    .ThenBy(vr => vr.Name)
                    .Select(vr => new VehicleVariantDto
                    {
                        Id = vr.Id,
                        Name = vr.Name,
                        Price = vr.ExShowroomPrice ?? 0,
                        IsDefault = vr.IsDefault,
                        Addons = vr.Addons
                            .OrderBy(x => x.SortOrder)
                            .Select(x => new VariantAddonDto
                            {
                                Id = x.VariantAddonId,
                                Name = x.VariantAddon.Name,
                                Price = x.OverridePrice ?? x.VariantAddon.Price ?? 0
                            })
                            .ToList()
                    })
                    .ToList()
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

        if (req.ColorsAvailableJson is not null) details.ColorsAvailableJson = req.ColorsAvailableJson;
        if (req.WarrantyYears.HasValue) details.WarrantyYears = req.WarrantyYears;
        if (req.ServiceIntervalKm.HasValue) details.ServiceIntervalKm = req.ServiceIntervalKm;
        if (req.Description is not null) details.Description = req.Description;

        await _db.SaveChangesAsync();
    }

    // =====================================================
    // HELPERS
    // =====================================================
    private static void NormalizeByKind(VehicleKind kind, VehicleDetails details)
    {
        if (kind == VehicleKind.Bike) details.Car = null;
        if (kind == VehicleKind.Car) details.Bike = null;
    }

    private static VehicleKind ParseKindOrThrow(string? vehicleType)
    {
        if (string.IsNullOrWhiteSpace(vehicleType))
            throw new ArgumentException("VehicleType is required.");

        return vehicleType.Trim().ToLowerInvariant() switch
        {
            "bike" => VehicleKind.Bike,
            "car" => VehicleKind.Car,
            _ => throw new ArgumentException("VehicleType must be Bike or Car.")
        };
    }

    private static void ValidateCreatePayload(CreateVehicleRequest r)
    {
        if (string.IsNullOrWhiteSpace(r.VehicleType))
            throw new ArgumentException("VehicleType is required.");
    }

    private static PowertrainKind DerivePowertrainOrThrow(CreateVehicleRequest r)
    {
        if (r.Ev != null) return PowertrainKind.Ev;
        if (r.Engine != null) return PowertrainKind.Ice;
        throw new ArgumentException("Powertrain cannot be derived.");
    }

    // ✅ FIXED: actually persist nested spec objects from request
    private static VehicleDetails BuildDetails(Vehicle vehicle, CreateVehicleRequest r)
    {
        return new VehicleDetails
        {
            Vehicle = vehicle,

            Description = r.Description,
            ColorsAvailableJson = r.ColorsAvailableJson,
            WarrantyYears = r.WarrantyYears,
            ServiceIntervalKm = r.ServiceIntervalKm,

            Engine = r.Engine == null ? null : new EngineSpecs
            {
                EngineType = r.Engine.EngineType,
                EngineDisplacement = r.Engine.EngineDisplacement,
                InductionType = r.Engine.InductionType,
                Emission = r.Engine.Emission,
                FuelType = r.Engine.FuelType,
                Power = r.Engine.Power,
                PowerRpm = r.Engine.PowerRpm,
                Torque = r.Engine.Torque,
                TorqueRpm = r.Engine.TorqueRpm,
                Mileage = r.Engine.Mileage,
                Range = r.Engine.Range
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
                WheelBase = r.Dimensions.WheelBase,
                GroundClearance = r.Dimensions.GroundClearance,
                Weight = r.Dimensions.Weight,
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
            }
        };
    }

    private static VehicleDetailsDto MapDetailsToDto(VehicleDetails? d)
    {
        if (d is null) return new VehicleDetailsDto();

        return new VehicleDetailsDto
        {
            Description = d.Description,
            ColorsAvailableJson = d.ColorsAvailableJson,
            WarrantyYears = d.WarrantyYears,
            ServiceIntervalKm = d.ServiceIntervalKm,

            Engine = d.Engine == null ? null : new EngineSpecsDto
            {
                EngineType = d.Engine.EngineType,
                EngineDisplacement = d.Engine.EngineDisplacement,
                InductionType = d.Engine.InductionType,
                Emission = d.Engine.Emission,
                FuelType = d.Engine.FuelType,
                Power = d.Engine.Power,
                PowerRpm = d.Engine.PowerRpm,
                Torque = d.Engine.Torque,
                TorqueRpm = d.Engine.TorqueRpm,
                Mileage = d.Engine.Mileage,
                Range = d.Engine.Range
            },

            Ev = d.Ev == null ? null : new EvSpecsDto
            {
                BatteryCapacity = d.Ev.BatteryCapacity,
                ChargingTimeFast = d.Ev.ChargingTimeFast,
                ChargingTimeNormal = d.Ev.ChargingTimeNormal,
                MotorPower = d.Ev.MotorPower,
                MotorTorque = d.Ev.MotorTorque,
                FastChargingPort = d.Ev.FastChargingPort,
                Range = d.Ev.Range
            },

            Dimensions = d.Dimensions == null ? null : new DimensionsSpecsDto
            {
                Length = d.Dimensions.Length,
                Width = d.Dimensions.Width,
                Height = d.Dimensions.Height,
                WheelBase = d.Dimensions.WheelBase,
                GroundClearance = d.Dimensions.GroundClearance,
                Weight = d.Dimensions.Weight,
                TurningRadius = d.Dimensions.TurningRadius
            },

            Dynamics = d.Dynamics == null ? null : new TyresBrakesSuspensionSpecsDto
            {
                FrontType = d.Dynamics.FrontType,
                BackType = d.Dynamics.BackType,
                FrontBrake = d.Dynamics.FrontBrake,
                BackBrake = d.Dynamics.BackBrake,
                FrontSuspension = d.Dynamics.FrontSuspension,
                RearSuspension = d.Dynamics.RearSuspension,
                TyreSizeFront = d.Dynamics.TyreSizeFront,
                TyreSizeBack = d.Dynamics.TyreSizeBack,
                TyreType = d.Dynamics.TyreType,
                WheelMaterial = d.Dynamics.WheelMaterial
            },

            Bike = d.Bike == null ? null : new BikeSpecsDto
            {
                NumberOfGears = d.Bike.NumberOfGears,
                TankSize = d.Bike.TankSize,
                Abs = d.Bike.Abs,
                TractionControl = d.Bike.TractionControl,
                DisplaySize = d.Bike.DisplaySize,
                Bluetooth = d.Bike.Bluetooth,
                Navigation = d.Bike.Navigation,
                SmartConnectivity = d.Bike.SmartConnectivity
            },

            Car = d.Car == null ? null : new CarSpecsDto
            {
                DriveType = d.Car.DriveType,
                ZeroToHundred = d.Car.ZeroToHundred,
                TopSpeed = d.Car.TopSpeed,
                PersonCapacity = d.Car.PersonCapacity,
                Rows = d.Car.Rows,
                Doors = d.Car.Doors,
                BootSpace = d.Car.BootSpace,
                PoweredSteering = d.Car.PoweredSteering,
                SteeringType = d.Car.SteeringType,
                HasSpareWheel = d.Car.HasSpareWheel,
                Airbags = d.Car.Airbags,
                HillAssist = d.Car.HillAssist,
                RearViewCamera = d.Car.RearViewCamera,
                ParkingSensors = d.Car.ParkingSensors,
                CruiseControl = d.Car.CruiseControl,
                DisplaySize = d.Car.DisplaySize,
                Bluetooth = d.Car.Bluetooth,
                Navigation = d.Car.Navigation,
                SmartConnectivity = d.Car.SmartConnectivity
            }
        };
    }
}
