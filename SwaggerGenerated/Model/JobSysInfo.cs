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
  public class JobSysInfo {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public long? Id { get; set; }

    /// <summary>
    /// Gets or Sets Avatar
    /// </summary>
    [DataMember(Name="avatar", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "avatar")]
    public List<string> Avatar { get; set; }

    /// <summary>
    /// Gets or Sets Title
    /// </summary>
    [DataMember(Name="title", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; }

    /// <summary>
    /// Gets or Sets Position
    /// </summary>
    [DataMember(Name="position", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "position")]
    public string Position { get; set; }

    /// <summary>
    /// Gets or Sets Location
    /// </summary>
    [DataMember(Name="location", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "location")]
    public string Location { get; set; }

    /// <summary>
    /// Gets or Sets JobDescription
    /// </summary>
    [DataMember(Name="job description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "job description")]
    public string JobDescription { get; set; }

    /// <summary>
    /// Gets or Sets Values
    /// </summary>
    [DataMember(Name="values", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "values")]
    public string Values { get; set; }

    /// <summary>
    /// Gets or Sets Match
    /// </summary>
    [DataMember(Name="match", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "match")]
    public long? Match { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class JobSysInfo {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Avatar: ").Append(Avatar).Append("\n");
      sb.Append("  Title: ").Append(Title).Append("\n");
      sb.Append("  Position: ").Append(Position).Append("\n");
      sb.Append("  Location: ").Append(Location).Append("\n");
      sb.Append("  JobDescription: ").Append(JobDescription).Append("\n");
      sb.Append("  Values: ").Append(Values).Append("\n");
      sb.Append("  Match: ").Append(Match).Append("\n");
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
