using Autorovers.Application.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace AutoroversApi.Controllers
{
    [ApiController]
    [Route("api/admin/vehicles")]
    public class AdminVehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public AdminVehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _vehicleService.CreateVehicleAsync(request);

            return StatusCode(StatusCodes.Status201Created, new { id });
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<VehicleListItemDto>>> GetAll()
        {
            var items = await _vehicleService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VehicleListItemDto>> GetById(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle is null)
                return NotFound();

            return Ok(vehicle);
        }
    }
}
