using System.Text.Json;
using ServerMonitoringAndNotificationSystem.Services;
using RabbitMQService.Services;
using ServerMonitoringAndNotificationSystem.Domin.Interfaces;
using ServerMonitoringAndNotificationSystem.Domin.configuration;
using Microsoft.Extensions.DependencyInjection;

var json = await File.ReadAllTextAsync("appsettings.json");
var root = JsonSerializer.Deserialize<JsonElement>(json);
var statsConfig = JsonSerializer.Deserialize<ServerStatisticsConfig>(
root.GetProperty("ServerStatisticsConfig").ToString());
var rabbitConfig = JsonSerializer.Deserialize<RabbitMqConfig>(
    root.GetProperty("RabbitMqConfig").ToString());

var services = new ServiceCollection();

services.AddSingleton(statsConfig);
services.AddSingleton(rabbitConfig);

services.AddSingleton<IStatisticsCollector, StatisticsCollector>();
services.AddSingleton<IMessageProducer, RabbitMQProducer>();

services.AddSingleton<IServerStatisticsService, ServerStatisticsService>();

var serviceProvider = services.BuildServiceProvider();

var service = serviceProvider.GetRequiredService<IServerStatisticsService>();
var cts = new CancellationTokenSource();

Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

await service.RunAsync(cts.Token);
