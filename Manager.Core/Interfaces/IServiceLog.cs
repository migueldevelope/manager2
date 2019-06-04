using Manager.Core.Base;
using Manager.Core.Views;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceLog
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task NewLog(ViewLog view);
    Task<List<ViewListLog>> ListLogs(string idaccount,  int count , int page, string filter);
  }
}
