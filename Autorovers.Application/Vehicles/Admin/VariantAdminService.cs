using Autorovers.Application.Abstractions.Data;
using Autorovers.Application.Vehicles;
using Autorovers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autorovers.Application.Vehicles.Admin;

public sealed class VariantAdminService : IVariantAdminService
{
    private readonly IApplicationDbContext _db;

    public VariantAdminService(IApplicationDbContext db)
    {
        _db = db;
    }

    // =========================
    // Variants
    // =========================
    public async Task<IReadOnlyList<VehicleVariantDto>> GetVariantsForVehicleAsync(int vehicleId)
    {
        var variants = await _db.VehicleVariants
            .AsNoTracking()
            .Where(v => v.VehicleId == vehicleId)
            .OrderByDescending(v => v.IsDefault)
            .ThenBy(v => v.ExShowroomPrice ?? int.MaxValue)
            .Select(v => new VehicleVariantDto
            {
                Id = v.Id,
                Name = v.Name,
                Price = v.ExShowroomPrice ?? 0,
                IsDefault = v.IsDefault,
                Addons = new List<VariantAddonDto>() // admin list endpoint doesn't need addons by default
            })
            .ToListAsync();

        return variants;
    }

    public async Task<int> CreateVariantAsync(int vehicleId, CreateVehicleVariantRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
            throw new ArgumentException("Variant name is required.");

        var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);
        if (vehicle is null) throw new KeyNotFoundException("Vehicle not found.");

        // If making this default, unset existing defaults.
        if (req.IsDefault)
        {
            var existingDefaults = await _db.VehicleVariants
                .Where(v => v.VehicleId == vehicleId && v.IsDefault)
                .ToListAsync();

            foreach (var d in existingDefaults) d.IsDefault = false;
        }

        var slug = string.IsNullOrWhiteSpace(req.Slug)
            ? Vehicle.GenerateSlug(vehicle.Brand, vehicle.Model, req.Name)
            : req.Slug.Trim();

        var variant = new VehicleVariant
        {
            VehicleId = vehicleId,
            Name = req.Name.Trim(),
            Code = req.Code?.Trim(),
            IsDefault = req.IsDefault,
            IsActive = req.IsActive,

            ExShowroomPrice = req.ExShowroomPrice,
            PriceDelta = req.PriceDelta,

            Transmission = req.Transmission?.Trim(),
            Powertrain = req.Powertrain,

            ImageUrl = req.ImageUrl?.Trim(),
            Slug = slug
        };

        _db.VehicleVariants.Add(variant);

        // Maintain Vehicle.StartingPrice as min active variant price (if present)
        await _db.SaveChangesAsync();
        await RecalculateStartingPriceAsync(vehicleId);

