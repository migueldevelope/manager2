using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListStructManager
  {
    public string Name { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idManager { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
    public List<ViewListStructManager> Team { get; set; }
  }
}
