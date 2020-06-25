using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class PendingCheckinObjective : BaseEntity
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idObjective { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idKeyResult { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
    public EnumLevelTrust LevelTrust { get; set; }
    public DateTime? Date { get; set; }
    public List<ViewCrudImpedimentsIniciatives> Impediments { get; set; }
    public List<ViewCrudImpedimentsIniciatives> Iniciatives { get; set; }
    public decimal Achievement { get; set; }
    public decimal QuantityResult { get; set; }
    public string QualityResult { get; set; }
    public decimal QuantityGoal { get; set; }
    public string QualityGoal { get; set; }
    public int Week { get; set; }

    public bool Lasted { get; set; }

    public int Month { get; set; }
    public byte Fortnight { get; set; }
    public EnumTypePersonObjective TypePersonObjective { get; set; }
    public ViewCrudPendingCheckinObjective GetViewCrud()
    {
      return new ViewCrudPendingCheckinObjective
      {
        _id = _id,
        _idObjective = _idObjective,
        _idKeyResult = _idKeyResult,
        _idPerson = _idPerson,
        LevelTrust = LevelTrust,
        Date = Date,
        Impediments = Impediments,
        Iniciatives = Iniciatives,
        Achievement = Achievement,
        QuantityResult = QuantityResult,
        QualityResult = QualityResult,
        QuantityGoal = QuantityGoal,
        QualityGoal = QualityGoal,
        Lasted = Lasted,
        Week = Week,
        Month = Month,
        Fortnight= Fortnight,
        TypePersonObjective = TypePersonObjective
  };
    }

    public ViewListPendingCheckinObjective GetViewList()
    {
      return new ViewListPendingCheckinObjective
      {
        _id = _id
      };
    }
  }
}
