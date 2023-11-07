using Confluent.Kafka;
using Domain;
using Infrastructure.Kafka;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading;

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

            _sut = new KafkaConsumer<User>(_kafkaSettings.Object);
        }

        [Fact]
        public async Task KafkaConsumerTests_WhenConsumeAsync_ShouldReturns()
        {
            using var waitHandle = new AutoResetEvent(false);

            _consumerBuilder.Setup(r => r.Subscribe(It.IsAny<string>()));

            _consumerBuilder.Setup(r=> r.Consume(It.IsAny<CancellationToken>())).Callback<CancellationToken>(_ =>
            {
                //wait until signaled to finish
                waitHandle.WaitOne();
            });

            _consumerBuilder.Setup(r => r.Close());


        }
    }
}
