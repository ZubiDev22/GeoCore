using GeoCore.Entities;
using System.Threading.Tasks;

namespace GeoCore.Repositories
{
    public interface ICashFlowRepository : IGenericRepository<CashFlow>
    {
        Task<CashFlow?> GetByIdAsync(string id);
    }
}
