using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Repositories;

namespace GeoCore.Controllers
{
    [ApiController]
    [Route("api/maintenance-events")]
    public class MaintenanceEventsController : ControllerBase
    {
        private readonly IMaintenanceEventRepository _repository;
        public MaintenanceEventsController(IMaintenanceEventRepository repository) { _repository = repository; }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceEventDto>>> GetAll() {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }
    }
}
