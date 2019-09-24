using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
namespace Manager.Services.Specific
{
  public static class MongoExtensions
  {
    public static BsonDocument RenderToBsonDocument<T>(this FilterDefinition<T> filter)
    {
      var serializerRegistry = BsonSerializer.SerializerRegistry;
      var documentSerializer = serializerRegistry.GetSerializer<T>();
      return filter.Render(documentSerializer, serializerRegistry);
    }
  }
}