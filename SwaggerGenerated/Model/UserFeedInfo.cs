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
  public class UserFeedInfo {
    /// <summary>
    /// Gets or Sets UserFirstName
    /// </summary>
    [DataMember(Name="userFirstName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userFirstName")]
    public string UserFirstName { get; set; }

    /// <summary>
    /// Gets or Sets UserLastName
    /// </summary>
    [DataMember(Name="userLastName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userLastName")]
    public string UserLastName { get; set; }

    /// <summary>
    /// Gets or Sets UserPosition
    /// </summary>
    [DataMember(Name="userPosition", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userPosition")]
    public string UserPosition { get; set; }

    /// <summary>
    /// Gets or Sets UserMatch
    /// </summary>
    [DataMember(Name="userMatch", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userMatch")]
    public decimal? UserMatch { get; set; }

    /// <summary>
    /// Gets or Sets UserValues
    /// </summary>
    [DataMember(Name="userValues", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userValues")]
    public List<string> UserValues { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UserFeedInfo {\n");
      sb.Append("  UserFirstName: ").Append(UserFirstName).Append("\n");
      sb.Append("  UserLastName: ").Append(UserLastName).Append("\n");
      sb.Append("  UserPosition: ").Append(UserPosition).Append("\n");
      sb.Append("  UserMatch: ").Append(UserMatch).Append("\n");
      sb.Append("  UserValues: ").Append(UserValues).Append("\n");
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
