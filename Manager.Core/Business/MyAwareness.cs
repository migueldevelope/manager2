using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
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
