using Manager.Core.Base;
using Manager.Core.BusinessModel;
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
    public Course Course { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public Entity Entity { get; set; }
    public decimal MinimumFrequency { get; set; }
    public byte LimitParticipants { get; set; }
    public decimal Grade { get; set; }
    public bool OpenSubscription { get; set; }
    public byte DaysSubscription { get; set; }
    public decimal Workload { get; set; }
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
    public List<Instructor> Instructors { get; set; }
    public List<DaysEvent> Days { get; set; }
    public List<Participant> Participants { get; set; }
    public EnumStatusEvent StatusEvent { get; set; }
    public string Observation { get; set; }
    public byte Evalution { get; set; }
    public List<AttachmentField> Attachments { get; set; }
    public Person UserInclude { get; set; }
    public DateTime? DateInclude { get; set; }
    public Person UserEdit { get; set; }
    public DateTime? DateEnd { get; set; }
    public EnumModalityESocial Modality { get; set; }
    public EnumTypeESocial TypeESocial { get; set; }
  }
}
