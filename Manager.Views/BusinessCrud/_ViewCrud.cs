using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Views.BusinessCrud
{
  public class _ViewCrud
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public long _change { get; set; }
  }
}