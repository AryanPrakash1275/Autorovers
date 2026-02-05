using Autorovers.Application.Vehicles;
using Autorovers.Common;
using Microsoft.AspNetCore.Mvc;

namespace AutoroversApi.Controllers;

[ApiController]
[Route("api/Vehicles")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<VehicleListItemDto>>> GetAll(
        [FromQuery] PublicVehiclesQueryParams query,
        CancellationToken ct)
    {
        var result = await _vehicleService.GetPublicVehiclesAsync(query, ct);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var vehicle = await _vehicleService.GetWithDetailsByIdAsync(id);
        return vehicle is null ? NotFound() : Ok(vehicle);
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var vehicle = await _vehicleService.GetWithDetailsBySlugAsync(slug);
        return vehicle is null ? NotFound() : Ok(vehicle);
    }
}
