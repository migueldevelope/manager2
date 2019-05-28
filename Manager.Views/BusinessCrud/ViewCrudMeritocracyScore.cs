using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMeritocracyScore: _ViewCrud
  {
    public EnumStatus EnabledCompanyDate { get; set; }
    public EnumStatus EnabledOccupationDate { get; set; }
    public EnumStatus EnabledSchooling { get; set; }
    public EnumStatus EnabledMaturity { get; set; }
    public EnumStatus EnabledActivitiesExcellence { get; set; }
    public EnumStatus EnabledGoals { get; set; }

    public decimal WeightCompanyDate { get; set; }
    public decimal WeightOccupationDate { get; set; }
    public decimal WeightSchooling { get; set; }
    public decimal WeightMaturity { get; set; }
    public decimal WeightActivitiesExcellence { get; set; }
    public decimal WeightGoals { get; set; }
  }
}
