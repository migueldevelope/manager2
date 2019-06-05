using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceMeritocracy
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task<string> Delete(string id);
    Task<string> New(string idperson);
    Task<string> Update(ViewCrudMeritocracy view);
    Task<ViewCrudMeritocracy> Get(string id);
    Task<List<ViewListMeritocracy>> List( ref long total, int count = 10, int page = 1, string filter = "");
    Task<string> UpdateOccupationDate(ViewCrudMeritocracyDate view, string id);
    Task<string> UpdateCompanyDate(ViewCrudMeritocracyDate view, string id);
    Task<string> UpdateOccupationActivitiesExcellence(ViewCrudMeritocracyWeight view, string id);
    Task<string> UpdateOccupationMaturity(ViewCrudMeritocracyWeight view, string id);
    Task<string> UpdateActivitieMark(string idmeritocracy, string idactivitie, byte mark);
    Task<List<ViewListMeritocracyActivitie>> ListMeritocracyActivitie(string idmeritocracy);
    Task<List<ViewListMeritocracy>> ListWaitManager(string idmanager,  string filter, int count, int page);
    Task<string> NewMeritocracyScore(ViewCrudMeritocracyScore view);
    Task<string> UpdateMeritocracyScore(ViewCrudMeritocracyScore view);
    Task<ViewCrudMeritocracyScore> GetMeritocracyScore(string id);
    Task<string> RemoveMeritocracyScore(string id);
    Task<ViewListMeritocracyScore> ListMeritocracyScore();
    Task<string> DeleteSalaryScaleScore(string id);
    Task<string> UpdateSalaryScaleScore(ViewCrudSalaryScaleScore view);
    Task<ViewCrudSalaryScaleScore> GetSalaryScaleScore(string id);
    Task<List<ViewCrudSalaryScaleScore>> ListSalaryScaleScore( ref long total, int count = 10, int page = 1, string filter = "");
  }
}
