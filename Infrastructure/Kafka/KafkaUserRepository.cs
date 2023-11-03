using Application.Abstration;
using Confluent.Kafka;
using Domain;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Infrastructure.Kafka;

public class KafkaUserRepository : IUserRepository
{
    private readonly IKafkaProducer<Null, string> _kafkaProducer;

    public KafkaUserRepository(IKafkaProducer<Null, string> kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }

    public async Task<User> Add(User user)
    {
        string message = JsonConvert.SerializeObject(user);

        var kafkaMessage = new Message<Null, string>
        {
            Value = message
        };

        string kafkaTopic = "mytopic1";
        bool result = await _kafkaProducer.ProduceAsync(kafkaTopic, kafkaMessage);

        if (result)
        {
            // Debug output
            Debug.WriteLine("User data sent to Kafka successfully.");
        }
        else
        {
            // Debug output
            Debug.WriteLine("Failed to send user data to Kafka.");
        }

        return user;
    }

    public Task<User> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User> Get(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<User> Update(User entity)
    {
        throw new NotImplementedException();
    }
}
