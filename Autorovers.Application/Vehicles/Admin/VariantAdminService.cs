using Autorovers.Application.Abstractions.Data;
using Autorovers.Application.Abstractions.Exceptions;
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

    public async Task<IReadOnlyList<VehicleVariantDto>> GetVariantsForVehicleAsync(int vehicleId)
    {
        var vehicleExists = await _db.Vehicles.AsNoTracking().AnyAsync(v => v.Id == vehicleId);
        if (!vehicleExists) throw new NotFoundException($"Vehicle {vehicleId} not found.");

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
                Addons = v.Addons
                    .OrderBy(x => x.SortOrder)
                    .Select(x => new VariantAddonDto
                    {
                        Id = x.VariantAddonId,
                        Name = x.VariantAddon.Name,
                        Price = x.OverridePrice ?? (x.VariantAddon.Price ?? 0)
                    })
                    .ToList()
            })
            .ToListAsync();

        return variants;
    }

    public async Task<int> CreateVariantAsync(int vehicleId, CreateVehicleVariantRequest req)
    {
        var name = (req.Name ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException(
                "Variant name is required.",
                new Dictionary<string, string[]> { ["name"] = ["Variant name is required."] }
            );

        var dbCtx = (DbContext)_db;
        var strategy = dbCtx.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await dbCtx.Database.BeginTransactionAsync();

            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);
            if (vehicle is null) throw new NotFoundException($"Vehicle {vehicleId} not found.");

            var slug = string.IsNullOrWhiteSpace(req.Slug)
                ? Vehicle.GenerateSlug(vehicle.Brand, vehicle.Model, name)
                : req.Slug.Trim();

            var slugClash = await _db.VehicleVariants
                .AsNoTracking()
                .AnyAsync(v => v.VehicleId == vehicleId && v.Slug == slug);

            if (slugClash)
                throw new ValidationException(
                    "Variant slug already exists for this vehicle.",
                    new Dictionary<string, string[]> { ["slug"] = ["Variant slug already exists for this vehicle."] }
                );

            if (req.IsDefault)
            {
                var defaults = await _db.VehicleVariants
                    .Where(v => v.VehicleId == vehicleId && v.IsDefault)
                    .ToListAsync();

                foreach (var d in defaults) d.IsDefault = false;
            }

            var variant = new VehicleVariant
            {
                VehicleId = vehicleId,
                Name = name,
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

            await _db.SaveChangesAsync();
            await RecalculateStartingPriceAsync(vehicleId);

            await tx.CommitAsync();
            return variant.Id;
        });
    }

    public async Task UpdateVariantAsync(int variantId, UpdateVehicleVariantRequest req)
    {
        var dbCtx = (DbContext)_db;
        var strategy = dbCtx.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await dbCtx.Database.BeginTransactionAsync();

            var variant = await _db.VehicleVariants.FirstOrDefaultAsync(v => v.Id == variantId);
            if (variant is null) throw new NotFoundException($"Variant {variantId} not found.");

            var newName = req.Name?.Trim();

            if (req.Name is not null && string.IsNullOrWhiteSpace(newName))
                throw new ValidationException(
                    "Variant name is required.",
                    new Dictionary<string, string[]> { ["name"] = ["Variant name is required."] }
                );

            var newSlug = req.Slug?.Trim();

            if (req.Slug is not null && string.IsNullOrWhiteSpace(newSlug))
                throw new ValidationException(
                    "Slug cannot be empty.",
                    new Dictionary<string, string[]> { ["slug"] = ["Slug cannot be empty."] }
                );

            if (newSlug is not null)
            {
                var slugClash = await _db.VehicleVariants
                    .AsNoTracking()
                    .AnyAsync(v => v.VehicleId == variant.VehicleId && v.Id != variant.Id && v.Slug == newSlug);

                if (slugClash)
                    throw new ValidationException(
                        "Variant slug already exists for this vehicle.",
                        new Dictionary<string, string[]> { ["slug"] = ["Variant slug already exists for this vehicle."] }
                    );
            }

            if (req.Name is not null) variant.Name = newName!;
            if (req.Code is not null) variant.Code = req.Code.Trim();

            if (req.IsActive.HasValue) variant.IsActive = req.IsActive.Value;

            if (req.ExShowroomPrice.HasValue) variant.ExShowroomPrice = req.ExShowroomPrice.Value;
            if (req.PriceDelta.HasValue) variant.PriceDelta = req.PriceDelta.Value;

            if (req.Transmission is not null) variant.Transmission = req.Transmission.Trim();
            if (req.Powertrain.HasValue) variant.Powertrain = req.Powertrain.Value;

            if (req.ImageUrl is not null) variant.ImageUrl = req.ImageUrl.Trim();
            if (req.Slug is not null) variant.Slug = newSlug!;

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

            await tx.CommitAsync();
        });
    }

    public async Task DeleteVariantAsync(int variantId)
    {
        var dbCtx = (DbContext)_db;
        var strategy = dbCtx.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await dbCtx.Database.BeginTransactionAsync();

            var variant = await _db.VehicleVariants
                .Include(v => v.Addons)
                .FirstOrDefaultAsync(v => v.Id == variantId);

            if (variant is null) return;

            var vehicleId = variant.VehicleId;
            var wasDefault = variant.IsDefault;

            if (variant.Addons.Count > 0)
                _db.VehicleVariantAddons.RemoveRange(variant.Addons);

            _db.VehicleVariants.Remove(variant);
            await _db.SaveChangesAsync();

            if (wasDefault)
            {
                var next = await _db.VehicleVariants
                    .Where(v => v.VehicleId == vehicleId && v.IsActive)
                    .OrderBy(v => v.ExShowroomPrice ?? int.MaxValue)
                    .ThenBy(v => v.Id)
                    .FirstOrDefaultAsync();

                if (next is not null)
                {
                    next.IsDefault = true;
                    await _db.SaveChangesAsync();
                }
            }

            await RecalculateStartingPriceAsync(vehicleId);

            await tx.CommitAsync();
        });
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

        vehicle.StartingPrice = minPrice == 0 ? vehicle.StartingPrice : minPrice;
        await _db.SaveChangesAsync();
    }

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
        var name = (req.Name ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException(
                "Addon name is required.",
                new Dictionary<string, string[]> { ["name"] = ["Addon name is required."] }
            );

        var addon = new VariantAddon
        {
            Name = name,
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
        if (addon is null) throw new NotFoundException($"Addon {addonId} not found.");

        var name = req.Name?.Trim();
        if (req.Name is not null && string.IsNullOrWhiteSpace(name))
            throw new ValidationException(
                "Addon name is required.",
                new Dictionary<string, string[]> { ["name"] = ["Addon name is required."] }
            );

        if (req.Name is not null) addon.Name = name!;
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

        var linked = await _db.VehicleVariantAddons.AsNoTracking().AnyAsync(x => x.VariantAddonId == addonId);
        if (linked)
            throw new ValidationException(
                "Addon is linked to variants and cannot be deleted.",
                new Dictionary<string, string[]> { ["addonId"] = ["Addon is linked to variants and cannot be deleted."] }
            );

        _db.VariantAddons.Remove(addon);
        await _db.SaveChangesAsync();
    }

    public async Task UpsertAddonLinkAsync(int variantId, int addonId, UpsertVariantAddonLinkRequest req)
    {
        var variantExists = await _db.VehicleVariants.AsNoTracking().AnyAsync(v => v.Id == variantId);
        if (!variantExists) throw new NotFoundException($"Variant {variantId} not found.");

        var addonExists = await _db.VariantAddons.AsNoTracking().AnyAsync(a => a.Id == addonId);
        if (!addonExists) throw new NotFoundException($"Addon {addonId} not found.");

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
