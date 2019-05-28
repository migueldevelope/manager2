using Manager.Core.Base;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessNew;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceAccount
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser _user);
    Task<string> NewAccount(ViewNewAccount view);
    Task<List<ViewListAccount>> GetAll(ref long total, int count = 10, int page = 1, string filter = "");
    Task<ViewPerson> AlterAccount(string idaccount);
    Task<ViewPerson> AlterAccountPerson(string idperson);
    Task<string> SynchronizeParameters();
    Task<string> UpdateAccount(ViewCrudAccount view, string id);
    Task<ViewCrudAccount> GetAccount(string id);
  }
}
