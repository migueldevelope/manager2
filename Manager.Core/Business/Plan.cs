using Manager.Core.Base;
using Manager.Core.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Plan : BaseEntity
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? Deadline { get; set; }
    public List<Skill> Skills { get; set; }
    public Person UserInclude { get; set; }
    public DateTime? DateInclude { get; set; }
    public EnumTypePlan TypePlan { get; set; }
    public EnumSourcePlan SourcePlan { get; set; }
    public EnumTypeAction TypeAction { get; set; }
    public EnumStatusPlan StatusPlan { get; set; }
    public string TextEnd { get; set; }
    public DateTime? DateEnd { get; set; }
    public byte Evaluation { get; set; }
    public string Result { get; set; }
    public EnumStatusPlanApproved StatusPlanApproved { get; set; }
    public List<AttachmentField> Attachments { get; set; }
  }
}
