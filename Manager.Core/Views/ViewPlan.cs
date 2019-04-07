using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Views
{
  public class ViewPlan
  {
    public string _idAccount { get; set; }
    public string _id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? Deadline { get; set; }
    public List<Skill> Skills { get; set; }
    public Person UserInclude { get; set; }
    public DateTime? DateInclude { get; set; }
    public EnumTypePlan TypePlan { get; set; }
    public string IdPerson { get; set; }
    public string NamePerson { get; set; }
    public EnumSourcePlan SourcePlan { get; set; }
    public string IdMonitoring { get; set; }
    public byte Evaluation { get; set; }
    public EnumStatusPlan StatusPlan { get; set; }
    public EnumTypeAction TypeAction { get; set; }
    public long Bomb { get; set; }
    public EnumStatusPlanApproved StatusPlanApproved { get; set; }
    public string TextEnd { get; set; }
    public string TextEndManager { get; set; }
    public DateTime? DateEnd { get; set; }
    public EnumStatus Status { get; set; }
    public List<AttachmentField> Attachments { get; set; }
    public Plan PlanNew { get; set; }
    public EnumNewAction NewAction { get; set; }
  }
}
