using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceGoals
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);

     string New(ViewCrudGoal view);
     string Update(ViewCrudGoal view);
     string Delete(string id);
     ViewCrudGoal Get(string id);
     List<ViewListGoal> List( ref long total, int count = 10, int page = 1, string filter = "");
     List<ViewListGoal> ListCompany(string id,  ref long total, int count = 10, int page = 1, string filter = "");
     List<ViewListGoal> ListManager(string id,  ref long total, int count = 10, int page = 1, string filter = "");
     string NewGoalsPeriod(ViewCrudGoalPeriod view);
     string UpdateGoalsPeriod(ViewCrudGoalPeriod view);
     string DeleteGoalsPeriod(string id);
     ViewCrudGoalPeriod GetGoalsPeriod(string id);
     List<ViewCrudGoalPeriod> ListGoalsPeriod( ref long total, int count = 10, int page = 1, string filter = "");
     string NewGoalsCompany(ViewCrudGoalCompany view);
     string UpdateGoalsCompany(ViewCrudGoalCompany view);
     string UpdateGoalsCompanyAchievement(ViewCrudAchievement view);
     string DeleteGoalsCompany(string id);
     ViewCrudGoalCompany GetGoalsCompany(string id);
     List<ViewCrudGoalItem> ListGoalsCompany(string idGoalsPeriod, string idCompany,  ref long total, int count = 10, int page = 1, string filter = "");
     List<ViewListGoalCompany> ListGoalsCompany( ref long total, int count = 10, int page = 1, string filter = "");
     string NewGoalsPersonPortal(ViewCrudGoalPerson view);
    //string NewGoalsPerson(ViewCrudGoalPerson view);
    //string UpdateGoalsPerson(ViewCrudGoalPerson view);
     string UpdateGoalsPersonPortal(ViewCrudGoalPerson view);
     string UpdateGoalsPersonAchievement(ViewCrudAchievement view);
     string DeleteGoalsPerson(string id);
     ViewCrudGoalPerson GetGoalsPerson(string id);
     ViewListGoalsItem ListGoalsPerson(string idGoalsPeriod, string idPerson,  ref long total, int count = 10, int page = 1, string filter = "");
     List<ViewListGoalPerson> ListGoalsPerson( ref long total, int count = 10, int page = 1, string filter = "");
     string NewGoalsManager(ViewCrudGoalManager view);
     string NewGoalsManagerPortal(ViewCrudGoalManagerPortal view);
     string UpdateGoalsManager(ViewCrudGoalManager view);
     string UpdateGoalsManagerPortal(ViewCrudGoalManagerPortal view);
     string UpdateGoalsManagerAchievement(ViewCrudAchievement view);
     string DeleteGoalsManager(string id);
     ViewCrudGoalManager GetGoalsManager(string id);
     ViewListGoalsItem ListGoalsManager(string idGoalsPeriod, string idManager,  ref long total, int count = 10, int page = 1, string filter = "");
     List<ViewListGoalManager> ListGoalsManager( ref long total, int count = 10, int page = 1, string filter = "");


     string NewGoalsPersonControl(string idperson, string idperiod);
     string UpdateGoalsPersonControl(ViewCrudGoalPersonControl view);
     string DeleteGoalsPersonControl(string idperson, string idperiod);
     ViewCrudGoalPersonControl GetGoalsPersonControl(string id);
     List<ViewListGoalPersonControl> ListGoalsPersonControl(string idmanager, ref long total, int count, int page, string filter);
     ViewListGoalPersonControl ListGoalsPersonControlMe(string idperson);
     ViewCrudGoalManagerPortal GetGoalsManagerPortal(string id);

  }
}
