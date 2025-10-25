namespace ServerMonitoringAndNotificationSystem.Domin.Interfaces
{
    public interface IServerStatisticsService
    {
        Task RunAsync(CancellationToken token);
    }
}
