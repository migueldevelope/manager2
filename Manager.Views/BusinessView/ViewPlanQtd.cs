using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Views.BusinessView
{
  public class ViewPlanQtd
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idManager { get; set; }
    public string Manager { get; set; }
    public long Schedule { get; set; }
    public long Realized { get; set; }
    public long Late { get; set; }
    public long Balance { get; set; }
    public long Ranking { get; set; }
  }
}
