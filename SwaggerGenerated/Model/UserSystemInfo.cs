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
  public class UserSystemInfo {
    /// <summary>
    /// Gets or Sets Login
    /// </summary>
    [DataMember(Name="login", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "login")]
    public string Login { get; set; }

    /// <summary>
    /// Gets or Sets UserID
    /// </summary>
    [DataMember(Name="userID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userID")]
    public string UserID { get; set; }

    /// <summary>
    /// Gets or Sets IsExpired
    /// </summary>
    [DataMember(Name="isExpired", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isExpired")]
    public bool? IsExpired { get; set; }

    /// <summary>
    /// Gets or Sets LastActive
    /// </summary>
    [DataMember(Name="lastActive", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lastActive")]
    public DateTime? LastActive { get; set; }

    /// <summary>
    /// Gets or Sets Role
    /// </summary>
    [DataMember(Name="role", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "role")]
    public string Role { get; set; }

    /// <summary>
    /// Gets or Sets FirstName
    /// </summary>
    [DataMember(Name="firstName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "firstName")]
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or Sets LastName
    /// </summary>
    [DataMember(Name="lastName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lastName")]
    public string LastName { get; set; }

    /// <summary>
    /// Gets or Sets Email
    /// </summary>
    [DataMember(Name="email", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "email")]
    public string Email { get; set; }

    /// <summary>
    /// Gets or Sets Telephone
    /// </summary>
    [DataMember(Name="telephone", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "telephone")]
    public string Telephone { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UserSystemInfo {\n");
      sb.Append("  Login: ").Append(Login).Append("\n");
      sb.Append("  UserID: ").Append(UserID).Append("\n");
      sb.Append("  IsExpired: ").Append(IsExpired).Append("\n");
      sb.Append("  LastActive: ").Append(LastActive).Append("\n");
      sb.Append("  Role: ").Append(Role).Append("\n");
      sb.Append("  FirstName: ").Append(FirstName).Append("\n");
      sb.Append("  LastName: ").Append(LastName).Append("\n");
      sb.Append("  Email: ").Append(Email).Append("\n");
      sb.Append("  Telephone: ").Append(Telephone).Append("\n");
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
