using Confluent.Kafka;

namespace ValuedInBE.System.Kafka
{
    public class KafkaConfigurationBuilder<TKey, TValue> : IKafkaConfigurationBuilder<TKey, TValue>
    {
        private const string bootstrapServer = "localhost:9092";

        private static ConsumerConfig consumerConfig = new()
        {
            BootstrapServers = bootstrapServer,
            GroupId = "my-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        }; 
        private static ProducerConfig producerConfig = new()
        {
            BootstrapServers = bootstrapServer
        };

        public const string newChatMessageTopic = "New-Chat-Message";

        public IConsumer<TKey, TValue> ConfigureConsumer()
        {
            ConsumerBuilder<TKey, TValue> builder = new(consumerConfig);
            var consumer = builder.Build();
            return consumer;
        }


        public IProducer<TKey, TValue> ConfigureProducer()
        {
            ProducerBuilder<TKey, TValue> builder = new(producerConfig);
            return builder.Build();
        }

    }
}
