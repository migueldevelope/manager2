using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Data.Infrastructure
{
  class Version
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public int Number { get; internal set; }
    public DateTime? Date { get; set; }
  }
}