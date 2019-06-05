using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceDictionarySystem
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task<string> New(ViewCrudDictionarySystem view);
    Task<string> New(List<ViewListDictionarySystem> list);
    Task<string> Update(ViewCrudDictionarySystem view);
    Task<string> Delete(string id);
    Task<ViewCrudDictionarySystem> Get(string id);
    Task<ViewListDictionarySystem> GetName(string name);
    Task<List<ViewListDictionarySystem>> List( ref long total, int count = 10, int page = 1, string filter = "");
  }
}
