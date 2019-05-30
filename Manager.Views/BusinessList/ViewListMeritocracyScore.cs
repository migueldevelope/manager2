
using Manager.Views.Enumns;

namespace Manager.Views.BusinessList
{
  public class ViewListMeritocracyScore: _ViewList
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
  }
}
