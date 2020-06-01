using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Views.BusinessList
{
  public class ViewListDetailResposibleObjective
  {
    public decimal AverageAchievement { get; set; }
    public decimal AverageTrust { get; set; }
    public string Description { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public long Impediments { get; set; }
    public long Iniciatives { get; set; }
    public byte LevelAchievement { get; set; }
    public byte LevelTrust { get; set; }
  }
}
