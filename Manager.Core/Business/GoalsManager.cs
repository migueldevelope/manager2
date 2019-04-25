using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessList;

namespace Manager.Core.Business
{
  public class GoalsManager : BaseEntity
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListPerson Manager { get; set; }
    public GoalsItem GoalsManagerList { get; set; }
  }
}
