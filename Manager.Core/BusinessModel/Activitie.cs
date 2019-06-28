using Manager.Core.Base;
using Manager.Views.BusinessList;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção utilizada paras entregas
  /// </summary>
  public class Activitie : BaseEntity
  {
    public string Name { get; set; }
    public long Order { get; set; }
    public ViewListActivitie GetViewList()
    {
      return new ViewListActivitie()
      {
        _id = _id,
        Name = Name,
        Order = Order
      };
    }
  }
}
