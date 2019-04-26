using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

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
    List<ViewListGoal> List(ref long total, int count = 10, int page = 1, string filter = "");

    string NewGoalsPeriod(ViewCrudGoalPeriod view);
    string UpdateGoalsPeriod(ViewCrudGoalPeriod view);
    string DeleteGoalsPeriod(string id);
    ViewCrudGoalPeriod GetGoalsPeriod(string id);
    List<ViewCrudGoalPeriod> ListGoalsPeriod(ref long total, int count = 10, int page = 1, string filter = "");

    string NewGoalsCompany(ViewCrudGoalCompany view);
    string UpdateGoalsCompany(ViewCrudGoalCompany view);
    string DeleteGoalsCompany(string id);
    ViewCrudGoalCompany GetGoalsCompany(string id);
    List<ViewCrudGoalItem> ListGoalsCompany(string idGoalsPeriod, string idCompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListGoalCompany> ListGoalsCompany(ref long total, int count = 10, int page = 1, string filter = "");

    string NewGoalsPerson(ViewCrudGoalPerson view);
    string UpdateGoalsPerson(ViewCrudGoalPerson view);
    string DeleteGoalsPerson(string id);
    ViewCrudGoalPerson GetGoalsPerson(string id);
    ViewListGoalsItem ListGoalsPerson(string idGoalsPeriod, string idPerson, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListGoalPerson> ListGoalsPerson(ref long total, int count = 10, int page = 1, string filter = "");

    string NewGoalsManager(ViewCrudGoalManager view);
    string UpdateGoalsManager(ViewCrudGoalManager view);
    string DeleteGoalsManager(string id);
    ViewCrudGoalManager GetGoalsManager(string id);
    ViewListGoalsItem ListGoalsManager(string idGoalsPeriod, string idManager, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListGoalManager> ListGoalsManager(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
