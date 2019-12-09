using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Core.Business
{
  public class OccupationLog: Occupation
  {
    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime? Date { get; set; }
    public DateTime? DateLog { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idOccupationPrevious { get; set; }
  }
}
