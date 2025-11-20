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

    public async Task<int> CreateVehicleAsync(CreateVehicleRequest r)
    {
        var vehicle = new Vehicle
        {
            Brand = r.Brand,
            Model = r.Model,
            Variant = r.Variant,
            Year = r.Year,
            Category = r.Category,
            Price = r.Price,
            Transmission = r.Transmission,
            Slug = Vehicle.GenerateSlug(r.Brand, r.Model, r.Variant)
        };

        _db.Vehicles.Add(vehicle);

        var details = new VehicleDetails
        {
            Vehicle = vehicle,

            // Engine Specs
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

            // Size & weight
            Length = r.Length,
            Width = r.Width,
            Height = r.Height,
            Weight = r.Weight,
            GroundClearance = r.GroundClearance,
            WheelBase = r.WheelBase,

            // Capacity
            PersonCapacity = r.PersonCapacity,
            Rows = r.Rows,
            Doors = r.Doors,
            BootSpace = r.BootSpace,
            TankSize = r.TankSize,

            // Dynamics
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

}
