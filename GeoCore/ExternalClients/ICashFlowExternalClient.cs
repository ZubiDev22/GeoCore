using System.Collections.Generic;
using System.Threading.Tasks;
using GeoCore.DTOs;

namespace GeoCore.ExternalClients
{
    public interface ICashFlowExternalClient
    {
        Task<IEnumerable<CashFlowDto>> GetCashFlowsAsync();
        Task<IEnumerable<CashFlowDto>> GetCashFlowsByBuildingCodeAsync(string buildingCode);
    }
}
