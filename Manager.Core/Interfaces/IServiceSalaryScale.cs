using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceSalaryScale
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);

    Task<List<ViewListSalaryScale>> List(string idcompany,  ref long total, int count = 10, int page = 1, string filter = "");
    Task<ViewCrudSalaryScale> Get(string id);
    Task<string> New(ViewCrudSalaryScale view);
    Task<string> Update(ViewCrudSalaryScale view);
    Task<string> Delete(string id);

    Task<List<ViewListGrade>> ListGrade(string idsalaryscale,  ref long total, int count = 10, int page = 1, string filter = "");
    Task<List<ViewListGradeFilter>> ListGrades(string idcompany,  ref long total, int count = 10, int page = 1, string filter = "");
    Task<string> AddGrade(ViewCrudGrade view);
    Task<string> UpdateGrade(ViewCrudGrade view);
    Task<string> DeleteGrade(string idsalaryscale, string id);
    Task<ViewCrudGrade> GetGrade(string idsalaryscale, string id);
    Task<string> UpdateGradePosition(string idsalaryscale, string idgrade, int position);
    Task<string> UpdateStep(ViewCrudStep view);
  }
}
