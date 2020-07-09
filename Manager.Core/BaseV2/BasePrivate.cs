using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.BaseV2
{
#pragma warning disable IDE1006 // Estilos de Nomenclatura
  public class BasePrivate : BasePublic
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idAccount { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _includedPerson { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _alteredPerson { get; set; }
  }
#pragma warning restore IDE1006 // Estilos de Nomenclatura
}
