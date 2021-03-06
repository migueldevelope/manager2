﻿using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGoalPeriod : _ViewCrudBase
  {
    public DateTime? DateBegin { get; set; }
    public DateTime? DateEnd { get; set; }
    public bool Review { get; set; }
    public bool ChangeCheck { get; set; }
    public decimal PercentCompany { get; set; }
    public decimal PercentTeam { get; set; }
    public decimal PercentPerson { get; set; }
  }
}
