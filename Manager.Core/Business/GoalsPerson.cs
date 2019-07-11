using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  public class GoalsPerson : BaseEntity
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListPersonBase Person { get; set; }
    public GoalsItem GoalsPersonList { get; set; }
  }
}
