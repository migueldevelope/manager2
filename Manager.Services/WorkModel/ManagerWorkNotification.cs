using Manager.Core.Base;
using Manager.Core.Business;

namespace Manager.Services.WorkModel
{
  public class ManagerWorkNotification
  {
    public BaseFields Manager { get; set; }
    public ManagerListType Type { get; set; }
    public Person Person { get; set; }
  }
}
