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

    Task<string> New(ViewCrudGoal view);
    Task<string> Update(ViewCrudGoal view);
    Task<string> Delete(string id);
    Task<ViewCrudGoal> Get(string id);
    Task<List<ViewListGoal>> List( int count = 10, int page = 1, string filter = "");
    Task<List<ViewListGoal>> ListCompany(string id,  int count = 10, int page = 1, string filter = "");
    Task<List<ViewListGoal>> ListManager(string id,  int count = 10, int page = 1, string filter = "");
    Task<string> NewGoalsPeriod(ViewCrudGoalPeriod view);
    Task<string> UpdateGoalsPeriod(ViewCrudGoalPeriod view);
    Task<string> DeleteGoalsPeriod(string id);
    Task<ViewCrudGoalPeriod> GetGoalsPeriod(string id);
    Task<List<ViewCrudGoalPeriod>> ListGoalsPeriod( int count = 10, int page = 1, string filter = "");
    Task<string> NewGoalsCompany(ViewCrudGoalCompany view);
    Task<string> UpdateGoalsCompany(ViewCrudGoalCompany view);
    Task<string> UpdateGoalsCompanyAchievement(ViewCrudAchievement view);
    Task<string> DeleteGoalsCompany(string id);
    Task<ViewCrudGoalCompany> GetGoalsCompany(string id);
    Task<List<ViewCrudGoalItem>> ListGoalsCompany(string idGoalsPeriod, string idCompany,  int count = 10, int page = 1, string filter = "");
    Task<List<ViewListGoalCompany>> ListGoalsCompany( int count = 10, int page = 1, string filter = "");
    Task<string> NewGoalsPersonPortal(ViewCrudGoalPerson view);
    //string NewGoalsPerson(ViewCrudGoalPerson view);
    //string UpdateGoalsPerson(ViewCrudGoalPerson view);
    Task<string> UpdateGoalsPersonPortal(ViewCrudGoalPerson view);
    Task<string> UpdateGoalsPersonAchievement(ViewCrudAchievement view);
    Task<string> DeleteGoalsPerson(string id);
    Task<ViewCrudGoalPerson> GetGoalsPerson(string id);
    Task<ViewListGoalsItem> ListGoalsPerson(string idGoalsPeriod, string idPerson,  int count = 10, int page = 1, string filter = "");
    Task<List<ViewListGoalPerson>> ListGoalsPerson( int count = 10, int page = 1, string filter = "");
    Task<string> NewGoalsManager(ViewCrudGoalManager view);
    Task<string> NewGoalsManagerPortal(ViewCrudGoalManagerPortal view);
    Task<string> UpdateGoalsManager(ViewCrudGoalManager view);
    Task<string> UpdateGoalsManagerPortal(ViewCrudGoalManagerPortal view);
    Task<string> UpdateGoalsManagerAchievement(ViewCrudAchievement view);
    Task<string> DeleteGoalsManager(string id);
    Task<ViewCrudGoalManager> GetGoalsManager(string id);
    Task<ViewListGoalsItem> ListGoalsManager(string idGoalsPeriod, string idManager,  int count = 10, int page = 1, string filter = "");
    Task<List<ViewListGoalManager>> ListGoalsManager( int count = 10, int page = 1, string filter = "");


    Task<string> NewGoalsPersonControl(string idperson, string idperiod);
    Task<string> UpdateGoalsPersonControl(ViewCrudGoalPersonControl view);
    Task<string> DeleteGoalsPersonControl(string idperson, string idperiod);
    Task<ViewCrudGoalPersonControl> GetGoalsPersonControl(string id);
    Task<List<ViewListGoalPersonControl>> ListGoalsPersonControl(string idmanager,  int count, int page, string filter);
    Task<ViewListGoalPersonControl> ListGoalsPersonControlMe(string idperson);
    Task<ViewCrudGoalManagerPortal> GetGoalsManagerPortal(string id);

  }
}
