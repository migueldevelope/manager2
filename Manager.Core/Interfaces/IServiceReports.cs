using Manager.Core.Base;
using Manager.Core.Views;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceReports
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    void RegisterOnMessageHandlerAndReceiveMesssages();
    string ListPersons();
  }
}
