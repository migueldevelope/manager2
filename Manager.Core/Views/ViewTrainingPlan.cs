using Manager.Core.Business;
using Manager.Core.Enumns;

namespace Manager.Core.Views
{
  public class ViewTrainingPlan
  {
    public string Person { get; set; }
    public string Course { get; set; }
    public EnumStatusTrainingPlan StatusTrainingPlan { get; set; }
    public EnumOrigin Origin { get; set; }
    public decimal PercentRealized { get; set; }
    public decimal PercentNo { get; set; }
  }
}
