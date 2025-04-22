using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProducerController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Send(string message, string queueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbit",
                UserName = "rmuser",
                Password = "rmpassword"
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false
                                  );

            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync(exchange: "",
                                 routingKey: queueName,
                                 body: body);

            return Ok($"Message '{message}' sent.");
        }
    }
}
