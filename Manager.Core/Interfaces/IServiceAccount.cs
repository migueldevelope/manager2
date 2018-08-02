using Manager.Core.Business;
using Manager.Core.Views;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Manager.Core.Interfaces
{
  public interface IServiceAccount
  {
    void NewAccount(ViewNewAccount view);
    Account GeAccount(Expression<Func<Account, bool>> filter);
    List<Account> GeAccounts(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
