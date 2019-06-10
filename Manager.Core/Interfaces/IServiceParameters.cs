using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceParameters
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task<string> Delete(string id);
    Task<string> New(ViewCrudParameter view);
    Task<string> Update(ViewCrudParameter view);
    Task<ViewCrudParameter> Get(string id);
    Task<ViewCrudParameter> GetKey(string key);
    Task<List<ViewListParameter>> List( ref long total, int count = 10, int page = 1, string filter = "");
  }
}
