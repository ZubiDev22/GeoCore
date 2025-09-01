using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Repositories;

namespace GeoCore.Controllers
{
    [ApiController]
    [Route("api/asset-assessments")]
    public class AssetAssessmentsController : ControllerBase
    {
        private readonly IAssetAssessmentRepository _repository;
        public AssetAssessmentsController(IAssetAssessmentRepository repository) { _repository = repository; }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetAssessmentDto>>> GetAll() { /* Simulado */ return Ok(new List<AssetAssessmentDto>()); }
        [HttpPost]
        public async Task<ActionResult<AssetAssessmentDto>> Create([FromBody] AssetAssessmentDto dto) { /* Simulado */ return Ok(dto); }
    }
}
