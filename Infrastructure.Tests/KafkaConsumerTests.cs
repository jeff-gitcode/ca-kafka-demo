using AutoFixture.Xunit2;
using Confluent.Kafka;
using Domain;
using Infrastructure.Kafka;
using Microsoft.Extensions.Options;
using Moq;

namespace Infrastructure.Tests
{

    public class KafkaConsumerTests
    {

        private readonly Mock<IOptions<KafkaSettings>> _kafkaSettings;
        private readonly Mock<IConsumer<Ignore, string>> _consumerBuilder;

        private KafkaConsumer<User> _sut;

        public KafkaConsumerTests()
        {
            _kafkaSettings = new Mock<IOptions<KafkaSettings>>();
            _consumerBuilder = new Mock<IConsumer<Ignore, string>>();

            _kafkaSettings.Setup(r => r.Value).Returns(new KafkaSettings()
            {
                GroupId = "groupid",
                BrokerLocation = "http:localhost"
            });

            _sut = new KafkaConsumer<User>(_kafkaSettings.Object);
        }

        [Theory, AutoData]
        public async Task KafkaConsumerTests_WhenConsumeAsync_ShouldReturns(KafkaSettings kafkaSettings, User user)
        {
            var cts = new CancellationTokenSource();
            var topic = "user-test";
            var consumeResult = new ConsumeResult<Ignore, string>() { 
            };
            _consumerBuilder.Setup(r => r.Subscribe(topic));

            _consumerBuilder.Setup(r => r.Consume(It.IsAny<CancellationToken>()))
                .Callback(() => Thread.Sleep(TimeSpan.FromMilliseconds(50)))
                .Returns(consumeResult);

            var result = await _sut.ConsumeAsync(topic, cts.Token);

            _consumerBuilder.VerifyAll();
        }
    }
}
