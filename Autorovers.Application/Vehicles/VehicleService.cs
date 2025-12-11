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

    public async Task<int> CreateVehicleAsync(CreateVehicleRequest r)
    {
        var slug = string.IsNullOrWhiteSpace(r.Slug)
            ? Vehicle.GenerateSlug(r.Brand, r.Model, r.Variant ?? string.Empty)
            : r.Slug.Trim();

        var vehicle = new Vehicle
        {
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
            EngineType = r.EngineType,
            Specification = r.Specification,
            InductionType = r.InductionType,
            Power = r.Power,
            PowerRpm = r.PowerRpm,
            Torque = r.Torque,
            TorqueRpm = r.TorqueRpm,
            Emission = r.Emission,
            Mileage = r.Mileage,
            AutoStartStop = r.AutoStartStop,
            Range = r.Range,
            Transmission = r.Transmission,
            Length = r.Length,
            Width = r.Width,
            Height = r.Height,
            Weight = r.Weight,
            GroundClearance = r.GroundClearance,
            WheelBase = r.WheelBase,
            PersonCapacity = r.PersonCapacity,
            Rows = r.Rows,
            Doors = r.Doors,
            BootSpace = r.BootSpace,
            TankSize = r.TankSize,
            FrontType = r.FrontType,
            BackType = r.BackType,
            FrontBrake = r.FrontBrake,
            BackBrake = r.BackBrake,
            PoweredSteering = r.PoweredSteering,
            TyreSizeFront = r.TyreSizeFront,
            TyreSizeBack = r.TyreSizeBack,
            TyreType = r.TyreType,
            WheelMaterial = r.WheelMaterial,
            Spare = r.Spare
        };

        _db.VehicleDetails.Add(details);
        await _db.SaveChangesAsync();

        return vehicle.Id;
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
                Details = new VehicleDetailsDto
                {
                    EngineType = v.Details!.EngineType,
                    Specification = v.Details!.Specification,
                    InductionType = v.Details!.InductionType,
                    Power = v.Details!.Power,
                    PowerRpm = v.Details!.PowerRpm,
                    Torque = v.Details!.Torque,
                    TorqueRpm = v.Details!.TorqueRpm,
                    Emission = v.Details!.Emission,
                    Mileage = v.Details!.Mileage,
                    AutoStartStop = v.Details!.AutoStartStop,
                    Range = v.Details!.Range,
                    Length = v.Details!.Length,
                    Width = v.Details!.Width,
                    Height = v.Details!.Height,
                    Weight = v.Details!.Weight,
                    GroundClearance = v.Details!.GroundClearance,
                    WheelBase = v.Details!.WheelBase,
                    PersonCapacity = v.Details!.PersonCapacity,
                    Rows = v.Details!.Rows,
                    Doors = v.Details!.Doors,
                    BootSpace = v.Details!.BootSpace,
                    TankSize = v.Details!.TankSize,
                    FrontType = v.Details!.FrontType,
                    BackType = v.Details!.BackType,
                    FrontBrake = v.Details!.FrontBrake,
                    BackBrake = v.Details!.BackBrake,
                    PoweredSteering = v.Details!.PoweredSteering,
                    TyreSizeFront = v.Details!.TyreSizeFront,
                    TyreSizeBack = v.Details!.TyreSizeBack,
                    TyreType = v.Details!.TyreType,
                    WheelMaterial = v.Details!.WheelMaterial,
                    Spare = v.Details!.Spare
                }
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
                Details = new VehicleDetailsDto
                {
                    EngineType = v.Details!.EngineType,
                    Specification = v.Details!.Specification,
                    InductionType = v.Details!.InductionType,
                    Power = v.Details!.Power,
                    PowerRpm = v.Details!.PowerRpm,
                    Torque = v.Details!.Torque,
                    TorqueRpm = v.Details!.TorqueRpm,
                    Emission = v.Details!.Emission,
                    Mileage = v.Details!.Mileage,
                    AutoStartStop = v.Details!.AutoStartStop,
                    Range = v.Details!.Range,
                    Length = v.Details!.Length,
                    Width = v.Details!.Width,
                    Height = v.Details!.Height,
                    Weight = v.Details!.Weight,
                    GroundClearance = v.Details!.GroundClearance,
                    WheelBase = v.Details!.WheelBase,
                    PersonCapacity = v.Details!.PersonCapacity,
                    Rows = v.Details!.Rows,
                    Doors = v.Details!.Doors,
                    BootSpace = v.Details!.BootSpace,
                    TankSize = v.Details!.TankSize,
                    FrontType = v.Details!.FrontType,
                    BackType = v.Details!.BackType,
                    FrontBrake = v.Details!.FrontBrake,
                    BackBrake = v.Details!.BackBrake,
                    PoweredSteering = v.Details!.PoweredSteering,
                    TyreSizeFront = v.Details!.TyreSizeFront,
                    TyreSizeBack = v.Details!.TyreSizeBack,
                    TyreType = v.Details!.TyreType,
                    WheelMaterial = v.Details!.WheelMaterial,
                    Spare = v.Details!.Spare
                }
            })
            .FirstOrDefaultAsync();
    }

    public async Task UpdateVehicleDetailsAsync(int id, UpdateVehicleDetailsRequest req)
    {
        var details = await _db.VehicleDetails.FirstOrDefaultAsync(d => d.VehicleId == id);
        if (details is null) throw new KeyNotFoundException();

        details.EngineType = req.EngineType ?? details.EngineType;
        details.Specification = req.Specification ?? details.Specification;
        details.InductionType = req.InductionType ?? details.InductionType;
        details.Power = req.Power ?? details.Power;
        details.PowerRpm = req.PowerRpm ?? details.PowerRpm;
        details.Torque = req.Torque ?? details.Torque;
        details.TorqueRpm = req.TorqueRpm ?? details.TorqueRpm;
        details.Emission = req.Emission ?? details.Emission;
        details.Mileage = req.Mileage ?? details.Mileage;
        details.AutoStartStop = req.AutoStartStop ?? details.AutoStartStop;
        details.Range = req.Range ?? details.Range;
        details.Length = req.Length ?? details.Length;
        details.Width = req.Width ?? details.Width;
        details.Height = req.Height ?? details.Height;
        details.Weight = req.Weight ?? details.Weight;
        details.GroundClearance = req.GroundClearance ?? details.GroundClearance;
        details.WheelBase = req.WheelBase ?? details.WheelBase;
        details.PersonCapacity = req.PersonCapacity ?? details.PersonCapacity;
        details.Rows = req.Rows ?? details.Rows;
        details.Doors = req.Doors ?? details.Doors;
        details.BootSpace = req.BootSpace ?? details.BootSpace;
        details.TankSize = req.TankSize ?? details.TankSize;
        details.FrontType = req.FrontType ?? details.FrontType;
        details.BackType = req.BackType ?? details.BackType;
        details.FrontBrake = req.FrontBrake ?? details.FrontBrake;
        details.BackBrake = req.BackBrake ?? details.BackBrake;
        details.PoweredSteering = req.PoweredSteering ?? details.PoweredSteering;
        details.TyreSizeFront = req.TyreSizeFront ?? details.TyreSizeFront;
        details.TyreSizeBack = req.TyreSizeBack ?? details.TyreSizeBack;
        details.TyreType = req.TyreType ?? details.TyreType;
        details.WheelMaterial = req.WheelMaterial ?? details.WheelMaterial;
        details.Spare = req.Spare ?? details.Spare;

        await _db.SaveChangesAsync();
    }
}
