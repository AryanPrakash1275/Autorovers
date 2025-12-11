// Autorovers.Application/Vehicles/IVehicleService.cs
namespace Autorovers.Application.Vehicles;

public interface IVehicleService
{
    Task<int> CreateVehicleAsync(CreateVehicleRequest request);

    Task<IReadOnlyList<VehicleListItemDto>> GetAllAsync();

    // Summary by id (used if you still want it for admin list/details)
    Task<VehicleListItemDto?> GetByIdAsync(int id);

    Task UpdateVehicleAsync(int id, UpdateVehicleRequest request);

    Task<bool> DeleteVehicleAsync(int id);

    // Summary by slug (if needed)
    Task<VehicleListItemDto?> GetBySlugAsync(string slug);

    // FULL details by id – used for details page
    Task<VehicleWithDetailsDto?> GetWithDetailsByIdAsync(int id);

    // FULL details by slug – used for slug-based details
    Task<VehicleWithDetailsDto?> GetWithDetailsBySlugAsync(string slug);

    Task UpdateVehicleDetailsAsync(int id, UpdateVehicleDetailsRequest req);
}
