﻿using Manager.Views.BusinessList;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGoalPerson : _ViewCrud
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListPerson Person { get; set; }
    public ViewCrudGoalItem GoalsPersonList { get; set; }
  }
}
