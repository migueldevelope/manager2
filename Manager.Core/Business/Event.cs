﻿using Manager.Core.Base;
using Manager.Core.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Event : BaseEntity
  {
    public Course Course { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public Entity Entity { get; set; }
    public byte MinimumFrequency { get; set; }
    public byte LimitParticipants { get; set; }
    public decimal Grade { get; set; }
    public bool OpenSubscription { get; set; }
    public byte DaysSubscription { get; set; }
    public decimal Workload { get; set; }
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
  }
}
