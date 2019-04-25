using Manager.Views.BusinessList;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGoalCompany : _ViewCrud
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListCompany Company { get; set; }
    public ViewCrudGoalItem GoalsCompanyList { get; set; }
  }
}
