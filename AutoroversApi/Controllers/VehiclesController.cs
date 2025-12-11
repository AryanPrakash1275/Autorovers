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
    // GET: /api/Vehicles/{id}  → FULL DETAILS
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var vehicle = await _vehicleService.GetWithDetailsByIdAsync(id);
        if (vehicle == null) return NotFound();
        return Ok(vehicle);
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var vehicle = await _vehicleService.GetWithDetailsBySlugAsync(slug);
        if (vehicle is null) return NotFound();
        return Ok(vehicle);
    }
}