using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoCore.Repositories
{
    public interface IApartmentRepository
    {
        Task<IEnumerable<Apartment>> GetAllAsync();
        Task<Apartment?> GetByIdAsync(string id);
        Task<IEnumerable<Apartment>> GetByBuildingIdAsync(string buildingId);
        Task AddAsync(Apartment entity);
        void Update(Apartment entity);
        void Remove(Apartment entity);
    }
}
