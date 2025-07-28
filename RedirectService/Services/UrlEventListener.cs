using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

public class UrlEventListener : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly IDatabase _redis;
    private IConnection? _connection;
    private IModel? _channel;

    public UrlEventListener(IConfiguration config)
    {
        _config = config;
        var redis = ConnectionMultiplexer.Connect(_config["Redis:Connection"]);
        _redis = redis.GetDatabase();
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _config["RabbitMQ:Host"],
            UserName = _config["RabbitMQ:Username"],
            Password = _config["RabbitMQ:Password"]
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare("url-events", durable: true, exclusive: false, autoDelete: false, arguments: null);

        return base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            // Expected message: { "short_url": "abc123", "long_url": "https://example.com" }
            var url = JsonSerializer.Deserialize<UrlEvent>(message);

            if (url != null)
            {
                await _redis.StringSetAsync(url.short_url, url.long_url);
            }
        };

        _channel.BasicConsume(queue: "url-events", autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }

    private class UrlEvent
    {
        public string short_url { get; set; } = string.Empty;
        public string long_url { get; set; } = string.Empty;
    }
}
