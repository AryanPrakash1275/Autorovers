using Autorovers.Application.Vehicles;
using Microsoft.AspNetCore.Mvc;
// using whatever namespace your service/DTOs live in

[ApiController]
[Route("api/Vehicles")] // → /api/Vehicles
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    // GET: /api/Vehicles
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vehicles = await _vehicleService.GetAllAsync();
        return Ok(vehicles);
    }

    // GET: /api/Vehicles/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);
        if (vehicle == null) return NotFound();
        return Ok(vehicle);
    }
}
