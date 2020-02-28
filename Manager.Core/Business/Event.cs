using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados - turma
  /// </summary>
  public class Event : BaseEntity
  {
    public ViewListCourse Course { get; set; }
    public string Name { get; set; }
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
    public List<ViewCrudAttachmentField> Attachments { get; set; }
    public ViewListPersonBase UserInclude { get; set; }
    public DateTime? DateInclude { get; set; }
    public ViewListPersonBase UserEdit { get; set; }
    public DateTime? DateEnd { get; set; }
    public EnumModalityESocial Modality { get; set; }
    public EnumTypeESocial TypeESocial { get; set; }
    public string Code { get; set; }
  }
}
