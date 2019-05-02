using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGoalPersonPortal: _ViewCrud
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListPerson Person { get; set; }
    public ViewCrudGoalItemPortal GoalsPersonList { get; set; }
    public EnumStatusGoalsPerson StatusGoalsPerson { get; set; }
  }
}
