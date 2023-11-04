using Confluent.Kafka;
using Domain;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Kafka
{
    public class KafkaConsumer<T> : IKafkaConsumer<T>
    {
        private readonly KafkaSettings _kafkaSettings;
        private Thread thread;

        public KafkaConsumer(IOptions<KafkaSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings.Value;
        }

        public async Task<IEnumerable<T>> ConsumeAsync(
            string topic,
            CancellationToken cancellationToken = default
        )
        {
            var config = new ConsumerConfig
            {
                GroupId = _kafkaSettings.GroupId,
                BootstrapServers = _kafkaSettings.BrokerLocation,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumerBuilder.Subscribe(topic);

                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumer = consumerBuilder.Consume(cancellationToken);

                    var result = JsonSerializer.Deserialize<T>(consumer.Message.Value);

                    return new T[] { result };
                }
                consumerBuilder.Close();
            }

            return null;
        }
    }

    public interface IKafkaConsumer<T>
    {
        Task<IEnumerable<T>> ConsumeAsync(
            string topic,
            CancellationToken cancellationToken = default
        );
    }
}
