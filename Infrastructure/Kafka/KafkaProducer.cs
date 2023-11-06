using Ardalis.GuardClauses;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Infrastructure.Kafka
{
    public class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
    {
        private readonly IProducer<TKey, TValue> _producer;
        private readonly KafkaProducerConfig _config;
        public KafkaProducer(IOptions<KafkaProducerConfig> config, IProducer<TKey, TValue> producer)
        {
            Guard.Against.Null(producer);
            Guard.Against.Null(config);

            _config = config.Value;
            _producer = producer;
        }

        public async Task<bool> ProduceAsync(Message<TKey, TValue> message)
        {
            try
            {
                    var deliveryReport = await _producer.ProduceAsync(_config.Topic, message);

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
        Task<bool> ProduceAsync(Message<TKey, TValue> message);
    }
}


