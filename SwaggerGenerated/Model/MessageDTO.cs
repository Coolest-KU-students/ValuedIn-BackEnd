using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class MessageDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public long? Id { get; set; }

    /// <summary>
    /// Gets or Sets SentByFirstName
    /// </summary>
    [DataMember(Name="sentByFirstName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sentByFirstName")]
    public string SentByFirstName { get; set; }

    /// <summary>
    /// Gets or Sets SentByLastName
    /// </summary>
    [DataMember(Name="sentByLastName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sentByLastName")]
    public string SentByLastName { get; set; }

    /// <summary>
    /// Gets or Sets Content
    /// </summary>
    [DataMember(Name="content", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "content")]
    public string Content { get; set; }

    /// <summary>
    /// Gets or Sets Sent
    /// </summary>
    [DataMember(Name="sent", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sent")]
    public DateTime? Sent { get; set; }

    /// <summary>
    /// Gets or Sets Unread
    /// </summary>
    [DataMember(Name="unread", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "unread")]
    public bool? Unread { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MessageDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  SentByFirstName: ").Append(SentByFirstName).Append("\n");
      sb.Append("  SentByLastName: ").Append(SentByLastName).Append("\n");
      sb.Append("  Content: ").Append(Content).Append("\n");
      sb.Append("  Sent: ").Append(Sent).Append("\n");
      sb.Append("  Unread: ").Append(Unread).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
