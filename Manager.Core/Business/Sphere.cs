using Manager.Core.Base;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class Sphere : BaseEntity
  {
    public string Name { get; set; }
    public EnumTypeSphere TypeSphere { get; set; }
    public ViewListCompany Company { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string Template { get; set; }
    public ViewListSphere GetViewList()
    {
      return new ViewListSphere()
      {
        _id = _id,
        Name = Name,
        TypeSphere = TypeSphere
      };
    }
  }
}
