using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  class ViewCrudGoalPersonPortal: _ViewCrud
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListPerson Person { get; set; }
    public ViewCrudGoalItemPortal GoalsPersonList { get; set; }
  }
}
