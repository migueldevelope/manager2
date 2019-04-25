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
    List<ViewCrudGoalCompanyItem> ListGoalsCompany(string idGoalsPeriod, string idCompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListGoalCompany> ListGoalsCompany(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
