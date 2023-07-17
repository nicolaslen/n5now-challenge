/*using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using N5.Core.Domain.Events;
using N5.Core.Messaging;

namespace N5.Infrastructure.Messaging
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly ProducerConfig _config;

        public KafkaProducer(IConfiguration configuration)
        {
            _config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:Producer:BootstrapServers"],
                ClientId = "KafkaProducer"
            };
        }

        public async Task PublishEvent(string topic, PermissionRequestedEvent permissionRequestedEvent)
        {
            using (var producer = new ProducerBuilder<Null, PermissionRequestedEvent>(_config).Build())
            {
                try
                {
                    var deliveryResult = await producer.ProduceAsync(topic, new Message<Null, PermissionRequestedEvent> { Value = permissionRequestedEvent });
                    Console.WriteLine($"Mensaje enviado a {deliveryResult.TopicPartitionOffset}");
                }
                catch (ProduceException<Null, string> ex)
                {
                    Console.WriteLine($"Error al enviar el mensaje: {ex.Error.Reason}");
                }
            }
        }
    }
}*/