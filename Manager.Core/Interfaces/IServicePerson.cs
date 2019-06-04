using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServicePerson
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task<List<ViewListPersonCrud>> List( int count, int page, string filter, EnumTypeUser type);
    Task<ViewCrudPerson> Get(string id);
    Task<ViewCrudPerson> New(ViewCrudPerson view);
    Task<string> Update(ViewCrudPerson person);
    Task<List<ViewListOccupation>> ListOccupation( string filter, int count, int page);
    Task<List<ViewListPerson>> ListManager( string filter, int count, int page);
    Task<List<ViewListCompany>> ListCompany( string filter, int count, int page);
    Task<List<ViewListPerson>> GetPersons(string idcompany, string filter);
    Task<string> AddPersonUser(ViewCrudPersonUser view);
    Task<string> UpdatePersonUser(ViewCrudPersonUser view);
    Task<List<ViewListPersonTeam>> ListTeam( string idPerson, string filter, int count, int page);
    Task<List<ViewListSalaryScalePerson>> ListSalaryScale(string idoccupation);
  }
}
