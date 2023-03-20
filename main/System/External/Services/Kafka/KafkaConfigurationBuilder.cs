using Confluent.Kafka;
using ValuedInBE.System.External.Services.Kafka.Serializers;

namespace ValuedInBE.System.External.Services.Kafka
{
    public class KafkaConfigurationBuilder<TKey, TValue> : IKafkaConfigurationBuilder<TKey, TValue>
    {
        private const string bootstrapServer = "localhost:29092";
        private readonly KafkaJsonSerializer<TValue> _serialization = new();

        public IConsumer<TKey, TValue> ConfigureConsumer()
        {
            return ConfigureConsumer(new());
        }

        public IProducer<TKey, TValue> ConfigureProducer()
        {
            return ConfigureProducer(new());
        }

        public IConsumer<TKey, TValue> ConfigureConsumer(ConsumerConfig config)
        {
            config.BootstrapServers ??= bootstrapServer;
            return new ConsumerBuilder<TKey, TValue>(config).SetValueDeserializer(_serialization).Build();
        }

        public IProducer<TKey, TValue> ConfigureProducer(ProducerConfig config)
        {
            config.BootstrapServers ??= bootstrapServer;

            return new ProducerBuilder<TKey, TValue>(config).SetValueSerializer(_serialization).Build();
        }
    }
}
