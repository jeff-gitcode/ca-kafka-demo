using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Infrastructure.Kafka
{
    public class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
    {
        private readonly KafkaSettings _kafkaSettings;
        private readonly IProducer<TKey, TValue> _producer;

        public KafkaProducer(IOptions<KafkaSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings.Value;

            var config = new ProducerConfig
            {
                BootstrapServers = _kafkaSettings.BrokerLocation,
            };

            _producer = new ProducerBuilder<TKey, TValue>(config).Build();

        }

        public async Task<bool> ProduceAsync(string topic, Message<TKey, TValue> message)
        {
            try
            {

                var deliveryReport = await _producer.ProduceAsync(topic, message);

                return deliveryReport.Status == PersistenceStatus.Persisted;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kafka error: {ex.Message}");
                return false;
            }
        }
    }

    public interface IKafkaProducer<TKey, TValue>
    {
        Task<bool> ProduceAsync(string topic, Message<TKey, TValue> message);
    }
}
