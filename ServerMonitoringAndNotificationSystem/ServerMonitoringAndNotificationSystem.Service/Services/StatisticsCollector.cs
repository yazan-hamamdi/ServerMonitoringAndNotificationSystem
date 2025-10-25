using ServerMonitoringAndNotificationSystem.Domin;
using ServerMonitoringAndNotificationSystem.Domin.Interfaces;
using ServerMonitoringAndNotificationSystem.Domin.Models;
using System.Diagnostics;

namespace ServerMonitoringAndNotificationSystem.Services
{
    public class StatisticsCollector : IStatisticsCollector , IDisposable
    {
        private readonly PerformanceCounter _cpuCounter;
        private readonly PerformanceCounter _memoryCounter;
        private readonly PerformanceCounter _availableMemoryCounter;

        public StatisticsCollector()
        {
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _memoryCounter = new PerformanceCounter("Memory", "Committed Bytes");
            _availableMemoryCounter = new PerformanceCounter("Memory", "Available MBytes");
            _cpuCounter.NextValue();
        }

        public async Task<ServerStatistics> CollectStatisticsAsync()
        {
            await Task.Delay(SystemConstants.DataCollectionDelayMs);

            var cpuUsage = _cpuCounter.NextValue();
            var memoryUsedBytes = _memoryCounter.NextValue();
            var availableMemoryMB = _availableMemoryCounter.NextValue();

            var memoryUsedMB = memoryUsedBytes / SystemConstants.BytesPerMegabyte;

            var statistics = new ServerStatistics
            {
                CpuUsage = Math.Round(cpuUsage, SystemConstants.RoundNumber),
                MemoryUsage = Math.Round(memoryUsedMB, SystemConstants.RoundNumber),
                AvailableMemory = Math.Round(availableMemoryMB, SystemConstants.RoundNumber),
                Timestamp = DateTime.UtcNow
            };

            return statistics;
        }

        public void Dispose()
        {
            _cpuCounter?.Dispose();
            _memoryCounter?.Dispose();
            _availableMemoryCounter?.Dispose();
        }
    }
}