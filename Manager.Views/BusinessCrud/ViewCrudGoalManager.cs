using Manager.Views.BusinessList;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGoalManager: _ViewCrudBase
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListPerson Manager { get; set; }
    public ViewCrudGoalItem GoalsManagerList { get; set; }
  }
}
