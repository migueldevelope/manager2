
using Manager.Core.Business;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceManager
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void UpdateStructManager();
    List<ViewListStructManager> GetHierarchy(string idmanager);
  }
}
