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
    void SetLogo(string idCompany, string url);
    string GetLogo(string idCompany);
    string Delete(string id);
    string RemoveEstablishment(string id);
    string New(ViewCrudCompany view);
    string Update(ViewCrudCompany view);
    ViewCrudCompany Get(string id);
    ViewCrudCompany GetByName(string id);
    List<ViewListCompany> List(ref long total, int count = 10, int page = 1, string filter = "");
    string NewEstablishment(ViewCrudEstablishment view);
    string UpdateEstablishment(ViewCrudEstablishment view);
    ViewCrudEstablishment GetEstablishment(string id);
    ViewCrudEstablishment GetEstablishmentByName(string idCompany, string name);
    List<ViewListEstablishment> ListEstablishment(string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListEstablishment> ListEstablishment(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
