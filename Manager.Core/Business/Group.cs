using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using MongoDB.Bson;
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
    public ViewListCompany Company { get; set; }
    public ViewListAxis Axis { get; set; }
    public ViewListSphere Sphere { get; set; }
    public long Line { get; set; }
    public List<ViewListSkill> Skills { get; set; }
    public List<ViewCrudSchooling> Schooling { get; set; }
    public List<ViewListScope> Scope { get; set; }
  
    [BsonRepresentation(BsonType.ObjectId)]
    public string Template { get; set; }
    [BsonIgnore]
    public List<ViewListOccupation> Occupations { get; set; }
    public ViewListGroup GetViewList()
    {
      return new ViewListGroup()
      {
        _id = _id,
        Name = Name,
        Axis = Axis,
        Sphere = Sphere,
        Line = Line,
        Company = Company
      };
    }
    public ViewCrudGroup GetViewCrud()
    {
      return new ViewCrudGroup()
      {
        _id = _id,
        Name = Name,
        Axis = Axis,
        Sphere = Sphere,
        Line = Line,
        Company = Company
      };
    }
  }
}
