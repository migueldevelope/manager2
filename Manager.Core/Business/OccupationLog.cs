using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Core.Business
{
  public class OccupationLog: Occupation
  {
    public DateTime? Date { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idOccupationPrevious { get; set; }
  }
}
