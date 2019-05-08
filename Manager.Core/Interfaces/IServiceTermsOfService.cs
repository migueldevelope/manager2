using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceTermsOfService
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    string Delete(string id);
    string New(ViewCrudTermsOfService view);
    string Update(ViewCrudTermsOfService view);
    ViewCrudTermsOfService Get(string id);
    ViewListTermsOfService GetTerm();
    List<ViewListTermsOfService> List(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
