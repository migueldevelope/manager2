using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudImpedimentsIniciatives
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id {get;set;}
    public string Description { get; set; }
    public List<ViewCrudLike> Like { get; set; }
    public List<ViewCrudLike> Deslike { get; set; }
  }
}
