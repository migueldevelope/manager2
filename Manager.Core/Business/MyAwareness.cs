using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Core.Business
{
  public class MyAwareness : BaseEntity
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
    public string NamePerson { get; set; }
    public DateTime? Date { get; set; }
    public MyAwarenessQuestions Reality { get; set; }
    public MyAwarenessQuestions Impediment { get; set; }
    public MyAwarenessQuestions FutureVision { get; set; }
    public MyAwarenessQuestions Planning { get; set; }

    public ViewCrudMyAwareness GetViewCrud()
    {
      return new ViewCrudMyAwareness()
      {
        _id = _id,
        _idPerson = _idPerson,
        NamePerson = NamePerson,
        Date = Date,
        Reality = Reality?.GetViewList(),
        Impediment = Impediment?.GetViewList(),
        FutureVision = FutureVision?.GetViewList(),
        Planning = Planning?.GetViewList()
      };
    }

    public ViewMyAwareness GetViewExport()
    {
      return new ViewMyAwareness()
      {
        _idPerson = _idPerson,
        NamePerson = NamePerson,
        Date = Date == null ? "" : Date.Value.ToString("dd/MM/yyyy hh:mm"),
        RealitySelfImage = Reality?.SelfImage,
        RealityWorker = Reality?.Worker,
        RealityPersonalRelationships = Reality?.PersonalRelationships,
        RealityPersonalInterest = Reality?.PersonalInterest,
        RealityHealth = Reality?.Health,
        RealityPurposeOfLife = Reality?.PurposeOfLife,
        ImpedimentSelfImage = Impediment?.SelfImage,
        ImpedimentWorker = Impediment?.Worker,
        ImpedimentPersonalRelationships = Impediment?.PersonalRelationships,
        ImpedimentPersonalInterest = Impediment?.PersonalInterest,
        ImpedimentHealth = Impediment?.Health,
        ImpedimentPurposeOfLife = Impediment?.PurposeOfLife,
        FutureVisionSelfImage = FutureVision?.SelfImage,
        FutureVisionWorker = FutureVision?.Worker,
        FutureVisionPersonalRelationships = FutureVision?.PersonalRelationships,
        FutureVisionPersonalInterest = FutureVision?.PersonalInterest,
        FutureVisionHealth = FutureVision?.Health,
        FutureVisionPurposeOfLife = FutureVision?.PurposeOfLife,
        PlanningSelfImage = Planning?.SelfImage,
        PlanningWorker = Planning?.Worker,
        PlanningPersonalRelationships = Planning?.PersonalRelationships,
        PlanningPersonalInterest = Planning?.PersonalInterest,
        PlanningHealth = Planning?.Health,
        PlanningPurposeOfLife = Planning?.PurposeOfLife,
      };
    }

    public ViewListMyAwareness GetViewList()
    {
      return new ViewListMyAwareness()
      {
        _id = _id,
        _idPerson = _idPerson,
        NamePerson = NamePerson,
        Date = Date
      };
    }
  }
}
