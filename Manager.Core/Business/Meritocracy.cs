using Manager.Core.Base;
using Manager.Views.BusinessList;

namespace Manager.Core.Business
{
  public class Meritocracy : BaseEntity
  {

    public ViewListPersonMeritocracy Person { get; set; }
    public decimal Maturity { get; set; }
    public decimal ActivitiesExcellence { get; set; }
  }
}
