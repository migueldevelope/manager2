using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudImpedimentsIniciatives
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id {get;set;}
    public string _idPerson { get; set; }
    public string NamePerson { get; set; }
    public string Description { get; set; }
    public List<ViewCrudLike> Like { get; set; }
    public List<ViewCrudLike> Deslike { get; set; }
    public DateTime? Date { get; set; }
    public EnumTypeImpedimentsIniciatives TypeImpedimentsIniciatives { get; set; }
  }
}
