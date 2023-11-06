using Confluent.Kafka;

namespace Infrastructure.Kafka
{
    public class KafkaProducerConfig : ProducerConfig
    {
        public required string Topic { get; set; }
    }
}
