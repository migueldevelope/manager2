using Manager.Core.Base;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados e coleção 
  /// </summary>
  public class ProcessLevelOne : BaseEntity
  {
    public string Name { get; set; }
    public Area Area { get; set; }
    public long Order { get; set; }
    [BsonIgnore]
    public List<ProcessLevelTwo> Process { get; set; }
  }
}
