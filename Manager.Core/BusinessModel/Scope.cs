using Manager.Core.Base;
using Manager.Views.BusinessList;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  ///  Coleção para grupo de cargo
  /// </summary>
  public class Scope : BaseEntityId
  {
    public string Name { get; set; }
    public long Order { get; set; }
    public ViewListScope GetViewList()
    {
      return new ViewListScope()
      {
        _id = _id,
        Name = Name,
        Order = Order
      };
    }
  }
}
