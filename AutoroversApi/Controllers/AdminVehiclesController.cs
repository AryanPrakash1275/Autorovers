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

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _vehicleService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var v = await _vehicleService.GetByIdAsync(id);
        return v is null ? NotFound() : Ok(v);
    }

    [HttpGet("{id:int}/details")]
    public async Task<IActionResult> GetWithDetails(int id)
    {
        var v = await _vehicleService.GetWithDetailsByIdAsync(id);
        return v is null ? NotFound() : Ok(v);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVehicleRequest req)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var id = await _vehicleService.CreateVehicleAsync(req);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreate([FromBody] List<CreateVehicleRequest> reqs)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (reqs == null || reqs.Count == 0)
            return BadRequest("Request body must be a non-empty array of vehicles.");

        var ids = await _vehicleService.BulkCreateAsync(reqs);
        return Ok(new { count = ids.Count, ids });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVehicleRequest req)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        await _vehicleService.UpdateVehicleAsync(id, req);
        return NoContent();
    }

    [HttpPut("{id:int}/details")]
    public async Task<IActionResult> UpdateDetails(int id, [FromBody] UpdateVehicleDetailsRequest req)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        await _vehicleService.UpdateVehicleDetailsAsync(id, req);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _vehicleService.DeleteVehicleAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
