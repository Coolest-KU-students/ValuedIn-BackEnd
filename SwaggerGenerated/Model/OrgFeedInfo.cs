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
  public class OrgFeedInfo {
    /// <summary>
    /// Gets or Sets OrganizationBanner
    /// </summary>
    [DataMember(Name="organizationBanner", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "organizationBanner")]
    public List<string> OrganizationBanner { get; set; }

    /// <summary>
    /// Gets or Sets OrganizationTitle
    /// </summary>
    [DataMember(Name="organizationTitle", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "organizationTitle")]
    public string OrganizationTitle { get; set; }

    /// <summary>
    /// Gets or Sets OrganizationDescription
    /// </summary>
    [DataMember(Name="organizationDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "organizationDescription")]
    public string OrganizationDescription { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class OrgFeedInfo {\n");
      sb.Append("  OrganizationBanner: ").Append(OrganizationBanner).Append("\n");
      sb.Append("  OrganizationTitle: ").Append(OrganizationTitle).Append("\n");
      sb.Append("  OrganizationDescription: ").Append(OrganizationDescription).Append("\n");
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
