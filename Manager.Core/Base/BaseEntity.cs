using Manager.Core.Enumns;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Base
{
  public class BaseEntity
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public EnumStatus Status { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idAccount { get; set; }
  }
}
