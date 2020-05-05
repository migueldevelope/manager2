using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Views.BusinessList
{
  public class ViewListStructManagerSend
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idManager { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idAccount { get; set; }
    public string Name { get; set; }
  }
}
