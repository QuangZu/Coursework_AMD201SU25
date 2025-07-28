using RabbitMQ.Client;
using System.Text;

public class RabbitMQPublisher
{
    private readonly IConfiguration _config;

    public RabbitMQPublisher(IConfiguration config)
    {
        _config = config;
    }

    public void Publish(string message, string queue = "url-events")
    {
        var factory = new ConnectionFactory
        {
            HostName = _config["RabbitMQ:Host"],
            Port = int.Parse(_config["RabbitMQ:Port"] ?? "5672"),
            UserName = _config["RabbitMQ:guest"],
            Password = _config["RabbitMQ:guest"]
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // Declare queue (only once per app lifetime ideally)
        channel.QueueDeclare(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var body = Encoding.UTF8.GetBytes(message);

        // Publish message
        channel.BasicPublish(
            exchange: "",
            routingKey: queue,
            basicProperties: null,
            body: body
        );
    }
}
