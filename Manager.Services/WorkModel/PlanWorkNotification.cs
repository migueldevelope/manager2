using Manager.Core.Base;
using Manager.Core.Business;

namespace Manager.Services.WorkModel
{
  public class PlanWorkNotification
  {
    public BaseFields Manager { get; set; }
    public ManagerListType Type { get; set; }
    public Person Person { get; set; }
    public Plan Plan { get; set; }
  }
}
