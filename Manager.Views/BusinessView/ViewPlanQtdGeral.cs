using System.Collections.Generic;

namespace Manager.Views.BusinessView
{
  public class ViewListPlanQtdGeral
  {
    public List<ViewPlanQtd> List { get; set; }
    public decimal Schedule { get; set; }
    public decimal Realized { get; set; }
    public decimal Late { get; set; }
    public decimal Balance { get; set; }
  }
}
