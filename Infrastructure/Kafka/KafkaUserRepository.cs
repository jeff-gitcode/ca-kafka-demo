using Application.Abstration;
using Ardalis.GuardClauses;
using Confluent.Kafka;
using Domain;
using MediatR;
using Newtonsoft.Json;

namespace Infrastructure.Kafka;

public class KafkaUserRepository : IUserRepository
{
    private readonly IKafkaProducer<Null, string> _kafkaProducer;
    private readonly IKafkaConsumer<Ignore, User> _kafkaConsumer;

    public KafkaUserRepository(IKafkaProducer<Null, string> kafkaProducer, IKafkaConsumer<Ignore, User> kafkaConsumer)
    {
        Guard.Against.Null(kafkaProducer);
        Guard.Against.Null(kafkaConsumer);

        _kafkaProducer = kafkaProducer;
        _kafkaConsumer = kafkaConsumer;
    }

    public async Task<User> Add(User user)
    {
        string message = JsonConvert.SerializeObject(user);

        var kafkaMessage = new Message<Null, string>
        {
            Value = message
        };

        bool result = await _kafkaProducer.ProduceAsync(kafkaMessage);

        if (result)
        {
            return user;
        }

        return null;
    }

    public Task<User> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User> Get(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        var cts = new CancellationTokenSource();
        return await _kafkaConsumer.ConsumeAsync(cts.Token);
    }

    public Task<User> Update(User entity)
    {
        throw new NotImplementedException();
    }
}
