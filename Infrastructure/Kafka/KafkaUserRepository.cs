using Application.Abstration;
using Confluent.Kafka;
using Domain;
using System.Diagnostics;

namespace Infrastructure.Kafka;

public class KafkaUserRepository : IUserRepository
{
    private readonly IKafkaProducer<Null, string> _kafkaProducer;

    public KafkaUserRepository(IKafkaProducer<Null, string> kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }

    public async Task<User> AddUser(User user)
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

    public void DeleteUser(int userId)
    {
        throw new NotImplementedException();
    }

    public User GetUser(int userId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> GetUsers()
    {
        throw new NotImplementedException();
    }

    public User UpdateUser(User user)
    {
        throw new NotImplementedException();
    }
}
