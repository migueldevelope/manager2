using Manager.Core.Base;
using Manager.Views.BusinessList;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class Area : BaseEntity
  {
    public string Name { get; set; }
    public Company Company { get; set; }
    public long Order { get; set; }
    public Area Template { get; set; }
    [BsonIgnore]
    public List<ProcessLevelOne> ProcessLevelOnes{get;set;}
    public ViewListArea GetViewList()
    {
      return new ViewListArea()
      {
        _id = _id,
        Name = Name
      };
    }
  }
}
