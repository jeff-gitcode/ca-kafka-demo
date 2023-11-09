using System.Text;
using System.Text.Json;
using Application.Users.Commands;
using Confluent.Kafka;
using Domain;
using FluentAssertions;
using Newtonsoft.Json;

namespace Integration.Tests
{
    public class UserControllerTests
    {
        [Theory]
        [UserControllerSetup]
        public async Task UserControllerTests_WhenPushUserToKafka_ShouldReturns(HttpClient client, IConsumer<Null, string> consumer, CreateUserCommand user)
        {
            var userJson = JsonConvert.SerializeObject(user);
            // Act
            var response = await client.PostAsync("/api/user",
                new StringContent(userJson, Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode();

            response = await client.GetAsync("/api/user");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var userList = JsonConvert.DeserializeObject<IEnumerable<User>>(result);

            //var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));
            //var consumedOrder = JsonSerializer.Deserialize<User>(consumeResult.Message.Value);

            userList.FirstOrDefault().Should().BeEquivalentTo(user.user);
        }
    }
}