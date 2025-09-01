using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Repositories
{
    public class BuildingRepositoryStub : IBuildingRepository
    {
        private readonly List<Building> _buildings = new();
        public Task<IEnumerable<Building>> GetAllAsync() => Task.FromResult(_buildings.AsEnumerable());
        public Task<Building?> GetByIdAsync(int id) => Task.FromResult(_buildings.FirstOrDefault(b => b.Id == id));
        public Task AddAsync(Building entity) { _buildings.Add(entity); return Task.CompletedTask; }
        public void Update(Building entity) { }
        public void Remove(Building entity) { _buildings.Remove(entity); }
    }
}
