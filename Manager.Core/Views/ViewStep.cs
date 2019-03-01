using Manager.Views.Enumns;

namespace Manager.Core.Views
{
  public class ViewStep
  {
    public string idgrade { get; set; }
    public string idestablishment { get; set; }
    public EnumSteps Step { get; set; }
    public decimal Salary { get; set; }
  }
}
