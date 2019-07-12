using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class Axis : BaseEntity
  {
    public string Name { get; set; }
    public EnumTypeAxis TypeAxis { get; set; }
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Template { get; set; }
    public ViewListCompany Company { get; set; }
    public ViewListAxis GetViewList()
    {
      return new ViewListAxis()
      {
        _id = _id,
        Name = Name,
        TypeAxis = TypeAxis
      };
    }
    public ViewCrudAxis GetViewCrud()
    {
      return new ViewCrudAxis()
      {
        _id = _id,
        Name = Name,
        TypeAxis = TypeAxis,
        Company = Company
      };
    }
  }
}
