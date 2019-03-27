using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceSalaryScale
  {
    void SetUser(IHttpContextAccessor contextAccessor);

    List<ViewListSalaryScale> List(string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    ViewCrudSalaryScale Get(string id);
    string NewSalaryScale(ViewCrudSalaryScale view);
    string UpdateSalaryScale(ViewCrudSalaryScale view);
    string Remove(string id);
    List<ViewListGrade> ListGrade(string idsalaryscale, ref long total, int count = 10, int page = 1, string filter = "");
    string AddGrade(ViewCrudGrade view);
    string UpdateGrade(ViewCrudGrade view);
    string RemoveGrade(string idsalaryscale, string id);
    ViewCrudGrade GetGrade(string idsalaryscale, string id);
    string UpdateStep(ViewCrudStep view);
  }
}
