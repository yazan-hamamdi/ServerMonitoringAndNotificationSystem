using ServerMonitoringAndNotificationSystem.Domin.Models;

namespace ServerMonitoringAndNotificationSystem.Domin.Interfaces
{
    public interface IStatisticsCollector
    {
        Task<ServerStatistics> CollectStatisticsAsync();
    }
}