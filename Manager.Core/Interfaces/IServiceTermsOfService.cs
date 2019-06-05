using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceTermsOfService
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task<string> Delete(string id);
    Task<string> New(ViewCrudTermsOfService view);
    Task<string> Update(ViewCrudTermsOfService view);
    Task<ViewCrudTermsOfService> Get(string id);
    Task<ViewListTermsOfService> GetTerm();
    Task<List<ViewListTermsOfService>> List( ref long total, int count = 10, int page = 1, string filter = "");
  }
}
