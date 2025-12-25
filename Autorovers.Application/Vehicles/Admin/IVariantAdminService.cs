using Autorovers.Application.Vehicles;

namespace Autorovers.Application.Vehicles.Admin;

public interface IVariantAdminService
{
    // Variants
    Task<IReadOnlyList<VehicleVariantDto>> GetVariantsForVehicleAsync(int vehicleId);
    Task<int> CreateVariantAsync(int vehicleId, CreateVehicleVariantRequest req);
    Task UpdateVariantAsync(int variantId, UpdateVehicleVariantRequest req);
    Task DeleteVariantAsync(int variantId);

    // Addons (catalog)
    Task<IReadOnlyList<VariantAddonDto>> GetAllAddonsAsync();
    Task<int> CreateAddonAsync(CreateVariantAddonRequest req);
    Task UpdateAddonAsync(int addonId, UpdateVariantAddonRequest req);
    Task DeleteAddonAsync(int addonId);

    // Link addon to variant
    Task UpsertAddonLinkAsync(int variantId, int addonId, UpsertVariantAddonLinkRequest req);
    Task RemoveAddonLinkAsync(int variantId, int addonId);
}
