using Microsoft.Extensions.Logging;
using System;

namespace GeoCore.Logging
{
    public class Loguer : ILoguer
    {
        private readonly ILogger<Loguer> _logger;
        public Loguer(ILogger<Loguer> logger)
        {
            _logger = logger;
        }
        public void LogInfo(string message)
        {
            _logger.LogInformation($"[INFO] {DateTime.Now}: {message}");
        }
        public void LogWarning(string message)
        {
            _logger.LogWarning($"[WARN] {DateTime.Now}: {message}");
        }
        public void LogError(string message, Exception? ex = null)
        {
            if (ex != null)
                _logger.LogError(ex, $"[ERROR] {DateTime.Now}: {message}");
            else
                _logger.LogError($"[ERROR] {DateTime.Now}: {message}");
        }
    }
}
