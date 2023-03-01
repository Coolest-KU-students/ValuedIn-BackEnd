using Confluent.Kafka;

namespace ValuedInBE.System.Kafka
{
    public interface IKafkaConfigurationBuilder<TKey, TValue>
    {
        IConsumer<TKey, TValue> ConfigureConsumer();
        IProducer<TKey, TValue> ConfigureProducer();
    }
}
