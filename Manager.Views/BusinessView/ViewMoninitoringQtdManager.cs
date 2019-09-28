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
    public long Total { get; set; }
    public bool PraisesAvg {get;set;}
    public bool CommentsAvg { get; set; }
    public bool PlansAvg { get; set; }
    public long Ranking { get; set; }
  }
}
