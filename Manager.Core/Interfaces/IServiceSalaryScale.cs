using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Views;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceSalaryScale
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    string UpdateStep(string idsalaryscale, string idgrade, EnumSteps step, decimal salary);
    string Remove(string id);
    SalaryScale Get(string id);
    List<SalaryScale> List(string idcompany,  ref long total, int count = 10, int page = 1, string filter = "");
    string AddGrade(Grade view, string idsalaryscale);
    string UpdateGrade(Grade view, string idsalaryscale);
    string NewSalaryScale(ViewNewSalaryScale view);
    string UpdateSalaryScale(ViewUpdateSalaryScale view);
    string RemoveGrade(string id, string idsalaryscale);
    Grade GetGrade(string id);
    List<Grade> ListGrade(string idsalaryscale, ref long total, int count = 10, int page = 1, string filter = "");
  }
}
