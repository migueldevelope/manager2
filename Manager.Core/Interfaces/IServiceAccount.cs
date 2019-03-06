using Manager.Core.Business;
using Manager.Core.Views;
using Manager.Views.BusinessList;
using Manager.Views.BusinessNew;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceAccount
  {
    Task<string> NewAccount(ViewNewAccount view);
    List<ViewListAccount> GetAll(ref long total, int count = 10, int page = 1, string filter = "");

    Account GeAccount(Expression<Func<Account, bool>> filter);
    ViewPerson AlterAccount(string idaccount, string link);
    ViewPerson AlterAccountPerson(string idperson, string link);
  }
}
