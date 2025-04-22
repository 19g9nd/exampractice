using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace Consumer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsumerController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetMessage(string queueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbit",
                UserName = "rmuser",
                Password = "rmpassword"
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var result = await channel.BasicGetAsync(queueName, autoAck: true);

            if (result == null)
            {
                return NotFound("No messages in the queue.");
            }

            var message = Encoding.UTF8.GetString(result.Body.ToArray());

            return Ok($"Received: {message}");
        }

    }
}
