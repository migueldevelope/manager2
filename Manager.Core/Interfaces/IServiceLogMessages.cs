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
     List<ViewListLogMessages> ListPerson(string idperson,  ref long total, int count = 10, int page = 1, string filter = "");
     List<ViewListLogMessages> ListManager(string idmanager,  ref long total, int count = 10, int page = 1, string filter = "");
     string New(ViewCrudLogMessages view);
     ViewCrudLogMessages Get(string id);
     string Update(ViewCrudLogMessages view);
     string Delete(string id);
  }
}
