// Carpeta para servicios en background (placeholder)
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace GeoCore.BackgroundServices
{
    public class BackgroundNotifier : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Lógica de inicio
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Lógica de parada
            return Task.CompletedTask;
        }
    }
}
