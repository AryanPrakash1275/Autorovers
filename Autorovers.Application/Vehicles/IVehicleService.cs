using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autorovers.Application.Vehicles;

public interface IVehicleService
{
    Task<int> CreateVehicleAsync(CreateVehicleRequest request);
    Task<IReadOnlyList<VehicleListItemDto>> GetAllAsync();
    Task<VehicleListItemDto?> GetByIdAsync(int id);
    Task UpdateVehicleAsync(int id, UpdateVehicleRequest request);
    Task<bool> DeleteVehicleAsync(int id);
    Task<VehicleListItemDto?> GetBySlugAsync(string slug); //new-slug-gen


}
