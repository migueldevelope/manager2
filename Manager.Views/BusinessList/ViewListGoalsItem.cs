using Manager.Views.BusinessCrud;
using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListGoalsItem
  {
    public List<ViewCrudGoalItem> GoalsCompany { get; set; }
    public List<ViewCrudGoalItem> GoalsManager { get; set; }
    public List<ViewCrudGoalItem> GoalsPerson { get; set; }
  }
}
