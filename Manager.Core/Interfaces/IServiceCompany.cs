using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceCompany
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    void SetLogo(string idCompany, string url);
    string GetLogo(string idCompany);
    string Remove(string id);
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


    #region Old
    string NewOld(Company view);
    string UpdateOld(Company view);
    Company GetOld(string id);
    Company GetByNameOld(string id);
    List<Company> ListOld(ref long total, int count = 10, int page = 1, string filter = "");
    string NewEstablishmentOld(Establishment view);
    string UpdateEstablishmentOld(Establishment view);
    Establishment GetEstablishmentOld(string id);
    Establishment GetEstablishmentByNameOld(string idCompany, string name);
    List<Establishment> ListEstablishmentOld(string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<Establishment> ListEstablishmentOld(ref long total, int count = 10, int page = 1, string filter = "");

    #endregion

  }
}
