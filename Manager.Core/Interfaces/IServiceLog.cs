using Manager.Core.Views;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceLog
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void NewLog(ViewLog view);
    List<ViewListLog> GetLogs(string idaccount, ref long total, int count , int page, string filter);
  }
}
