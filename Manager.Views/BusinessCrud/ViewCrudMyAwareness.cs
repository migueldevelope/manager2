using Manager.Views.BusinessList;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMyAwareness: _ViewCrud
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
    public string NamePerson { get; set; }
    public DateTime? Date { get; set; }
    public ViewListMyAwarenessQuestions Reality { get; set; }
    public ViewListMyAwarenessQuestions Impediment { get; set; }
    public ViewListMyAwarenessQuestions FutureVision { get; set; }
    public ViewListMyAwarenessQuestions Planning { get; set; }
  }
}
