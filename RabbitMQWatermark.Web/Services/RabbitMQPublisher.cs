using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQWatermark.Web.Services
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClientService _rabbitMqClientService;

        public RabbitMQPublisher(RabbitMQClientService rabbitMqClientService)
        {
            _rabbitMqClientService = rabbitMqClientService;
        }

        public void Publish(ProductImageCreatedEvent createdEvent)
        {
            var channel = _rabbitMqClientService.Connect();
            var bodyString = JsonSerializer.Serialize(createdEvent);
            var bodyByte = Encoding.UTF8.GetBytes(bodyString);
            var property = channel.CreateBasicProperties();
            property.Persistent = true;
            channel.BasicPublish
                (
                exchange: RabbitMQClientService.ExchangeName,
                routingKey: RabbitMQClientService.RoutingWatermark,
                basicProperties: property,
                body: bodyByte
                );
        }
    }
}
