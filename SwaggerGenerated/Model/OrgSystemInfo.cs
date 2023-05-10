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
  public class OrgSystemInfo {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public long? Id { get; set; }

    /// <summary>
    /// Gets or Sets Banner
    /// </summary>
    [DataMember(Name="banner", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "banner")]
    public List<string> Banner { get; set; }

    /// <summary>
    /// Gets or Sets Title
    /// </summary>
    [DataMember(Name="title", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; }

    /// <summary>
    /// Gets or Sets City
    /// </summary>
    [DataMember(Name="city", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "city")]
    public string City { get; set; }

    /// <summary>
    /// Gets or Sets Values
    /// </summary>
    [DataMember(Name="values", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "values")]
    public List<string> Values { get; set; }

    /// <summary>
    /// Gets or Sets Match
    /// </summary>
    [DataMember(Name="match", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "match")]
    public long? Match { get; set; }

    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or Sets Owner
    /// </summary>
    [DataMember(Name="owner", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "owner")]
    public string Owner { get; set; }

    /// <summary>
    /// Gets or Sets Positions
    /// </summary>
    [DataMember(Name="positions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "positions")]
    public string Positions { get; set; }

    /// <summary>
    /// Gets or Sets EmployeeValues
    /// </summary>
    [DataMember(Name="employeeValues", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "employeeValues")]
    public List<string> EmployeeValues { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class OrgSystemInfo {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Banner: ").Append(Banner).Append("\n");
      sb.Append("  Title: ").Append(Title).Append("\n");
      sb.Append("  City: ").Append(City).Append("\n");
      sb.Append("  Values: ").Append(Values).Append("\n");
      sb.Append("  Match: ").Append(Match).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  Owner: ").Append(Owner).Append("\n");
      sb.Append("  Positions: ").Append(Positions).Append("\n");
      sb.Append("  EmployeeValues: ").Append(EmployeeValues).Append("\n");
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
