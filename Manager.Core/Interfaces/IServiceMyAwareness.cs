using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceMyAwareness
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    string Delete(string id);
    string New(ViewCrudMyAwareness view);
    string Update(ViewCrudMyAwareness view);
    ViewCrudMyAwareness Get(string id);
    ViewCrudMyAwareness GetNow();
    List<ViewListMyAwareness> ListPerson(string idperson, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListMyAwareness> List(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
