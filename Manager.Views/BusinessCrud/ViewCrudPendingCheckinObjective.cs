using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudPendingCheckinObjective : _ViewCrud
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idObjective { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idKeyResult { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
    public EnumLevelTrust LevelTrust { get; set; }
    public DateTime? Date { get; set; }
    public int Week { get; set; }
    public int Month { get; set; }
    public List<ViewCrudImpedimentsIniciatives> Impediments { get; set; }
    public List<ViewCrudImpedimentsIniciatives> Iniciatives { get; set; }
    public decimal Achievement { get; set; }
    public decimal QuantityResult { get; set; }
    public string QualityResult { get; set; }
    public EnumTypePersonObjective TypePersonObjective { get; set; }


  }
}
