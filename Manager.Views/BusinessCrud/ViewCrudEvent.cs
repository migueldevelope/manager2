using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudEvent:_ViewCrudBase
  {
    public ViewListCourse Course { get; set; }
    public string Content { get; set; }
    public ViewCrudEntity Entity { get; set; }
    public decimal MinimumFrequency { get; set; }
    public byte LimitParticipants { get; set; }
    public decimal Grade { get; set; }
    public bool OpenSubscription { get; set; }
    public byte DaysSubscription { get; set; }
    public decimal Workload { get; set; }
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
    public List<ViewCrudInstructor> Instructors { get; set; }
    public List<ViewCrudDaysEvent> Days { get; set; }
    public List<ViewCrudParticipant> Participants { get; set; }
    public EnumStatusEvent StatusEvent { get; set; }
    public string Observation { get; set; }
    public byte Evalution { get; set; }
    public string Code { get; set; }
    public List<ViewCrudAttachmentField> Attachments { get; set; }
    public DateTime? DateEnd { get; set; }
    public EnumModalityESocial Modality { get; set; }
    public EnumTypeESocial TypeESocial { get; set; }
  }
}
