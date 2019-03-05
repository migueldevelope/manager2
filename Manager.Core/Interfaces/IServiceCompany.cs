using Manager.Core.Base;
using Manager.Core.Business;
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
    string New(Company view);
    string Update(Company view);
    string Remove(string id);
    Company Get(string id);
    Company GetByName(string id);
    List<Company> List(ref long total, int count = 10, int page = 1, string filter = "");

    string NewEstablishment(Establishment view);
    string UpdateEstablishment(Establishment view);
    string RemoveEstablishment(string id);
    Establishment GetEstablishment(string id);
    Establishment GetEstablishmentByName(string idCompany, string name);
    List<Establishment> ListEstablishment(string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<Establishment> ListEstablishment(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
