using Confluent.Kafka;
using ValuedInBE.System.External.Services.Kafka.Serializers;

namespace ValuedInBE.System.External.Services.Kafka
{
    public class KafkaConfigurationBuilder<TKey, TValue> : IKafkaConfigurationBuilder<TKey, TValue>
    {
        private const string bootstrapServer = "kafka:29091"; // TODO: user secrets or config file, can't have it like this
        private readonly KafkaJsonSerializer<TValue> _serialization = new();

        public IConsumer<TKey, TValue> ConfigureConsumer()
        {
            return ConfigureConsumer(new());
        }

        public IConsumer<TKey, TValue> ConfigureConsumer(ConsumerConfig config)
        {
            config.BootstrapServers ??= bootstrapServer;
            return new ConsumerBuilder<TKey, TValue>(config).SetValueDeserializer(_serialization).Build();
        }

        public IProducer<TKey, TValue> ConfigureProducer()
        {
            return ConfigureProducer(new());
        }

        public IProducer<TKey, TValue> ConfigureProducer(ProducerConfig config)
        {
            config.BootstrapServers ??= bootstrapServer;
            return new ProducerBuilder<TKey, TValue>(config).SetValueSerializer(_serialization).Build();
        }
    }
}
