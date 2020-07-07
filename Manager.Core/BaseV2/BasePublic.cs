using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Core.BaseV2
{
#pragma warning disable IDE1006 // Estilos de Nomenclatura
  public class BasePublic
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public bool _disabled { get; set; }
    public DateTime _included { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _includedUser { get; set; }
    public DateTime _altered { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _alteredUser { get; set; }
    public long _change { get; set; }
  }
#pragma warning restore IDE1006 // Estilos de Nomenclatura
}
