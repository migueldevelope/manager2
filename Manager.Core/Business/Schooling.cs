using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class Schooling : BaseEntity
  {
    public string Name { get; set; }
    public string Complement { get; set; }
    public EnumTypeSchooling Type { get; set; }

    public long Order { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string Template { get; set; }
    public ViewListSchooling GetViewList()
    {
      return new ViewListSchooling()
      {
        _id = _id,
        Name = Name,
        Order = Order
      };
    }
    public ViewCrudSchooling GetViewCrud()
    {
      return new ViewCrudSchooling()
      {
        _id = _id,
        Name = Name,
        Order = Order,
        Complement = Complement,
        Type = Type
      };
    }
  }
}
