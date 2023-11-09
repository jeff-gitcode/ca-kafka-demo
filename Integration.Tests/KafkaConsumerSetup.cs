using AutoFixture;
using Confluent.Kafka;
using Infrastructure.Kafka;

namespace Integration.Tests
{
    internal class KafkaConsumerSetup : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var kafkaConfig = fixture.Create<KafkaSettings>();

            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaConfig.BrokerLocation,
                GroupId = Guid.NewGuid().ToString(),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumer = new ConsumerBuilder<Null, string>(config).Build();

            consumer.Subscribe(kafkaConfig.TopicName);

            fixture.Inject(consumer);
        }
    }
}