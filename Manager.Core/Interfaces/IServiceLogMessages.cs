using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceLogMessages
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task<List<ViewListLogMessages>> ListPerson(string idperson,  ref long total, int count = 10, int page = 1, string filter = "");
    Task<List<ViewListLogMessages>> ListManager(string idmanager,  ref long total, int count = 10, int page = 1, string filter = "");
    Task<string> New(ViewCrudLogMessages view);
    Task<ViewCrudLogMessages> Get(string id);
    Task<string> Update(ViewCrudLogMessages view);
    Task<string> Delete(string id);
  }
}
