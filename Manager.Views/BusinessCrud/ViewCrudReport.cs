using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudReport: _ViewCrudBase
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idAccount { get; set; }
    public EnumStatusReport StatusReport { get; set; }
    public string Link { get; set; }
  }
}
