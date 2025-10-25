using ServerMonitoringAndNotificationSystem.Models;
using System.Diagnostics;

namespace ServerMonitoringAndNotificationSystem.Services.Services
{
    public class StatisticsCollector 
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

            await Task.Delay(100);
          
            var cpuUsage = _cpuCounter.NextValue();
            var memoryUsedBytes = _memoryCounter.NextValue();
            var availableMemoryMB = _availableMemoryCounter.NextValue();
          
            var memoryUsedMB = memoryUsedBytes / (1024 * 1024);
          
            var statistics = new ServerStatistics
            {
                CpuUsage = Math.Round(cpuUsage, 2),
                MemoryUsage = Math.Round(memoryUsedMB, 2),
                AvailableMemory = Math.Round(availableMemoryMB, 2),
                Timestamp = DateTime.UtcNow
            };
          
            return statistics;
        }
    }
}