using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Repositories
{
    public class AssetAssessmentRepositoryStub : IAssetAssessmentRepository
    {
        private readonly List<AssetAssessment> _assessments = new();
        public Task<IEnumerable<AssetAssessment>> GetAllAsync() => Task.FromResult(_assessments.AsEnumerable());
        public Task<AssetAssessment?> GetByIdAsync(int id) => Task.FromResult(_assessments.FirstOrDefault(a => a.Id == id));
        public Task AddAsync(AssetAssessment entity) { _assessments.Add(entity); return Task.CompletedTask; }
        public void Update(AssetAssessment entity) { }
        public void Remove(AssetAssessment entity) { _assessments.Remove(entity); }
    }
}
