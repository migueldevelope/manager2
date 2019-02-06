using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business.Integration
{
  public class IntegrationEstablishment : IntegrationBase
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdEstablishment { get; set; }
    public string NameEstablishment { get; set; }
  }
}
