using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business
{
  public class ListManager
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idManager {get;set;}
  }
}
