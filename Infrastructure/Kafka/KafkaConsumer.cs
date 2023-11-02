using Confluent.Kafka;
using Domain;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Kafka
{
    public class KafkaConsumer: IKafkaConsumer
    {
        private readonly KafkaSettings _kafkaSettings;
        private Thread thread;

        public KafkaConsumer(IOptions<KafkaSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings.Value;
        }

        public async Task ConsumeAsync(string topic, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _kafkaSettings.GroupId,
                BootstrapServers = _kafkaSettings.BrokerLocation,

                AutoOffsetReset = AutoOffsetReset.Latest
            };

            using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumerBuilder.Subscribe(topic);

                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumer = consumerBuilder.Consume(cancellationToken);
                    var signRequest = JsonSerializer.Deserialize
                                <User>
                                    (consumer.Message.Value);

                }
                consumerBuilder.Close();
            }
        }
    }

    public interface IKafkaConsumer
    {
        Task ConsumeAsync(string topic, CancellationToken cancellationToken);
    }
}
