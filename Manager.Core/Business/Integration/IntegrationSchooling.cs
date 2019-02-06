using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business.Integration
{
  public class IntegrationSchooling : IntegrationBase 
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdSchooling { get; set; }
    public string NameSchooling { get; set; }
  }
}
