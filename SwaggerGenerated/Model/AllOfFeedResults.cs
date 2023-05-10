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
  public class AllOfFeedResults : UserFeedInfo {
    /// <summary>
    /// Gets or Sets JobAvatar
    /// </summary>
    [DataMember(Name="jobAvatar", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobAvatar")]
    public List<string> JobAvatar { get; set; }

    /// <summary>
    /// Gets or Sets JobTitle
    /// </summary>
    [DataMember(Name="jobTitle", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobTitle")]
    public string JobTitle { get; set; }

    /// <summary>
    /// Gets or Sets JobTags
    /// </summary>
    [DataMember(Name="jobTags", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobTags")]
    public List<string> JobTags { get; set; }

    /// <summary>
    /// Gets or Sets JobValues
    /// </summary>
    [DataMember(Name="jobValues", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobValues")]
    public List<string> JobValues { get; set; }

    /// <summary>
    /// Gets or Sets JobMatch
    /// </summary>
    [DataMember(Name="jobMatch", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobMatch")]
    public long? JobMatch { get; set; }

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
      sb.Append("class AllOfFeedResults {\n");
      sb.Append("  JobAvatar: ").Append(JobAvatar).Append("\n");
      sb.Append("  JobTitle: ").Append(JobTitle).Append("\n");
      sb.Append("  JobTags: ").Append(JobTags).Append("\n");
      sb.Append("  JobValues: ").Append(JobValues).Append("\n");
      sb.Append("  JobMatch: ").Append(JobMatch).Append("\n");
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
    public  new string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
