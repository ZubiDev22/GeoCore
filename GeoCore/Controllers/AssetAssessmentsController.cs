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
        public async Task<ActionResult<IEnumerable<AssetAssessmentDto>>> GetAll() {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<AssetAssessmentDto>> Create([FromBody] AssetAssessmentDto dto) {
            // Ejemplo de llamada asíncrona (simulada)
            await _repository.AddAsync(new GeoCore.Entities.AssetAssessment {
                BuildingId = dto.BuildingId,
                Date = dto.Date,
                Profitability = dto.Profitability,
                RiskLevel = dto.RiskLevel,
                Recommendation = dto.Recommendation
            });
            return Ok(dto);
        }
    }
}
