using Autorovers.Application.Abstractions.Exceptions;
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
        if (v is null) throw new NotFoundException($"Vehicle {id} not found.");
        return Ok(v);
    }

    [HttpGet("{id:int}/details")]
    public async Task<IActionResult> GetWithDetails(int id)
    {
        var v = await _vehicleService.GetWithDetailsByIdAsync(id);
        if (v is null) throw new NotFoundException($"Vehicle {id} not found.");
        return Ok(v);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVehicleRequest req)
    {
        ThrowIfModelInvalid();

        var id = await _vehicleService.CreateVehicleAsync(req);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreate([FromBody] List<CreateVehicleRequest> reqs)
    {
        ThrowIfModelInvalid();

        if (reqs is null || reqs.Count == 0)
        {
            throw new ValidationException(
                "Request body must be a non-empty array of vehicles.",
                new Dictionary<string, string[]>
                {
                    ["vehicles"] = ["Request body must be a non-empty array of vehicles."]
                }
            );
        }

        var ids = await _vehicleService.BulkCreateAsync(reqs);
        return Ok(new { count = ids.Count, ids });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVehicleRequest req)
    {
        ThrowIfModelInvalid();

        await _vehicleService.UpdateVehicleAsync(id, req);
        return NoContent();
    }

    [HttpPut("{id:int}/details")]
    public async Task<IActionResult> UpdateDetails(int id, [FromBody] UpdateVehicleDetailsRequest req)
    {
        ThrowIfModelInvalid();

        await _vehicleService.UpdateVehicleDetailsAsync(id, req);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _vehicleService.DeleteVehicleAsync(id);
        if (!ok) throw new NotFoundException($"Vehicle {id} not found.");
        return NoContent();
    }

    private void ThrowIfModelInvalid()
    {
        if (ModelState.IsValid) return;

        var errors = ModelState
            .Where(kvp => kvp.Value is not null && kvp.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors
                    .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Invalid value." : e.ErrorMessage)
                    .ToArray()
            );

        throw new ValidationException("Validation failed.", errors);
    }
}
