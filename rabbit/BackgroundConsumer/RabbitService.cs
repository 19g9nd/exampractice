using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BackgroundProducer;

public class RabbitService : IHostedService
{
    private IConnection connection;
    private IChannel channel;
    private readonly string queueName;
    private readonly ILogger<RabbitService> logger;

    public RabbitService(IConfiguration configuration, ILogger<RabbitService> logger)
    {
        queueName = configuration["RabbitMQ:QueueName"];
        this.logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = "rabbit",
            UserName = "rmuser",
            Password = "rmpassword"
        };

        connection = await factory.CreateConnectionAsync();
        channel = await connection.CreateChannelAsync();

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            var message = Encoding.UTF8.GetString(args.Body.ToArray());
            Console.WriteLine($"Received: '{message}'");
            logger.LogInformation("Received: {Message}", message);
            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: true,
            consumer: consumer);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (channel != null)
        {
            await channel.CloseAsync();
            await channel.DisposeAsync();
        }

        if (connection != null)
        {
            await connection.CloseAsync();
            await connection.DisposeAsync();
        }
    }
}
