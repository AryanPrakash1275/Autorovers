using Autorovers.Application.Vehicles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoroversApi.Controllers;

[ApiController]
[Route("api/AdminVehicles")]
[Authorize(Policy = "AdminOnly")]
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
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var v = await _vehicleService.GetByIdAsync(id);
        if (v is null) return NotFound();
        return Ok(v);
    }

    // GET: /api/AdminVehicles/{id}/details
    [HttpGet("{id:int}/details")]
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
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var id = await _vehicleService.CreateVehicleAsync(req);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    // POST: /api/AdminVehicles/bulk
    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreate([FromBody] List<CreateVehicleRequest> reqs)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        if (reqs == null || reqs.Count == 0)
        {
            return BadRequest("Request body must be a non-empty array of vehicles.");
        }

        var ids = new List<int>(reqs.Count);

        foreach (var r in reqs)
        {
            var id = await _vehicleService.CreateVehicleAsync(r);
            ids.Add(id);
        }

        return Ok(new
        {
            count = ids.Count,
            ids
        });
    }

    // PUT: /api/AdminVehicles/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVehicleRequest req)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        await _vehicleService.UpdateVehicleAsync(id, req);
        return NoContent();
    }

    // PUT: /api/AdminVehicles/{id}/details
    [HttpPut("{id:int}/details")]
    public async Task<IActionResult> UpdateDetails(int id, [FromBody] UpdateVehicleDetailsRequest req)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        await _vehicleService.UpdateVehicleDetailsAsync(id, req);
        return NoContent();
    }

    // DELETE: /api/AdminVehicles/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _vehicleService.DeleteVehicleAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
