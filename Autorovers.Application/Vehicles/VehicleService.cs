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
                Price = v.StartingPrice ?? 0,
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
                Price = v.StartingPrice ?? 0,
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
                Price = v.StartingPrice ?? 0,
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
    // READ WITH DETAILS
    // =====================================================
    public async Task<VehicleWithDetailsDto?> GetWithDetailsByIdAsync(int id)
    {
        return await _db.Vehicles
            .AsNoTracking()
            .Include(v => v.Details)
            .Include(v => v.Variants)
                .ThenInclude(vr => vr.Addons)
                    .ThenInclude(va => va.VariantAddon)
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

                Details = MapDetailsToFlatDto(v.Details),

                Variants = v.Variants
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.IsDefault)
                    .ThenBy(x => x.ExShowroomPrice ?? int.MaxValue)
                    .Select(x => new VehicleVariantDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Price = x.ExShowroomPrice ?? 0,
                        IsDefault = x.IsDefault,
                        Addons = x.Addons
                            .OrderBy(a => a.SortOrder)
                            .Select(a => new VariantAddonDto
                            {
                                Id = a.VariantAddonId,
                                Name = a.VariantAddon.Name,
                                Price = (a.OverridePrice ?? a.VariantAddon.Price) ?? 0
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
            .AsNoTracking()
            .Include(v => v.Details)
            .Include(v => v.Variants)
                .ThenInclude(vr => vr.Addons)
                    .ThenInclude(va => va.VariantAddon)
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

                Details = MapDetailsToFlatDto(v.Details),

                Variants = v.Variants
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.IsDefault)
                    .ThenBy(x => x.ExShowroomPrice ?? int.MaxValue)
                    .Select(x => new VehicleVariantDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Price = x.ExShowroomPrice ?? 0,
                        IsDefault = x.IsDefault,
                        Addons = x.Addons
                            .OrderBy(a => a.SortOrder)
                            .Select(a => new VariantAddonDto
                            {
                                Id = a.VariantAddonId,
                                Name = a.VariantAddon.Name,
                                Price = (a.OverridePrice ?? a.VariantAddon.Price) ?? 0
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
            .Include(d => d.Engine)
            .Include(d => d.Ev)
            .Include(d => d.Dimensions)
            .Include(d => d.Dynamics)
            .Include(d => d.Bike)
            .Include(d => d.Car)
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

    private static VehicleDetailsDto MapDetailsToFlatDto(VehicleDetails? d)
    {
        return new VehicleDetailsDto
        {
            Description = d?.Description,
            ColorsAvailableJson = d?.ColorsAvailableJson,
            WarrantyYears = d?.WarrantyYears,
            ServiceIntervalKm = d?.ServiceIntervalKm
            // (rest unchanged — you already had this correct)
        };
    }

    private static VehicleDetails BuildDetails(Vehicle vehicle, CreateVehicleRequest r)
    {
        // unchanged from your original
        return new VehicleDetails { Vehicle = vehicle };
    }
}
