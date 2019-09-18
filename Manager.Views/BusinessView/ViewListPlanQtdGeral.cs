using System.Collections.Generic;

namespace Manager.Views.BusinessView
{
  public class ViewListPlanQtdGerals
  {
    public List<ViewPlanQtd> List { get; set; }
    public double Schedule { get; set; }
    public double Realized { get; set; }
    public double Late { get; set; }
    public double Balance { get; set; }
  }
}
