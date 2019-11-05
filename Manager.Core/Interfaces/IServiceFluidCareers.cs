using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceFluidCareers
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    string Delete(string id);
    string New(ViewCrudFluidCareers view);
    string Update(ViewCrudFluidCareers view);
    ViewCrudFluidCareers Get(string id);
    List<ViewListFluidCareers> List(ref long total, int count = 10, int page = 1, string filter = "");
    ViewFluidCareers Calc(string idperson, List<ViewCrudSkillsCareers> skills);
    List<ViewCrudSkillsCareers> GetSkills(ref long total, string filter, int count, int page);
    ViewFluidCareersPerson GetPerson(string idperson);
  }
}
