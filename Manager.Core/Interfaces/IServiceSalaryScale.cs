using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceSalaryScale
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);

    List<ViewListSalaryScale> List(string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    ViewCrudSalaryScale Get(string id);
    string New(ViewCrudSalaryScale view);
    string Update(ViewCrudSalaryScale view);
    string Delete(string id);

    List<ViewListGrade> ListGrade(string idsalaryscale, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListGradeFilter> ListGrades(string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    string AddGrade(ViewCrudGrade view);
    string UpdateGrade(ViewCrudGrade view);
    string DeleteGrade(string idsalaryscale, string id);
    ViewCrudGrade GetGrade(string idsalaryscale, string id);
    string UpdateGradePosition(string idsalaryscale, string idgrade, int position);
    string UpdateStep(ViewCrudStep view);
  }
}
