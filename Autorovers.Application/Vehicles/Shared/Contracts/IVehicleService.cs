namespace Autorovers.Application.Vehicles;

using Autorovers.Common;

public interface IVehicleService
{
    Task<int> CreateVehicleAsync(CreateVehicleRequest request);

    Task<IReadOnlyList<int>> BulkCreateAsync(List<CreateVehicleRequest> requests);

    Task<IReadOnlyList<VehicleListItemDto>> GetAllAsync();

    Task<PagedResult<VehicleListItemDto>> GetPublicVehiclesAsync(
        PublicVehiclesQueryParams query,
        CancellationToken ct = default);

    Task<VehicleListItemDto?> GetByIdAsync(int id);

    Task<VehicleListItemDto?> GetBySlugAsync(string slug);

    Task UpdateVehicleAsync(int id, UpdateVehicleRequest request);

    Task<bool> DeleteVehicleAsync(int id);

    Task<VehicleWithDetailsDto?> GetWithDetailsByIdAsync(int id);

    Task<VehicleWithDetailsDto?> GetWithDetailsBySlugAsync(string slug);

    Task<VehicleVariantDto?> GetPublicVariantBySlugAsync(
        string vehicleSlug,
        string variantSlug,
        CancellationToken ct = default);

    Task UpdateVehicleDetailsAsync(int id, UpdateVehicleDetailsRequest req);
}
