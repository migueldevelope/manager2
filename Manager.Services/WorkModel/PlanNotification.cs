using Manager.Core.Business;
using System.Collections.Generic;

namespace Manager.Services.WorkModel
{
  public class PlanNotification
  {
    public Person Person { get; set; }
    public List<Plan> Defeated { get; set; }
    public List<Plan> DefeatedNow { get; set; }
    public List<Plan> LastSevenDays { get; set; }
    public List<Plan> FifteenDays { get; set; }
    public List<Plan> ThirtyDays { get; set; }
  }
}
