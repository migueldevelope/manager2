using Manager.Views.BusinessCrud;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListEventSubscription:_ViewList
  {
    public string NameEvent { get; set; }
    public List<ViewCrudDaysEvent> Days { get; set; }
    public List<ViewCrudInstructor> Instructors { get; set; }
    public decimal Workload { get; set; }
    public string Entity { get; set; }
    public string Observation { get; set; }
    public List<ViewCrudAttachmentField> Attachments { get; set; }
  }
}
