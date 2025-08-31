using System.Collections.Generic;
using System.Threading.Tasks;
using GeoCore.Entities;

namespace GeoCore.Repositories
{
    public interface ICashFlowRepository : IGenericRepository<CashFlow> { }
    public interface IMaintenanceEventRepository : IGenericRepository<MaintenanceEvent> { }
    public interface IAssetAssessmentRepository : IGenericRepository<AssetAssessment> { }
}
