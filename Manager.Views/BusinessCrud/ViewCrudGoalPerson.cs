using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGoalPerson : _ViewCrud
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListPersonBase Person { get; set; }
    public ViewCrudGoalItem GoalsPersonList { get; set; }
  }
}
