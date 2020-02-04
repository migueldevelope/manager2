using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListTrainingPlan
  {
    public string Person { get; set; }
    public List<ViewListTrainingPlanPerson> Courses { get; set; }
    public decimal PercentRealized { get; set; }
    public decimal PercentNo { get; set; }
  }
}
