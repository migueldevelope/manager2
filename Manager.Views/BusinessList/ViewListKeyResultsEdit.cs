using Manager.Views.BusinessCrud;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListKeyResultsEdit : ViewCrudKeyResult
  {
    public byte LevelAchievement { get; set; }
    public byte LevelTrust { get; set; }
    public decimal QuantityResult { get; set; }
    public string QualityResult { get; set; }
    public bool PendingChecking { get; set; }
    public bool PendingCheckinTrust { get; set; }
    public bool PendingCheckinAchievement { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPendingChecking { get; set; }
    public long QuantityImpediments { get; set; }
    public long QuantityIniciatives { get; set; }
    public decimal AverageTrust { get; set; }
  }
}
