using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMeritocracy : _ViewCrud
  {
    public ViewListPersonMeritocracy Person { get; set; }
    public decimal Maturity { get; set; }
    public decimal ActivitiesExcellence { get; set; }
    public EnumStatusMeritocracy StatusMeritocracy { get; set; }

    public byte WeightCompanyDate { get; set; }
    public byte WeightOccupationDate { get; set; }
    public byte WeightSchooling { get; set; }
    public byte WeightMaturity { get; set; }
    public byte WeightActivitiesExcellence { get; set; }
    public EnumMeritocracyGoals WeightGoals { get; set; }

    public bool ValidCompanyDate { get; set; }
    public bool ValidOccupationDate { get; set; }
    public bool ValidSchooling { get; set; }
    public bool ValidActivitiesExcellence { get; set; }

    public bool EnabledCompanyDate { get; set; }
    public bool EnabledOccupationDate { get; set; }
    public bool EnabledSchooling { get; set; }
    public bool EnabledMaturity { get; set; }
    public bool EnabledActivitiesExcellence { get; set; }
    public bool EnabledGoals { get; set; }

    public decimal ResultEnd { get; set; }

    public EnumSteps ResultStep { get; set; }
    public ViewListGrade Grade { get; set; }
  }
}
