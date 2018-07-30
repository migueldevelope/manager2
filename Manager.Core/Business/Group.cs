using Manager.Core.Base;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Group : BaseEntity
  {
    public string Name { get; set; }
    public Company Company { get; set; }
    public Axis Axis { get; set; }
    public Sphere Sphere { get; set; }
    public long Line { get; set; }
    public List<Skill> Skills { get; set; }
    public List<Schooling> Schooling { get; set; }
    public List<Scope> Scope { get; set; }
    public Group Template { get; set; }
    [BsonIgnore]
    public List<Occupation> Occupations { get; set; }
  }
}
