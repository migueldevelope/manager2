﻿using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGoalPersonControl: _ViewCrud
  {
    public ViewListPersonBaseManager Person { get; set; }
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public EnumStatusGoalsPerson StatusGoalsPerson { get; set; }
  }
}
