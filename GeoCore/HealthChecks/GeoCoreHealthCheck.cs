// Carpeta para health checks (placeholder)
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace GeoCore.HealthChecks
{
    public class GeoCoreHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // L�gica de verificaci�n
            return Task.FromResult(HealthCheckResult.Healthy("GeoCore is healthy."));
        }
    }
}
