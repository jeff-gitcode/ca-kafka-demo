using Confluent.Kafka;

namespace Infrastructure.Kafka
{
    public class KafkaConsumerConfig : ConsumerConfig
    {
        public string Topic { get; set; }
        public KafkaConsumerConfig()
        {
            AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
            EnableAutoCommit = true;
            EnableAutoOffsetStore = false;
        }
    }
}
