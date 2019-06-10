using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceMaturity
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task<string> Delete(string id);
    Task<string> New(ViewCrudMaturity view);
    Task<string> Update(ViewCrudMaturity view);
    Task<ViewCrudMaturity> Get(string id);
    Task<List<ViewCrudMaturity>> List( ref long total, int count = 10, int page = 1, string filter = "");
    Task<string> NewMaturityRegister(ViewCrudMaturityRegister view);
    Task<string> UpdateMaturityRegister(ViewCrudMaturityRegister view);
    Task<ViewCrudMaturityRegister> GetMaturityRegister(string id);
    Task<string> DeleteMaturityRegister(string id);
    Task<List<ViewCrudMaturityRegister>> ListMaturityRegister( ref long total, int count = 10, int page = 1, string filter = "");
    Task MathMonth();
  }
}
