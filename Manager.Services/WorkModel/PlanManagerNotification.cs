using Manager.Core.Base;
using System.Collections.Generic;

namespace Manager.Services.WorkModel
{
  public class PlanManagerNotification
  {
    public BaseFields Manager { get; set; }
    public List<PlanWorkPerson> Defeated { get; set; }
    public List<PlanWorkPerson> DefeatedNow { get; set; }
    public List<PlanWorkPerson> LastSevenDays { get; set; }
    public List<PlanWorkPerson> FifteenDays { get; set; }
    public List<PlanWorkPerson> ThirtyDays { get; set; }
  }
}
