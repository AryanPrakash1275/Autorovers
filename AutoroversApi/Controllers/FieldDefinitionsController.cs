using Autorovers.Application.FieldRegistry;
using Microsoft.AspNetCore.Mvc;

namespace AutoroversApi.Controllers;

[ApiController]
[Route("api/field-definitions")]
public sealed class FieldDefinitionsController : ControllerBase
{
    private readonly IFieldRegistryService _service;

    public FieldDefinitionsController(IFieldRegistryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FieldDefinitionDto>>> Get(
        [FromQuery] string? vehicleType,
        [FromQuery] string? powertrain)
    {
        var items = await _service.GetAsync(new FieldRegistryQuery
        {
            VehicleType = vehicleType,
            Powertrain = powertrain
        });

        return Ok(items);
    }
}
