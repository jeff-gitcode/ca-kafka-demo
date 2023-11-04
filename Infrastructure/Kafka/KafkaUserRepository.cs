using Application.Abstration;
using Confluent.Kafka;
using Domain;
using Newtonsoft.Json;

namespace Infrastructure.Kafka;

public class KafkaUserRepository : IUserRepository
{
    private readonly IKafkaProducer<Null, string> _kafkaProducer;
    private readonly IKafkaConsumer<User> _kafkaConsumer;

    public KafkaUserRepository(IKafkaProducer<Null, string> kafkaProducer, IKafkaConsumer<User> kafkaConsumer)
    {
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

        string kafkaTopic = "user-test";
        bool result = await _kafkaProducer.ProduceAsync(kafkaTopic, kafkaMessage);

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
        return await _kafkaConsumer.ConsumeAsync("user-test", cts.Token);
    }

    public Task<User> Update(User entity)
    {
        throw new NotImplementedException();
    }
}
