using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceCompany
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task SetLogo(string idCompany, string url);
    Task<string> GetLogo(string idCompany);
    Task<string> Delete(string id);
    Task<string> RemoveEstablishment(string id);
    Task<string> New(ViewCrudCompany view);
    Task<string> Update(ViewCrudCompany view);
    Task<ViewCrudCompany> Get(string id);
    Task<ViewCrudCompany> GetByName(string id);
    Task<List<ViewListCompany>> List( int count = 10, int page = 1, string filter = "");
    Task<string> NewEstablishment(ViewCrudEstablishment view);
    Task<string> UpdateEstablishment(ViewCrudEstablishment view);
    Task<ViewCrudEstablishment> GetEstablishment(string id);
    Task<ViewCrudEstablishment> GetEstablishmentByName(string idCompany, string name);
    Task<List<ViewListEstablishment>> ListEstablishment(string idcompany,  int count = 10, int page = 1, string filter = "");
    Task<List<ViewListEstablishment>> ListEstablishment( int count = 10, int page = 1, string filter = "");
  }
}
