using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceGoals
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    string New(Goals view);
    string Update(Goals view);
    string Remove(string id);
    Goals Get(string id);
    List<Goals> List(ref long total, int count = 10, int page = 1, string filter = "");

    string NewGoalsPeriod(GoalsPeriod view);
    string UpdateGoalsPeriod(GoalsPeriod view);
    string RemoveGoalsPeriod(string id);
    GoalsPeriod GetGoalsPeriod(string id);
    List<GoalsPeriod> ListGoalsPeriod(ref long total, int count = 10, int page = 1, string filter = "");

    string NewGoalsCompany(GoalsCompany view);
    string UpdateGoalsCompany(GoalsCompany view);
    string RemoveGoalsCompany(string id);
    GoalsCompany GetGoalsCompany(string id);
    List<GoalsCompany> ListGoalsCompany(string idgoalsperiod, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<GoalsCompany> ListGoalsCompany(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
