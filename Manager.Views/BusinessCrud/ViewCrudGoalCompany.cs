using Manager.Views.BusinessList;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGoalCompany : _ViewCrudBase
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListCompany Company { get; set; }
    public ViewCrudGoalCompanyItem GoalsCompanyList { get; set; }
  }
}
