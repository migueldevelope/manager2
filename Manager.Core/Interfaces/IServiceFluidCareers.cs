using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
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
        ViewCrudFluidCareers New(ViewCrudFluidCareers view);
        ViewCrudFluidCareers Update(ViewCrudFluidCareers view);
        ViewCrudFluidCareers Get(string id);
        List<ViewListFluidCareers> List(ref long total, int count = 10, int page = 1, string filter = "");
        ViewFluidCareerPerson Calc(string idperson, List<ViewCrudSkillsCareers> skills, EnumFilterCalcFluidCareers filterCalcFluidCareers);
        List<ViewCrudSkillsCareers> GetSkills(byte type, ref long total, string filter, int count, int page);
        ViewFluidCareersPerson GetPerson(string idperson);
        List<ViewListFluidCareers> ListPerson(string idperson, ref long total, int count = 10, int page = 1, string filter = "");
        string DeletePlan(string idfluidcareer);
        ViewCrudFluidCareerPlan NewPlan(string idfluidcareer, ViewCrudFluidCareerPlan view);
        ViewCrudFluidCareerPlan UpdatePlan(string idfluidcareer, ViewCrudFluidCareerPlan view);
        ViewCrudFluidCareerPlan GetPlan(string idfluidcareer);
        List<ViewListSkill> GetSkillsPlan(string idoccupation);
    }
}
