
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

    public decimal WeightCompanyDate { get; set; }
    public decimal WeightOccupationDate { get; set; }
    public decimal WeightSchooling { get; set; }
    public decimal WeightMaturity { get; set; }
    public decimal WeightActivitiesExcellence { get; set; }
    public decimal WeightGoals { get; set; }
  }
}
