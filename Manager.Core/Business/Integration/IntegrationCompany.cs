using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business.Integration
{
  public class IntegrationCompany : IntegrationBase
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdCompany { get; set; }
  }
}
