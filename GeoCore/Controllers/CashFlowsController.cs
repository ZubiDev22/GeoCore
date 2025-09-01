using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Repositories;

namespace GeoCore.Controllers
{
    [ApiController]
    [Route("api/cashflows")]
    public class CashFlowsController : ControllerBase
    {
        private readonly ICashFlowRepository _repository;
        public CashFlowsController(ICashFlowRepository repository) { _repository = repository; }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CashFlowDto>>> GetAll() {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }
    }
}
