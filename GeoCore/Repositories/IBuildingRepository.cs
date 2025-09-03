using System.Collections.Generic;
using System.Threading.Tasks;
using GeoCore.Entities;

namespace GeoCore.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }

    public interface IBuildingRepository : IGenericRepository<Building>
    {
        Task<Building?> GetByCodeAsync(string code); // Agregado para soporte en el controlador
    }
}
