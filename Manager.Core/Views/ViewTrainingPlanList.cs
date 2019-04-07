using Manager.Core.Business;
using System;
using System.Collections.Generic;

namespace Manager.Core.Views
{
  public class ViewTrainingPlanList
  {
    public string Person { get; set; }
    public List<ViewTrainingPlan> TraningPlans { get; set; }
  }
}
