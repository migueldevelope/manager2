
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudPlan : _ViewCrudBase
  {
    public string Description { get; set; }
    public DateTime? Deadline { get; set; }
    public List<ViewListSkill> Skills { get; set; }
    public EnumTypePlan TypePlan { get; set; }
    public EnumSourcePlan SourcePlan { get; set; }
    public EnumStatusPlan StatusPlan { get; set; }
    public EnumStatusPlanApproved StatusPlanApproved { get; set; }
    public EnumTypeAction TypeAction { get; set; }
    public List<ViewCrudAttachmentField> Attachments { get; set; }
    public EnumNewAction NewAction { get; set; }
    public string TextEnd { get; set; }
    public string TextEndManager { get; set; }
    public byte Evaluation { get; set; }
  }
}
