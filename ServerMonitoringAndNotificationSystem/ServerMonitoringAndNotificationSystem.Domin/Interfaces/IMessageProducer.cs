namespace ServerMonitoringAndNotificationSystem.Domin.Interfaces
{
    public interface IMessageProducer
    {
        Task ProduceAsync<T>(string routingKey, T message);
    }
}