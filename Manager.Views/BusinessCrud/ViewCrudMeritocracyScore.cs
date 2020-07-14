using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMeritocracyScore: _ViewCrud
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
    public byte WeightGoals { get; set; }
    //public EnumMeritocracyGoals WeightGoals { get; set; }
  }
}
