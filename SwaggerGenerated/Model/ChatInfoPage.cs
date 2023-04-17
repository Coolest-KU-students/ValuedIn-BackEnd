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
  public class ChatInfoPage {
    /// <summary>
    /// Gets or Sets Results
    /// </summary>
    [DataMember(Name="results", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "results")]
    public List<ChatInfo> Results { get; set; }

    /// <summary>
    /// Gets or Sets NextOffset
    /// </summary>
    [DataMember(Name="nextOffset", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "nextOffset")]
    public DateTime? NextOffset { get; set; }

    /// <summary>
    /// Gets or Sets Last
    /// </summary>
    [DataMember(Name="last", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "last")]
    public bool? Last { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ChatInfoPage {\n");
      sb.Append("  Results: ").Append(Results).Append("\n");
      sb.Append("  NextOffset: ").Append(NextOffset).Append("\n");
      sb.Append("  Last: ").Append(Last).Append("\n");
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
