using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessList;

namespace Manager.Core.Business
{
  public class GoalsPerson : BaseEntity
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListPerson Person { get; set; }
    public GoalsItem GoalsPersonList { get; set; }
  }
}
