using Manager.Views.BusinessList;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGoalManager: _ViewCrud
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListPersonBase Manager { get; set; }
    public ViewCrudGoalItem GoalsManagerList { get; set; }
  }
}