        return variant.Id;
    }

    public async Task UpdateVariantAsync(int variantId, UpdateVehicleVariantRequest req)
    {
        var variant = await _db.VehicleVariants.FirstOrDefaultAsync(v => v.Id == variantId);
        if (variant is null) throw new KeyNotFoundException("Variant not found.");

        if (req.Name is not null) variant.Name = req.Name.Trim();
        if (req.Code is not null) variant.Code = req.Code.Trim();

        if (req.IsActive.HasValue) variant.IsActive = req.IsActive.Value;

        if (req.ExShowroomPrice.HasValue) variant.ExShowroomPrice = req.ExShowroomPrice.Value;
        if (req.PriceDelta.HasValue) variant.PriceDelta = req.PriceDelta.Value;

        if (req.Transmission is not null) variant.Transmission = req.Transmission.Trim();
        if (req.Powertrain.HasValue) variant.Powertrain = req.Powertrain.Value;

        if (req.ImageUrl is not null) variant.ImageUrl = req.ImageUrl.Trim();
        if (req.Slug is not null) variant.Slug = req.Slug.Trim();

        // Default handling
        if (req.IsDefault.HasValue)
        {
            if (req.IsDefault.Value)
            {
                var others = await _db.VehicleVariants
                    .Where(v => v.VehicleId == variant.VehicleId && v.Id != variant.Id && v.IsDefault)
                    .ToListAsync();

                foreach (var o in others) o.IsDefault = false;
                variant.IsDefault = true;
            }
            else
            {
                variant.IsDefault = false;
            }
        }

        await _db.SaveChangesAsync();
        await RecalculateStartingPriceAsync(variant.VehicleId);
    }

    public async Task DeleteVariantAsync(int variantId)
    {
        var variant = await _db.VehicleVariants.FirstOrDefaultAsync(v => v.Id == variantId);
        if (variant is null) return;

        var vehicleId = variant.VehicleId;

        _db.VehicleVariants.Remove(variant);
        await _db.SaveChangesAsync();

        await RecalculateStartingPriceAsync(vehicleId);
    }

    private async Task RecalculateStartingPriceAsync(int vehicleId)
    {
        var minPrice = await _db.VehicleVariants
            .AsNoTracking()
            .Where(v => v.VehicleId == vehicleId && v.IsActive && v.ExShowroomPrice.HasValue)
            .Select(v => v.ExShowroomPrice!.Value)
            .OrderBy(x => x)
            .FirstOrDefaultAsync();

        var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);
        if (vehicle is null) return;

        // If no variant prices exist, leave as-is (or set null if you prefer)
        vehicle.StartingPrice = minPrice == 0 ? vehicle.StartingPrice : minPrice;
        await _db.SaveChangesAsync();
    }

    // =========================
    // Addons
    // =========================
    public async Task<IReadOnlyList<VariantAddonDto>> GetAllAddonsAsync()
    {
        return await _db.VariantAddons
            .AsNoTracking()
            .OrderBy(a => a.Name)
            .Select(a => new VariantAddonDto
            {
                Id = a.Id,
                Name = a.Name,
                Price = a.Price ?? 0
            })
            .ToListAsync();
    }

    public async Task<int> CreateAddonAsync(CreateVariantAddonRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
            throw new ArgumentException("Addon name is required.");

        var addon = new VariantAddon
        {
            Name = req.Name.Trim(),
            Description = req.Description?.Trim(),
            Category = req.Category?.Trim(),
            Price = req.Price,
            IsActive = req.IsActive
        };

        _db.VariantAddons.Add(addon);
        await _db.SaveChangesAsync();
        return addon.Id;
    }

    public async Task UpdateAddonAsync(int addonId, UpdateVariantAddonRequest req)
    {
        var addon = await _db.VariantAddons.FirstOrDefaultAsync(a => a.Id == addonId);
        if (addon is null) throw new KeyNotFoundException("Addon not found.");

        if (req.Name is not null) addon.Name = req.Name.Trim();
        if (req.Description is not null) addon.Description = req.Description.Trim();
        if (req.Category is not null) addon.Category = req.Category.Trim();
        if (req.Price.HasValue) addon.Price = req.Price.Value;
        if (req.IsActive.HasValue) addon.IsActive = req.IsActive.Value;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAddonAsync(int addonId)
    {
        var addon = await _db.VariantAddons.FirstOrDefaultAsync(a => a.Id == addonId);
        if (addon is null) return;

        _db.VariantAddons.Remove(addon);
        await _db.SaveChangesAsync();
    }

    // =========================
    // Link addon ↔ variant
    // =========================
    public async Task UpsertAddonLinkAsync(int variantId, int addonId, UpsertVariantAddonLinkRequest req)
    {
        // validate existence
        var variantExists = await _db.VehicleVariants.AnyAsync(v => v.Id == variantId);
        if (!variantExists) throw new KeyNotFoundException("Variant not found.");

        var addonExists = await _db.VariantAddons.AnyAsync(a => a.Id == addonId);
        if (!addonExists) throw new KeyNotFoundException("Addon not found.");

        var link = await _db.VehicleVariantAddons
            .FirstOrDefaultAsync(x => x.VehicleVariantId == variantId && x.VariantAddonId == addonId);

        if (link is null)
        {
            link = new VehicleVariantAddon
            {
                VehicleVariantId = variantId,
                VariantAddonId = addonId,
                IsIncluded = req.IsIncluded,
                SortOrder = req.SortOrder,
                OverridePrice = req.OverridePrice
            };
            _db.VehicleVariantAddons.Add(link);
        }
        else
        {
            link.IsIncluded = req.IsIncluded;
            link.SortOrder = req.SortOrder;
            link.OverridePrice = req.OverridePrice;
        }

        await _db.SaveChangesAsync();
    }

    public async Task RemoveAddonLinkAsync(int variantId, int addonId)
    {
        var link = await _db.VehicleVariantAddons
            .FirstOrDefaultAsync(x => x.VehicleVariantId == variantId && x.VariantAddonId == addonId);

        if (link is null) return;

        _db.VehicleVariantAddons.Remove(link);
        await _db.SaveChangesAsync();
    }
}
