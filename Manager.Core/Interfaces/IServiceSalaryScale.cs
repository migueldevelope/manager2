using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceSalaryScale
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    string UpdateStep(string idestablishment, string idgrade, EnumSteps step, decimal salary);
    string Remove(string id);
    SalaryScale Get(string id);
    List<SalaryScale> List(string idcompany, string idestablishment, ref long total, int count = 10, int page = 1, string filter = "");
    string NewGrade(Grade view);
    string UpdateGrade(Grade view);
    string RemoveGrade(string id);
    Grade GetGrade(string id);
    List<Grade> ListGrade(string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
  }
}
