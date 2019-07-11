using Manager.Views.BusinessList;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGoalManagerPortal: _ViewCrud
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListPersonBase Manager { get; set; }
    public ViewCrudGoalItemPortal GoalsManagerList { get; set; }
  }
}
