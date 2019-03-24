using Manager.Core.Business;
using Manager.Core.Views;
using Manager.Views.BusinessList;
using Manager.Views.BusinessNew;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceAccount
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    Task<string> NewAccount(ViewNewAccount view);
    List<ViewListAccount> GetAll(ref long total, int count = 10, int page = 1, string filter = "");
    ViewPerson AlterAccount(string idaccount);
    ViewPerson AlterAccountPerson(string idperson);
    Task<string> SynchronizeParameters();
    Account GeAccount(Expression<Func<Account, bool>> filter);
  }
}
