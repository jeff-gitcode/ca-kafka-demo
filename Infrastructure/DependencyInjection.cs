using Application.Abstration;
using Ardalis.GuardClauses;
using Confluent.Kafka;
using Infrastructure.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddKafkaProducer<string, object>(options =>
        {
            options.Topic = "user-test";
            options.BootstrapServers = "localhost:9092";
        });

        services.AddKafkaConsumer<string, object>(options =>
        {
            options.Topic = "user-test";
            options.BootstrapServers = "localhost:9092";
        });

        services.AddScoped<IUserRepository, KafkaUserRepository>();
        return services;
    }

    public static IServiceCollection AddKafkaConsumer<TKey, TValue>(this IServiceCollection services,
    Action<KafkaConsumerConfig> config)
    {
        services.Configure(config);

        services.AddScoped(typeof(IKafkaConsumer<,>), typeof(KafkaConsumer<,>));

        return services;
    }

    private static IServiceCollection AddKafkaProducer<Tkey, TValue>(
        this IServiceCollection services,
        Action<KafkaProducerConfig> config
    )
    {
        services.Configure(config);

        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IOptions<KafkaProducerConfig>>();

            Guard.Against.Null(config);

            var builder = new ProducerBuilder<Tkey, TValue>(config.Value).SetValueSerializer(new KafkaSerializer<TValue>());

            return builder.Build();
        });

        services.AddScoped(typeof(IKafkaProducer<,>), typeof(KafkaProducer<,>));

        return services;
    }
}
