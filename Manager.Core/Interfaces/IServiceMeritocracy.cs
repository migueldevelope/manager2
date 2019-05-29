using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceMeritocracy
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    string Delete(string id);
    string New(string idperson);
    string Update(ViewCrudMeritocracy view);
    ViewCrudMeritocracy Get(string id);
    List<ViewListMeritocracy> List(ref long total, int count = 10, int page = 1, string filter = "");
    string UpdateOccupationDate(ViewCrudMeritocracyDate view, string id);
    string UpdateCompanyDate(ViewCrudMeritocracyDate view, string id);
    string UpdateOccupationActivitiesExcellence(ViewCrudMeritocracyWeight view, string id);
    string UpdateOccupationMaturity(ViewCrudMeritocracyWeight view, string id);
    string UpdateActivitieMark(string idmeritocracy, string idactivitie, byte mark);
    List<ViewListMeritocracyActivitie> ListMeritocracyActivitie(string idmeritocracy);
    List<ViewListMeritocracy> ListWaitManager(string idmanager, ref long total, string filter, int count, int page);
    string NewMeritocracyScore(ViewCrudMeritocracyScore view);
    string UpdateMeritocracyScore(ViewCrudMeritocracyScore view);
    ViewCrudMeritocracyScore GetMeritocracyScore(string id);
    string RemoveMeritocracyScore(string id);
    ViewListMeritocracyScore ListMeritocracyScore();
    string DeleteSalaryScaleScore(string id);
    string UpdateSalaryScaleScore(ViewCrudSalaryScaleScore view);
    ViewCrudSalaryScaleScore GetSalaryScaleScore(string id);
    List<ViewCrudSalaryScaleScore> ListSalaryScaleScore(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
