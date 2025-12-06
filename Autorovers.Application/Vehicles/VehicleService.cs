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
    // CREATE VEHICLE — Option 2 slug (brand-model-var-id)
    // =====================================================
    public async Task<int> CreateVehicleAsync(CreateVehicleRequest r)
    {
        // STEP 1: Create vehicle WITHOUT slug first
        var vehicle = new Vehicle
        {
            Brand = r.Brand,
            Model = r.Model,
            Variant = r.Variant,
            Year = r.Year,
            Category = r.Category,
            Price = r.Price,
            Transmission = r.Transmission
        };

        _db.Vehicles.Add(vehicle);
        await _db.SaveChangesAsync(); // Generates ID

        // STEP 2: Now generate slug with ID appended
        vehicle.Slug = SlugHelper.ToSlug(r.Brand, r.Model, r.Variant) + "-" + vehicle.Id;
        await _db.SaveChangesAsync();

        // STEP 3: Create details
        var details = new VehicleDetails
        {
            VehicleId = vehicle.Id,

            EngineType = r.EngineType,
            Specification = r.Specification,
            InductionType = r.InductionType,
            Power = r.Power,
            Torque = r.Torque,
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

            FrontTyre = r.FrontTyre,
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

    // =====================================================
    // GET ALL
    // =====================================================
    public async Task<IReadOnlyList<VehicleListItemDto>> GetAllAsync()
    {
        return await _db.Vehicles
            .OrderBy(v => v.Brand)
            .ThenBy(v => v.Model)
            .Select(v => new VehicleListItemDto
            {
                Id = v.Id,
                Brand = v.Brand,
                Model = v.Model,
                Variant = v.Variant,
                Year = v.Year,
                Price = v.Price,
                Category = v.Category,
                Transmission = v.Transmission,
                Slug = v.Slug
            })
            .ToListAsync();
    }

    // =====================================================
    // GET BY ID
    // =====================================================
    public async Task<VehicleListItemDto?> GetByIdAsync(int id)
    {
        return await _db.Vehicles
            .Where(v => v.Id == id)
            .Select(v => new VehicleListItemDto
            {
                Id = v.Id,
                Brand = v.Brand,
                Model = v.Model,
                Variant = v.Variant,
                Year = v.Year,
                Price = v.Price,
                Category = v.Category,
                Transmission = v.Transmission,
                Slug = v.Slug
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
                Brand = v.Brand,
                Model = v.Model,
                Variant = v.Variant,
                Year = v.Year,
                Price = v.Price,
                Category = v.Category,
                Transmission = v.Transmission,
                Slug = v.Slug
            })
            .FirstOrDefaultAsync();
    }


    // =====================================================
    // UPDATE VEHICLE — regenerate slug with ID
    // =====================================================
    public async Task UpdateVehicleAsync(int id, UpdateVehicleRequest request)
    {
        var entity = await _db.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id);

        if (entity is null)
            throw new KeyNotFoundException($"Vehicle with id {id} not found.");

        entity.Brand = request.Brand;
        entity.Model = request.Model;
        entity.Variant = request.Variant;
        entity.Year = request.Year;
        entity.Price = request.Price;
        entity.Category = request.Category;
        entity.Transmission = request.Transmission;

        // regenerate slug: brand-model-variant-id
        entity.Slug = SlugHelper.ToSlug(request.Brand, request.Model, request.Variant) + "-" + id;

        await _db.SaveChangesAsync();
    }

    // =====================================================
    // DELETE
    // =====================================================
    public async Task<bool> DeleteVehicleAsync(int id)
    {
        var entity = await _db.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id);

        if (entity is null)
            return false;

        _db.Vehicles.Remove(entity);
        await _db.SaveChangesAsync();

        return true;
    }
}
