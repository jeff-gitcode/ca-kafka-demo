using AutoFixture;
using Confluent.Kafka.Admin;
using Confluent.Kafka;
using System.Net.Sockets;
using System.Net;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Nito.AsyncEx;
using Infrastructure.Kafka;

namespace Integration.Tests
{
    public class TestContainersSetup : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var zookeeperContainerName = $"zookeeper_{fixture.Create<string>()}";
            var zookeeperContainer = new ContainerBuilder()
                .WithImage("confluentinc/cp-zookeeper:7.0.1")
                .WithName(zookeeperContainerName)
                .WithPortBinding(2181, true)
                .WithEnvironment(
                    new Dictionary<string, string>
                    {
                        { "ZOOKEEPER_CLIENT_PORT", "2181" },
                        { "ZOOKEEPER_TICK_TIME", "2000" }
                    }
                )
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(2181))
                .Build();

            AsyncContext.Run(async () => await zookeeperContainer.StartAsync());

            var zookeeperHostPort = zookeeperContainer.GetMappedPublicPort(2181);

            var kafkaHostPort = GetAvailablePort();
            var kafkaContainerName = $"kafka_{fixture.Create<string>()}";
            var kafkaContainer = new ContainerBuilder()
                .WithImage("confluentinc/cp-kafka:7.0.1")
                .WithName(kafkaContainerName)
                .WithHostname(kafkaContainerName)
                .WithPortBinding(kafkaHostPort, 9092)
                .WithEnvironment(
                    new Dictionary<string, string>
                    {
                        { "KAFKA_BROKER_ID", "1" },
                        { "KAFKA_ZOOKEEPER_CONNECT", $"host.docker.internal:{zookeeperHostPort}" },
                        {
                            "KAFKA_LISTENER_SECURITY_PROTOCOL_MAP",
                            "PLAINTEXT:PLAINTEXT,PLAINTEXT_INTERNAL:PLAINTEXT"
                        },
                        { "KAFKA_LISTENERS", "PLAINTEXT://:9092,PLAINTEXT_INTERNAL://:29092" },
                        {
                            "KAFKA_ADVERTISED_LISTENERS",
                            $"PLAINTEXT://localhost:{kafkaHostPort},PLAINTEXT_INTERNAL://{kafkaContainerName}:29092"
                        },
                         { "KAFKA_INTER_BROKER_LISTENER_NAME", "PLAINTEXT_INTERNAL" },
                        // { "KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR", "1" },
                        // { "KAFKA_TRANSACTION_STATE_LOG_MIN_ISR", "1" },
                        // { "KAFKA_TRANSACTION_STATE_LOG_REPLICATION_FACTOR", "1" }

                        { "KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR", "1" },
                        { "KAFKA_GROUP_INITIAL_REBALANCE_DELAY_MS", "0" },
                        { "KAFKA_CONFLUENT_LICENSE_TOPIC_REPLICATION_FACTOR", "1" },
                        { "CONFLUENT_SUPPORT_CUSTOMER_ID", "anonymous" }
                    }
                )
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(9092))
                .Build();

            AsyncContext.Run(async () => await kafkaContainer.StartAsync());

            var topicName = $"users_{fixture.Create<string>()}";
            var brokerLocation = $"localhost:{kafkaHostPort}";

            AsyncContext.Run(async () => await CreateKafkaTopic(topicName, brokerLocation));

            fixture.Inject(
                new KafkaSettings { TopicName = topicName, BrokerLocation = brokerLocation }
            );

            var kafDropContainerName = $"kafDrop_{fixture.Create<string>()}";
            var kafDropContainer = new ContainerBuilder()
                .WithImage("obsidiandynamics/kafdrop")
                .WithName(kafDropContainerName)
                .WithPortBinding(9000, true)
                .WithEnvironment(
                    new Dictionary<string, string>
                    {
                        { "KAFKA_BROKERCONNECT", $"localhost:{kafkaHostPort}" }
                    }
                )
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(9000))
                .Build();

            AsyncContext.Run(async () => await kafDropContainer.StartAsync());
        }

        private static int GetAvailablePort()
        {
            IPEndPoint defaultLoopbackEndpoint = new(IPAddress.Loopback, 0);

            using var socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
            socket.Bind(defaultLoopbackEndpoint);
            var port = ((IPEndPoint)socket.LocalEndPoint)!.Port;

            return port;
        }

        private static async Task CreateKafkaTopic(string topicName, string bootstrapServers)
        {
            using var adminClient = new AdminClientBuilder(
                new AdminClientConfig { BootstrapServers = bootstrapServers }
            ).Build();

            await adminClient.CreateTopicsAsync(
                new TopicSpecification[]
                {
                    new()
                    {
                        Name = topicName,
                        ReplicationFactor = 1,
                        NumPartitions = 1
                    }
                }
            );
        }
    }
}
