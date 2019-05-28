using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMeritocracy: _ViewCrud
  {
    public ViewListPersonMeritocracy Person { get; set; }
    public decimal Maturity { get; set; }
    public decimal ActivitiesExcellence { get; set; }
    public EnumStatusMeritocracy StatusMeritocracy { get; set; }
  }
}
