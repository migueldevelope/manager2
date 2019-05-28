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
    string New(ViewCrudMeritocracy view);
    string Update(ViewCrudMeritocracy view);
    ViewCrudMeritocracy Get(string id);
    List<ViewListMeritocracy> List(ref long total, int count = 10, int page = 1, string filter = "");
    string UpdateOccupationDate(ViewCrudMeritocracyDate view, string id);
    string UpdateCompanyDate(ViewCrudMeritocracyDate view, string id);
    string UpdateOccupationActivitiesExcellence(ViewCrudMeritocracyWeight view, string id);
    string UpdateOccupationMaturity(ViewCrudMeritocracyWeight view, string id);

    string NewMeritocracyScore(ViewCrudMeritocracyScore view);
    string UpdateMeritocracyScore(ViewCrudMeritocracyScore view);
    ViewCrudMeritocracyScore GetMeritocracyScore(string id);
    string RemoveMeritocracyScore(string id);
    List<ViewListMeritocracyScore> ListMeritocracyScore(ref long total, int count = 10, int page = 1, string filter = "");
    string DeleteSalaryScaleScore(string id);
    string NewSalaryScaleScore(ViewCrudSalaryScaleScore view);
    string UpdateSalaryScaleScore(ViewCrudSalaryScaleScore view);
    ViewCrudSalaryScaleScore GetSalaryScaleScore(string id);
    List<ViewCrudSalaryScaleScore> ListSalaryScaleScore(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
