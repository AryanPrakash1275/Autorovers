using Autorovers.Application.Abstractions.Data;
using Autorovers.Common;
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

        if (vehicle.Powertrain == PowertrainKind.Ev)
            details.Engine = null;

        _db.Vehicles.Add(vehicle);
        _db.VehicleDetails.Add(details);

        await _db.SaveChangesAsync();
        return vehicle.Id;
    }

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
                ImageUrl = v.ImageUrl,

                VehicleType = v.Kind == VehicleKind.Bike ? "bike" : "car",

                FuelType =
                    v.Powertrain == PowertrainKind.Ev ? "EV" :
                    v.Powertrain == PowertrainKind.Hybrid ? "Hybrid" :
                    v.Powertrain == PowertrainKind.Ice
                        ? (v.Details != null && v.Details.Engine != null && v.Details.Engine.FuelType != null
                            ? v.Details.Engine.FuelType
                            : "ICE")
                        : "ICE"
            })
            .ToListAsync();
    }

    public async Task<PagedResult<VehicleListItemDto>> GetPublicVehiclesAsync(
        PublicVehiclesQueryParams query,
        CancellationToken ct = default)
    {
        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize < 1 ? 24 : query.PageSize;
        if (pageSize > 100) pageSize = 100;

        var q = _db.Vehicles
            .AsNoTracking()
            .Select(v => new
            {
                v.Id,
                Brand = v.Brand!,
                Model = v.Model!,
                v.Year,

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
                v.Slug,
                v.ImageUrl,

                VehicleType = v.Kind == VehicleKind.Bike ? "bike" : "car",

                FuelType =
                    v.Powertrain == PowertrainKind.Ev ? "EV" :
                    v.Powertrain == PowertrainKind.Hybrid ? "Hybrid" :
                    v.Powertrain == PowertrainKind.Ice
                        ? (v.Details != null && v.Details.Engine != null && v.Details.Engine.FuelType != null
                            ? v.Details.Engine.FuelType
                            : "ICE")
                        : "ICE"
            });

        if (!string.IsNullOrWhiteSpace(query.Brand))
        {
            var brand = query.Brand.Trim();
            q = q.Where(x => x.Brand == brand);
        }

        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            var category = query.Category.Trim();
            q = q.Where(x => x.Category == category);
        }

        if (!string.IsNullOrWhiteSpace(query.FuelType))
        {
            var fuelType = query.FuelType.Trim();
            q = q.Where(x => x.FuelType == fuelType);
        }

        if (query.MinPrice.HasValue)
            q = q.Where(x => x.Price >= query.MinPrice.Value);

        if (query.MaxPrice.HasValue)
            q = q.Where(x => x.Price <= query.MaxPrice.Value);

        var totalCount = await q.CountAsync(ct);

        q = (query.Sort ?? "").Trim() switch
        {
            "priceAsc" => q.OrderBy(x => x.Price),
            "priceDesc" => q.OrderByDescending(x => x.Price),
            "yearAsc" => q.OrderBy(x => x.Year),
            "yearDesc" => q.OrderByDescending(x => x.Year),
            "brandAsc" => q.OrderBy(x => x.Brand).ThenBy(x => x.Model),
            _ => q.OrderByDescending(x => x.Year).ThenBy(x => x.Brand).ThenBy(x => x.Model),
        };

        var skip = (page - 1) * pageSize;

        var items = await q
            .Skip(skip)
            .Take(pageSize)
            .Select(x => new VehicleListItemDto
            {
                Id = x.Id,
                Brand = x.Brand,
                Model = x.Model,
                Year = x.Year,
                Price = x.Price,
                Category = x.Category,
                Transmission = x.Transmission,
                Slug = x.Slug,
                ImageUrl = x.ImageUrl,
                VehicleType = x.VehicleType,
                FuelType = x.FuelType
            })
            .ToListAsync(ct);

        return new PagedResult<VehicleListItemDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
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
                ImageUrl = v.ImageUrl,

                VehicleType = v.Kind == VehicleKind.Bike ? "bike" : "car",

                FuelType =
                    v.Powertrain == PowertrainKind.Ev ? "EV" :
                    v.Powertrain == PowertrainKind.Hybrid ? "Hybrid" :
                    v.Powertrain == PowertrainKind.Ice
                        ? (v.Details != null && v.Details.Engine != null && v.Details.Engine.FuelType != null
                            ? v.Details.Engine.FuelType
                            : "ICE")
                        : "ICE"
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
                ImageUrl = v.ImageUrl,

                VehicleType = v.Kind == VehicleKind.Bike ? "bike" : "car",

                FuelType =
                    v.Powertrain == PowertrainKind.Ev ? "EV" :
                    v.Powertrain == PowertrainKind.Hybrid ? "Hybrid" :
                    v.Powertrain == PowertrainKind.Ice
                        ? (v.Details != null && v.Details.Engine != null && v.Details.Engine.FuelType != null
                            ? v.Details.Engine.FuelType
                            : "ICE")
                        : "ICE"
            })
            .FirstOrDefaultAsync();
    }

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

                VehicleType = v.Kind == VehicleKind.Bike ? "bike" : "car",

                FuelType =
                    v.Powertrain == PowertrainKind.Ev ? "EV" :
                    v.Powertrain == PowertrainKind.Hybrid ? "Hybrid" :
                    v.Powertrain == PowertrainKind.Ice
                        ? (v.Details != null && v.Details.Engine != null && v.Details.Engine.FuelType != null
                            ? v.Details.Engine.FuelType
                            : "ICE")
                        : "ICE",

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

                VehicleType = v.Kind == VehicleKind.Bike ? "bike" : "car",

                FuelType =
                    v.Powertrain == PowertrainKind.Ev ? "EV" :
                    v.Powertrain == PowertrainKind.Hybrid ? "Hybrid" :
                    v.Powertrain == PowertrainKind.Ice
                        ? (v.Details != null && v.Details.Engine != null && v.Details.Engine.FuelType != null
                            ? v.Details.Engine.FuelType
                            : "ICE")
                        : "ICE",

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

    public async Task<VehicleVariantDto?> GetPublicVariantBySlugAsync(
     string vehicleSlug,
     string variantSlug,
     CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(vehicleSlug) || string.IsNullOrWhiteSpace(variantSlug))
            return null;

        vehicleSlug = vehicleSlug.Trim();
        variantSlug = variantSlug.Trim();

        return await _db.Vehicles
            .AsNoTracking()
            .Where(v => v.Slug == vehicleSlug)
            .SelectMany(v => v.Variants)
            .Where(vr =>
                vr.IsActive &&
                (vr.Slug == variantSlug || vr.Slug.EndsWith("-" + variantSlug)))
            .OrderByDescending(vr => vr.IsDefault)
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
            .FirstOrDefaultAsync(ct);
    }

    public async Task UpdateVehicleDetailsAsync(int id, UpdateVehicleDetailsRequest req)
    {
        var details = await _db.VehicleDetails
            .Include(d => d.Vehicle)
            .FirstOrDefaultAsync(d => d.VehicleId == id);

        if (details is null) throw new KeyNotFoundException();

        if (!string.IsNullOrWhiteSpace(req.VehicleType))
        {
            var requestedKind = ParseKindOrThrow(req.VehicleType);
            if (details.Vehicle.Kind != requestedKind)
                throw new ArgumentException("VehicleType mismatch for this vehicle.");
        }

        if (req.Description is not null) details.Description = req.Description;
        if (req.ColorsAvailableJson is not null) details.ColorsAvailableJson = req.ColorsAvailableJson;
        if (req.WarrantyYears.HasValue) details.WarrantyYears = req.WarrantyYears;
        if (req.ServiceIntervalKm.HasValue) details.ServiceIntervalKm = req.ServiceIntervalKm;

        NormalizeByKind(details.Vehicle.Kind, details);

        var powertrain = details.Vehicle.Powertrain;

        if (req.Engine is not null)
        {
            if (powertrain == PowertrainKind.Ev)
            {
                details.Engine = null;
            }
            else
            {
                details.Engine ??= new EngineSpecs();
                details.Engine.EngineType = req.Engine.EngineType;
                details.Engine.EngineDisplacement = req.Engine.EngineDisplacement;
                details.Engine.InductionType = req.Engine.InductionType;
                details.Engine.Emission = req.Engine.Emission;
                details.Engine.Power = req.Engine.Power;
                details.Engine.PowerRpm = req.Engine.PowerRpm;
                details.Engine.Torque = req.Engine.Torque;
                details.Engine.TorqueRpm = req.Engine.TorqueRpm;
                details.Engine.Mileage = req.Engine.Mileage;
                details.Engine.Range = req.Engine.Range;
                details.Engine.FuelType = req.Engine.FuelType;
            }
        }
        else if (powertrain == PowertrainKind.Ev)
        {
            details.Engine = null;
        }

        if (req.Ev is not null)
        {
            details.Ev ??= new EvSpecs();
            details.Ev.BatteryCapacity = req.Ev.BatteryCapacity;
            details.Ev.ChargingTimeFast = req.Ev.ChargingTimeFast;
            details.Ev.ChargingTimeNormal = req.Ev.ChargingTimeNormal;
            details.Ev.MotorPower = req.Ev.MotorPower;
            details.Ev.MotorTorque = req.Ev.MotorTorque;
            details.Ev.FastChargingPort = req.Ev.FastChargingPort;
            details.Ev.Range = req.Ev.Range;
        }

        if (req.Dimensions is not null)
        {
            details.Dimensions ??= new DimensionsSpecs();
            details.Dimensions.Length = req.Dimensions.Length;
            details.Dimensions.Width = req.Dimensions.Width;
            details.Dimensions.Height = req.Dimensions.Height;
            details.Dimensions.WheelBase = req.Dimensions.WheelBase;
            details.Dimensions.GroundClearance = req.Dimensions.GroundClearance;
            details.Dimensions.Weight = req.Dimensions.Weight;
            details.Dimensions.TurningRadius = req.Dimensions.TurningRadius;
        }

        if (req.Dynamics is not null)
        {
            details.Dynamics ??= new TyresBrakesSuspensionSpecs();
            details.Dynamics.FrontType = req.Dynamics.FrontType;
            details.Dynamics.BackType = req.Dynamics.BackType;
            details.Dynamics.FrontBrake = req.Dynamics.FrontBrake;
            details.Dynamics.BackBrake = req.Dynamics.BackBrake;
            details.Dynamics.FrontSuspension = req.Dynamics.FrontSuspension;
            details.Dynamics.RearSuspension = req.Dynamics.RearSuspension;
            details.Dynamics.TyreSizeFront = req.Dynamics.TyreSizeFront;
            details.Dynamics.TyreSizeBack = req.Dynamics.TyreSizeBack;
            details.Dynamics.TyreType = req.Dynamics.TyreType;
            details.Dynamics.WheelMaterial = req.Dynamics.WheelMaterial;
        }

        if (details.Vehicle.Kind == VehicleKind.Bike)
        {
            if (req.Bike is not null)
            {
                details.Bike ??= new BikeSpecs();
                details.Bike.NumberOfGears = req.Bike.NumberOfGears;
                details.Bike.TankSize = req.Bike.TankSize;
                details.Bike.Abs = req.Bike.Abs;
                details.Bike.TractionControl = req.Bike.TractionControl;
                details.Bike.DisplaySize = req.Bike.DisplaySize;
                details.Bike.Bluetooth = req.Bike.Bluetooth;
                details.Bike.Navigation = req.Bike.Navigation;
                details.Bike.SmartConnectivity = req.Bike.SmartConnectivity;
            }

            details.Car = null;
        }

        if (details.Vehicle.Kind == VehicleKind.Car)
        {
            if (req.Car is not null)
            {
                details.Car ??= new CarSpecs();
                details.Car.DriveType = req.Car.DriveType;
                details.Car.ZeroToHundred = req.Car.ZeroToHundred;
                details.Car.TopSpeed = req.Car.TopSpeed;
                details.Car.PersonCapacity = req.Car.PersonCapacity;
                details.Car.Rows = req.Car.Rows;
                details.Car.Doors = req.Car.Doors;
                details.Car.BootSpace = req.Car.BootSpace;
                details.Car.PoweredSteering = req.Car.PoweredSteering;
                details.Car.SteeringType = req.Car.SteeringType;
                details.Car.HasSpareWheel = req.Car.HasSpareWheel;
                details.Car.Airbags = req.Car.Airbags;
                details.Car.HillAssist = req.Car.HillAssist;
                details.Car.RearViewCamera = req.Car.RearViewCamera;
                details.Car.ParkingSensors = req.Car.ParkingSensors;
                details.Car.CruiseControl = req.Car.CruiseControl;
                details.Car.DisplaySize = req.Car.DisplaySize;
                details.Car.Bluetooth = req.Car.Bluetooth;
                details.Car.Navigation = req.Car.Navigation;
                details.Car.SmartConnectivity = req.Car.SmartConnectivity;
            }

            details.Bike = null;
        }

        await _db.SaveChangesAsync();
    }

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
