using Manager.Core.Base;
using Manager.Views.BusinessCrud;
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
    public ViewListCompany Company { get; set; }
    public long Order { get; set; }
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Template { get; set; }
    [BsonIgnore]
    public List<ViewListProcessLevelOne> ProcessLevelOnes { get; set; }
    public ViewListArea GetViewList()
    {
      return new ViewListArea()
      {
        _id = _id,
        Name = Name
      };
    }
    public ViewCrudArea GetViewCrud()
    {
      return new ViewCrudArea()
      {
        _id = _id,
        Name = Name,
        Company = Company,
        Order = Order
      };
    }
  }
}
