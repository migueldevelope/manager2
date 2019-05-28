﻿using Manager.Core.Base;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Meritocracy : BaseEntity
  {
    public ViewListPersonMeritocracy Person { get; set; }
    public decimal Maturity { get; set; }
    public decimal ActivitiesExcellence { get; set; }
    public EnumStatusMeritocracy StatusMeritocracy { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DateEnd { get; set; }
    public List<MeritocracyActivities> MeritocracyActivities { get; set; }

    public byte WeightCompanyDate { get; set; }
    public byte WeightOccupationDate { get; set; }
    public byte WeightSchooling { get; set; }
    public byte WeightMaturity { get; set; }
    public byte WeightActivitiesExcellence { get; set; }
    public EnumMeritocracyGoals WeightGoals { get; set; }

  }
}
