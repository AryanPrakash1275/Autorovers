// Autorovers.Application/Vehicles/IVehicleService.cs
namespace Autorovers.Application.Vehicles;

public interface IVehicleService
{
    Task<int> CreateVehicleAsync(CreateVehicleRequest request);

    // REAL bulk (one SaveChanges)
    Task<IReadOnlyList<int>> BulkCreateAsync(List<CreateVehicleRequest> requests);

    Task<IReadOnlyList<VehicleListItemDto>> GetAllAsync();

    Task<VehicleListItemDto?> GetByIdAsync(int id);

    Task UpdateVehicleAsync(int id, UpdateVehicleRequest request);

    Task<bool> DeleteVehicleAsync(int id);

    Task<VehicleListItemDto?> GetBySlugAsync(string slug);

    Task<VehicleWithDetailsDto?> GetWithDetailsByIdAsync(int id);

    Task<VehicleWithDetailsDto?> GetWithDetailsBySlugAsync(string slug);

    Task UpdateVehicleDetailsAsync(int id, UpdateVehicleDetailsRequest req);
}
