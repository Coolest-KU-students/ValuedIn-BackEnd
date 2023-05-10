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
  public class Job {
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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Job {\n");
      sb.Append("  JobAvatar: ").Append(JobAvatar).Append("\n");
      sb.Append("  JobTitle: ").Append(JobTitle).Append("\n");
      sb.Append("  JobTags: ").Append(JobTags).Append("\n");
      sb.Append("  JobValues: ").Append(JobValues).Append("\n");
      sb.Append("  JobMatch: ").Append(JobMatch).Append("\n");
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
