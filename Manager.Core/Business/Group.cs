using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
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
    public ViewListGroup GetViewList()
    {
      return new ViewListGroup()
      {
        _id = _id,
        Name = Name,
        Axis = Axis.GetViewList(),
        Sphere = Sphere.GetViewList(),
        Line = Line
      };
    }
    public ViewCrudGroup GetViewCrud()
    {
      return new ViewCrudGroup()
      {
        _id = _id,
        Name = Name,
        Axis = Axis.GetViewList(),
        Sphere = Sphere.GetViewList(),
        Line = Line,
        Company = Company.GetViewList()
      };
    }
  }
}
