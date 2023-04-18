using Confluent.Kafka;
using ValuedInBE.System.Configuration.Environmental;
using ValuedInBE.System.Exceptions;
using ValuedInBE.System.External.Services.Kafka.Serializers;

namespace ValuedInBE.System.External.Services.Kafka
{
    public class KafkaConfigurationBuilder<TKey, TValue> : IKafkaConfigurationBuilder<TKey, TValue>
    {
        private const string kafkaServerVariableName = "KAFKA_BROKER_SERVER_NAME";
        private const string kafkaPortVariableName = "KAFKA_ACCESS_PORT";
        private static readonly string _bootstrapServer = 
            $"{EnvironmentVariableAccessor.GetEnvironmentVariable(kafkaServerVariableName)}" +
            $":" +
            $"{EnvironmentVariableAccessor.GetEnvironmentVariable(kafkaPortVariableName)}"; 
        private readonly KafkaJsonSerializer<TValue> _serialization = new();

        public IConsumer<TKey, TValue> ConfigureConsumer()
        {
            return ConfigureConsumer(new());
        }

        public IConsumer<TKey, TValue> ConfigureConsumer(ConsumerConfig config)
        {
            config.BootstrapServers ??= _bootstrapServer;
            return new ConsumerBuilder<TKey, TValue>(config).SetValueDeserializer(_serialization).Build();
        }

        public IProducer<TKey, TValue> ConfigureProducer()
        {
            return ConfigureProducer(new());
        }

        public IProducer<TKey, TValue> ConfigureProducer(ProducerConfig config)
        {
            config.BootstrapServers ??= _bootstrapServer;
            return new ProducerBuilder<TKey, TValue>(config).SetValueSerializer(_serialization).Build();
        }
    }
}
