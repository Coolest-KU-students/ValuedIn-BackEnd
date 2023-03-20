using Confluent.Kafka;
using Newtonsoft.Json;

namespace ValuedInBE.System.External.Services.Kafka.Serializers
{
    public class KafkaJsonSerializer<TModel> : ISerializer<TModel>, IDeserializer<TModel>
    {
        public TModel Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            using var ms = new MemoryStream(data.ToArray());
            var reader = new StreamReader(ms);
            string json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<TModel>(json);
        }

        public byte[] Serialize(TModel data, SerializationContext context)
        {
            using var ms = new MemoryStream();
            string jsonString = JsonConvert.SerializeObject(data);
            var writer = new StreamWriter(ms);

            writer.Write(jsonString);
            writer.Flush();
            ms.Position = 0;

            return ms.ToArray();
        }
    }
}
