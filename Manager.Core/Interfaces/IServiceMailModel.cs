using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceMailModel
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);

    Task<List<ViewListMailModel>> List( ref long total, int count = 10, int page = 1, string filter = "");
    Task<string> New(ViewCrudMailModel view);
    Task<ViewCrudMailModel> Get(string id);
    Task<string> Update(ViewCrudMailModel view);
  }
}
