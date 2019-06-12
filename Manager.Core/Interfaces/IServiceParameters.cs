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
     string Delete(string id);
     string New(ViewCrudParameter view);
     string Update(ViewCrudParameter view);
     ViewCrudParameter Get(string id);
     ViewCrudParameter GetKey(string key);
     List<ViewListParameter> List( ref long total, int count = 10, int page = 1, string filter = "");
  }
}
