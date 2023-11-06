using Ardalis.GuardClauses;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Infrastructure.Kafka
{
    public class KafkaConsumer<TKey, TValue> : IKafkaConsumer<TKey,TValue>
    {
        private readonly KafkaConsumerConfig _config;

        public KafkaConsumer(IOptions<KafkaConsumerConfig> config)
        {
            Guard.Against.Null(config);

            _config = config.Value;
        }

        public async Task<IEnumerable<TValue>> ConsumeAsync(
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                using var consumerBuilder = new ConsumerBuilder<TKey, TValue>(_config).SetValueDeserializer(new KafkaDeserializer<TValue>()).Build();

                consumerBuilder.Subscribe(_config.Topic);

                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = consumerBuilder.Consume(TimeSpan.FromMilliseconds(1000));

                    if (result == null)
                    {
                        return new TValue[] { };
                    }


                    consumerBuilder.Commit(result);
                    consumerBuilder.StoreOffset(result);
                    var value = result.Message.Value ?? default(TValue);
                    return new TValue[] { value };
                }

                return new TValue[] { };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kafka error: {ex.Message}");
                throw;
            }

        }
    }

    public interface IKafkaConsumer<T, Tvalue>
    {
        Task<IEnumerable<Tvalue>> ConsumeAsync(
            CancellationToken cancellationToken = default
        );
    }
}
