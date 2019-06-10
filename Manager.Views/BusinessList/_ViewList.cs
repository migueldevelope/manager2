using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Views.BusinessList
{
  public class _ViewList
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
  }
}
