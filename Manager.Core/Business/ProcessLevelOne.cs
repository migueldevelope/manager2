using Manager.Core.Base;
using Manager.Views.BusinessList;
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
    public ViewListArea Area { get; set; }
    public long Order { get; set; }
    [BsonIgnore]
    public List<ViewListProcessLevelTwo> Process { get; set; }
    public ViewListProcessLevelOne GetViewList()
    {
      return new ViewListProcessLevelOne()
      {
        _id = _id,
        Name = Name,
        Area = Area,
        Order = Order
      };
    }
  }
}
