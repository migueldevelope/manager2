using Manager.Core.Base;
using Manager.Core.Views;
using Manager.Views.BusinessList;
using Manager.Views.BusinessNew;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceAccount
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser _user);
    string NewAccount(ViewNewAccount view);
    List<ViewListAccount> GetAll(ref long total, int count = 10, int page = 1, string filter = "");
    ViewPerson AlterAccount(string idaccount);
    ViewPerson AlterAccountPerson(string idperson);
    string SynchronizeParameters();
  }
}
