using Manager.Core.Business;
using Manager.Core.Views;
using System;
using System.Linq.Expressions;

namespace Manager.Core.Interfaces
{
  public interface IServiceAccount
  {
    void NewAccount(ViewNewAccount view);
    Account GeAccount(Expression<Func<Account, bool>> filter);
  }
}
