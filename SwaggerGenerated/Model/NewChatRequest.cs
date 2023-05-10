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
  public class NewChatRequest {
    /// <summary>
    /// Gets or Sets Participants
    /// </summary>
    [DataMember(Name="participants", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "participants")]
    public List<string> Participants { get; set; }

    /// <summary>
    /// Gets or Sets MessageContent
    /// </summary>
    [DataMember(Name="messageContent", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "messageContent")]
    public string MessageContent { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class NewChatRequest {\n");
      sb.Append("  Participants: ").Append(Participants).Append("\n");
      sb.Append("  MessageContent: ").Append(MessageContent).Append("\n");
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
