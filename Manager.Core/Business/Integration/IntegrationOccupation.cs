using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business.Integration
{
  public class IntegrationOccupation : IntegrationBase
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdOccupation { get; set; }
  }
}
