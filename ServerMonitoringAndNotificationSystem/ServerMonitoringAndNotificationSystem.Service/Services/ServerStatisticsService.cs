using ServerMonitoringAndNotificationSystem.Domin.configuration;
using ServerMonitoringAndNotificationSystem.Domin.Interfaces;

namespace ServerMonitoringAndNotificationSystem.Services
{
    public class ServerStatisticsService : IServerStatisticsService
    {
        private readonly IStatisticsCollector _collector;
        private readonly IMessageProducer _producer;
        private readonly ServerStatisticsConfig _config;

        public ServerStatisticsService(
            IStatisticsCollector collector,
            IMessageProducer producer,
            ServerStatisticsConfig config)
        {
            _collector = collector;
            _producer = producer;
            _config = config;
        }

        public async Task RunAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var stats = await _collector.CollectStatisticsAsync();
                await _producer.ProduceAsync($"ServerStatistics.{_config.ServerIdentifier}", stats);

                await Task.Delay(TimeSpan.FromSeconds(_config.SamplingIntervalSeconds), token);
            }
        }
    }
}
