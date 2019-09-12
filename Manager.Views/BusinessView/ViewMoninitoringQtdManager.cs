using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Views.BusinessView
{
  public class ViewMoninitoringQtdManager
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idManager { get; set; }
    public string Manager { get; set; }
    public long Praises { get; set; }
    public long Comments { get; set; }
    public long Plans { get; set; }
  }
}
