using System.Collections.Generic;
using System.Threading.Tasks;
using GeoCore.Entities;

namespace GeoCore.Repositories
{
    public interface IRentalRepository
    {
        Task<IEnumerable<Rental>> GetAllAsync();
        Task<Rental?> GetByIdAsync(int id);
        Task AddAsync(Rental entity);
        void Update(Rental entity);
        void Remove(Rental entity);
    }
}
