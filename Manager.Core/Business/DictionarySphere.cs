using Manager.Core.Base;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados, porém não foi utilizado
  /// </summary>
  public class DictionarySphere : BaseEntity
  {
    public string Name { get; set; }
    public EnumTypeSphere Type { get; set; }
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Template { get; set; }
  }
}
