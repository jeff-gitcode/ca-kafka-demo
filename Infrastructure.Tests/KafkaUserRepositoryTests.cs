using AutoFixture.Xunit2;
using Confluent.Kafka;
using Domain;
using Infrastructure.Kafka;
using Moq;
using FluentAssertions;

namespace Infrastructure.Tests
{
    public class KafkaUserRepositoryTests
    {
        private readonly Mock<IKafkaProducer<Null, string>> _kafkaProducer;
        private readonly Mock<IKafkaConsumer<User>> _kafkaConsumer;

        private KafkaUserRepository _userRepository;

        public KafkaUserRepositoryTests()
        {
            _kafkaProducer = new Mock<IKafkaProducer<Null, string>>();
            _kafkaConsumer = new Mock<IKafkaConsumer<User>>();

            _userRepository = new KafkaUserRepository(_kafkaProducer.Object, _kafkaConsumer.Object);
        }

        [Theory, AutoData]
        public async Task KafkaUserRepositoryTests_When_AddUser_ShouldReturns(User user)
        {

            _kafkaProducer.Setup(r => r.ProduceAsync("user-test", It.IsAny<Message<Null, string>>())).ReturnsAsync(true);

            var result = await _userRepository.Add(user);

            result.Should().Be(user);
        }

        [Theory, AutoData]
        public async Task KafkaUserRepositoryTests_When_GetUser_ShouldReturns(IEnumerable<User> user)
        {

            _kafkaConsumer.Setup(r => r.ConsumeAsync("user-test", It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var result = await _userRepository.GetAll();

            result.Should().BeEquivalentTo(user);
        }
    }
}