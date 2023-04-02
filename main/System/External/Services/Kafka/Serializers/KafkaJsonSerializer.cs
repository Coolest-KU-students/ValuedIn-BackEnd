using Confluent.Kafka;
using Newtonsoft.Json;

namespace ValuedInBE.System.External.Services.Kafka.Serializers
{
    public class KafkaJsonSerializer<TModel> : ISerializer<TModel>, IDeserializer<TModel>
    {
        public TModel Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            using MemoryStream ms = new(data.ToArray());
            StreamReader reader = new(ms);
            string json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<TModel>(json);
        }

        public byte[] Serialize(TModel data, SerializationContext context)
        {
            using MemoryStream ms = new();
            string jsonString = JsonConvert.SerializeObject(data);
            StreamWriter writer = new(ms);

            writer.Write(jsonString);
            writer.Flush();
            ms.Position = 0;

            return ms.ToArray();
        }
    }
}
