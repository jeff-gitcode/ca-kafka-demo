using AutoFixture;
using Infrastructure.Kafka;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Tests
{

    public class CustomWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
    {
        private readonly IFixture fixture;

        public CustomWebApplicationFactory(IFixture fixture) => this.fixture = fixture;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var kafkaTestConfig = fixture.Create<KafkaSettings>();
                services.Configure<KafkaSettings>(options =>
                {
                    options.TopicName = kafkaTestConfig.TopicName;
                    options.BrokerLocation = kafkaTestConfig.BrokerLocation;
                });
            });
        }
    }
}