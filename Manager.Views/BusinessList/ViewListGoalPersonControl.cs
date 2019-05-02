using Manager.Views.Enumns;

namespace Manager.Views.BusinessList
{
  public class ViewListGoalPersonControl: _ViewList
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListPerson Person { get; set; }
    public EnumStatusGoalsPerson StatusGoalsPerson { get; set; }
  }
}
