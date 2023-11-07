using Application.Abstration;
using Confluent.Kafka;
using Infrastructure.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IKafkaConsumer<>), typeof(KafkaConsumer<>));

        services.AddSingleton(typeof(IKafkaProducer<,>), typeof(KafkaProducer<,>));

        services.AddScoped<IUserRepository, KafkaUserRepository>();

        return services;
    }
}
