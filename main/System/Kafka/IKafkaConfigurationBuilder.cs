using Confluent.Kafka;

namespace ValuedInBE.System.Kafka
{
    public interface IKafkaConfigurationBuilder<TKey, TValue>
    {
        IConsumer<TKey, TValue> ConfigureConsumer(); 
        IConsumer<TKey, TValue> ConfigureConsumer(ConsumerConfig config);
        IProducer<TKey, TValue> ConfigureProducer();
        IProducer<TKey, TValue> ConfigureProducer(ProducerConfig config);
    }
}