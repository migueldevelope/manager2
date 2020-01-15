using Manager.Core.Base;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceNotification
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    void SendMessage();

    List<string> EmployeeWaiting(EnumActionNotification action);
    List<string> ManagerWaiting(EnumActionNotification action);
  }
}
