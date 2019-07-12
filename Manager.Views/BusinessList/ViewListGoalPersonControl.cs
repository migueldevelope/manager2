﻿using Manager.Views.Enumns;

namespace Manager.Views.BusinessList
{
  public class ViewListGoalPersonControl: _ViewList
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListPersonBase Person { get; set; }
    public EnumStatusGoalsPerson StatusGoalsPerson { get; set; }
  }
}
