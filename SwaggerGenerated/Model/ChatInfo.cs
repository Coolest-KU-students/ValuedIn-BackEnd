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
  public class ChatInfo {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public long? Id { get; set; }

    /// <summary>
    /// Gets or Sets ChatName
    /// </summary>
    [DataMember(Name="chatName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "chatName")]
    public string ChatName { get; set; }

    /// <summary>
    /// Gets or Sets ParticipatingUsers
    /// </summary>
    [DataMember(Name="participatingUsers", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "participatingUsers")]
    public List<string> ParticipatingUsers { get; set; }

    /// <summary>
    /// Gets or Sets LastMessage
    /// </summary>
    [DataMember(Name="lastMessage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lastMessage")]
    public DateTime? LastMessage { get; set; }

    /// <summary>
    /// Gets or Sets LastMessageContent
    /// </summary>
    [DataMember(Name="lastMessageContent", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lastMessageContent")]
    public string LastMessageContent { get; set; }

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
      sb.Append("class ChatInfo {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  ChatName: ").Append(ChatName).Append("\n");
      sb.Append("  ParticipatingUsers: ").Append(ParticipatingUsers).Append("\n");
      sb.Append("  LastMessage: ").Append(LastMessage).Append("\n");
      sb.Append("  LastMessageContent: ").Append(LastMessageContent).Append("\n");
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
