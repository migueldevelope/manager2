using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Views.BusinessList
{
  public class ViewListDetailResposibleObjective
  {
    public decimal AverageAchievement { get; set; }
    public decimal AverageTrust { get; set; }
    public string Description { get; set; }
    public string Detail { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public long Impediments { get; set; }
    public long Iniciatives { get; set; }
    public byte LevelAchievement { get; set; }
    public byte LevelTrust { get; set; }
  }
}
