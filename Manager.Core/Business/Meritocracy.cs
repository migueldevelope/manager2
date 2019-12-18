using Manager.Core.Base;
using Manager.Core.BusinessModel;
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
    public decimal WeightActivitiesExcellence { get; set; }
    public EnumMeritocracyGoals WeightGoals { get; set; }


    public decimal PercentCompanyDate { get; set; }
    public decimal PercentOccupationDate { get; set; }
    public decimal PercentSchooling { get; set; }
    public decimal PercentMaturity { get; set; }
    public decimal PercentActivitiesExcellence { get; set; }
    public decimal PercentGoals { get; set; }
    public decimal SalaryNew { get; set; }
    public decimal SalaryDifference { get; set; }
    public decimal PercentSalary { get; set; }

    public decimal ResultEnd { get; set; }
    public EnumSteps ResultStep { get; set; }
    public EnumSteps ResultStepScale { get; set; }
    public Grade Grade { get; set; }
    public Grade GradeScale { get; set; }
    public bool ShowPerson { get; set; }
    public MeritocracyScore Score { get; set; }

  }
}
