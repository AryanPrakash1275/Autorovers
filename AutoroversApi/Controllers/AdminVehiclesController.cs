using Autorovers.Application.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace AutoroversApi.Controllers;

[ApiController]
[Route("api/AdminVehicles")]
public class AdminVehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public AdminVehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    // GET: /api/AdminVehicles
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _vehicleService.GetAllAsync();
        return Ok(list);
    }

    // GET: /api/AdminVehicles/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var v = await _vehicleService.GetByIdAsync(id);
        if (v is null) return NotFound();
        return Ok(v);
    }

    // GET: /api/AdminVehicles/{id}/details
    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetWithDetails(int id)
    {
        var v = await _vehicleService.GetWithDetailsByIdAsync(id);
        if (v is null) return NotFound();
        return Ok(v);
    }

    // POST: /api/AdminVehicles
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVehicleRequest req)
    {
        var id = await _vehicleService.CreateVehicleAsync(req);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    // PUT: /api/AdminVehicles/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVehicleRequest req)
    {
        await _vehicleService.UpdateVehicleAsync(id, req);
        return NoContent();
    }

    // PUT: /api/AdminVehicles/{id}/details
    [HttpPut("{id}/details")]
    public async Task<IActionResult> UpdateDetails(int id, [FromBody] UpdateVehicleDetailsRequest req)
    {
        await _vehicleService.UpdateVehicleDetailsAsync(id, req);
        return NoContent();
    }

    // DELETE: /api/AdminVehicles/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _vehicleService.DeleteVehicleAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
