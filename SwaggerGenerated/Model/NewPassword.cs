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
  public class NewPassword {
    /// <summary>
    /// Gets or Sets OldPassword
    /// </summary>
    [DataMember(Name="oldPassword", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "oldPassword")]
    public string OldPassword { get; set; }

    /// <summary>
    /// Gets or Sets _NewPassword
    /// </summary>
    [DataMember(Name="newPassword", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "newPassword")]
    public string _NewPassword { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class NewPassword {\n");
      sb.Append("  OldPassword: ").Append(OldPassword).Append("\n");
      sb.Append("  _NewPassword: ").Append(_NewPassword).Append("\n");
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
