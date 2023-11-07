using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Kafka
{
    public class KafkaConsumer<T> : IKafkaConsumer<T>
    {
        private readonly KafkaSettings _kafkaSettings;
        private readonly IConsumer<Ignore, string> consumerBuilder;

        public KafkaConsumer(IOptions<KafkaSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings.Value;

            var config = new ConsumerConfig
            {
                GroupId = _kafkaSettings.GroupId,
                BootstrapServers = _kafkaSettings.BrokerLocation,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        public async Task<IEnumerable<T>> ConsumeAsync(
            string topic,
            CancellationToken cancellationToken = default
        )
        {
            consumerBuilder.Subscribe(topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumer = consumerBuilder.Consume(cancellationToken);

                if (consumer.IsPartitionEOF) continue;

                if (consumer.Message.Value is null) continue;

                var result = JsonSerializer.Deserialize<T>(consumer.Message.Value);

                return new T[] { result };
            }
            consumerBuilder.Close();

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
