using Manager.Core.Base;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  public class GoalsPersonControl : BaseEntity
  {
    public ViewListPerson Person { get; set; }
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public EnumStatusGoalsPerson StatusGoalsPerson { get; set; }
    public DateTime? DateBeginPerson { get; set; }
    public DateTime? DateBeginManager { get; set; }
    public DateTime? DateBeginEnd { get; set; }
    public DateTime? DateEndPerson { get; set; }
    public DateTime? DateEndManager { get; set; }
    public DateTime? DateEndEnd { get; set; }
  }
}
