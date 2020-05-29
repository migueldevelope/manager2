using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudLike
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
    public DateTime? Date { get; set; }
  }
}
