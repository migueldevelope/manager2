using Manager.Core.Base;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  public class MeritocracyScore : BaseEntity
  {
    public bool EnabledCompanyDate { get; set; }
    public bool EnabledOccupationDate { get; set; }
    public bool EnabledSchooling { get; set; }
    public bool EnabledMaturity { get; set; }
    public bool EnabledActivitiesExcellence { get; set; }
    public bool EnabledGoals { get; set; }

    public byte WeightCompanyDate { get; set; }
    public byte WeightOccupationDate { get; set; }
    public byte WeightSchooling { get; set; }
    public byte WeightMaturity { get; set; }
    public byte WeightActivitiesExcellence { get; set; }
    public EnumMeritocracyGoals WeightGoals { get; set; }

    public decimal PercentCompanyDate { get; set; }
    public decimal PercentOccupationDate { get; set; }
    public decimal PercentSchooling { get; set; }
    public decimal PercentMaturity { get; set; }
    public decimal PercentActivitiesExcellence { get; set; }
    public decimal PercentGoals { get; set; }

  }
}
