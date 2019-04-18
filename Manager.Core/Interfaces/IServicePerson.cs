using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServicePerson
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    List<ViewListPersonCrud> List(ref long total, int count, int page, string filter, EnumTypeUser type);
    ViewCrudPerson Get(string id);
    ViewCrudPerson New(ViewCrudPerson view);
    string Update(ViewCrudPerson person);

    List<ViewListOccupation> ListOccupation(ref long total, string filter, int count, int page);
    List<ViewListPerson> ListManager(ref long total, string filter, int count, int page);
    List<ViewListCompany> ListCompany(ref long total, string filter, int count, int page);

    List<ViewListPerson> GetPersons(string idcompany, string filter);

    string AddPersonUser(ViewCrudPersonUser view);
    string UpdatePersonUser(ViewCrudPersonUser view);
    List<ViewListPersonTeam> ListTeam(ref long total, string idPerson, string filter, int count, int page);
    List<ViewListSalaryScalePerson> ListSalaryScale(string idoccupation);

  }
}
