using Autorovers.Application.Vehicles.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoroversApi.Controllers;

[ApiController]
[Route("api/Admin")]
[Authorize(Policy = "AdminOnly")]
public class AdminVariantsController : ControllerBase
{
    private readonly IVariantAdminService _svc;

    public AdminVariantsController(IVariantAdminService svc)
    {
        _svc = svc;
    }

    [HttpGet("Vehicles/{vehicleId:int}/variants")]
    public async Task<IActionResult> GetVehicleVariants(int vehicleId)
    {
        var items = await _svc.GetVariantsForVehicleAsync(vehicleId);
        return Ok(items);
    }

    [HttpPost("Vehicles/{vehicleId:int}/variants")]
    public async Task<IActionResult> CreateVariant(int vehicleId, [FromBody] CreateVehicleVariantRequest req)
    {
        var id = await _svc.CreateVariantAsync(vehicleId, req);
        return Ok(new { id });
    }

    [HttpPut("Variants/{variantId:int}")]
    public async Task<IActionResult> UpdateVariant(int variantId, [FromBody] UpdateVehicleVariantRequest req)
    {
        await _svc.UpdateVariantAsync(variantId, req);
        return NoContent();
    }

    [HttpDelete("Variants/{variantId:int}")]
    public async Task<IActionResult> DeleteVariant(int variantId)
    {
        await _svc.DeleteVariantAsync(variantId);
        return NoContent();
    }

    [HttpGet("Addons")]
    public async Task<IActionResult> GetAllAddons()
    {
        var items = await _svc.GetAllAddonsAsync();
        return Ok(items);
    }

    [HttpPost("Addons")]
    public async Task<IActionResult> CreateAddon([FromBody] CreateVariantAddonRequest req)
    {
        var id = await _svc.CreateAddonAsync(req);
        return Ok(new { id });
    }

    [HttpPut("Addons/{addonId:int}")]
    public async Task<IActionResult> UpdateAddon(int addonId, [FromBody] UpdateVariantAddonRequest req)
    {
        await _svc.UpdateAddonAsync(addonId, req);
        return NoContent();
    }

    [HttpDelete("Addons/{addonId:int}")]
    public async Task<IActionResult> DeleteAddon(int addonId)
    {
        await _svc.DeleteAddonAsync(addonId);
        return NoContent();
    }

    [HttpPost("Variants/{variantId:int}/addons/{addonId:int}")]
    public async Task<IActionResult> UpsertVariantAddonLink(
        int variantId,
        int addonId,
        [FromBody] UpsertVariantAddonLinkRequest req)
    {
        await _svc.UpsertAddonLinkAsync(variantId, addonId, req);
        return NoContent();
    }

    [HttpDelete("Variants/{variantId:int}/addons/{addonId:int}")]
    public async Task<IActionResult> RemoveVariantAddonLink(int variantId, int addonId)
    {
        await _svc.RemoveAddonLinkAsync(variantId, addonId);
        return NoContent();
    }
}
