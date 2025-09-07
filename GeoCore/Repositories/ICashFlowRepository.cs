using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Repositories
{
    public interface ICashFlowRepository
    {
        Task<IEnumerable<CashFlow>> GetAllAsync();
        Task<CashFlow?> GetByIdAsync(string id);
        Task AddAsync(CashFlow entity);
        void Update(CashFlow entity);
        void Remove(CashFlow entity);
    }
}
