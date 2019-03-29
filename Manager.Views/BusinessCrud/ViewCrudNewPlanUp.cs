using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudNewPlanUp: _ViewCrudBase
  {
    public string Description { get; set; }
    public DateTime? Deadline { get; set; }
    public List<ViewListSkill> Skills { get; set; }
    public string UserInclude { get; set; }
    public DateTime? DateInclude { get; set; }
    public EnumTypePlan TypePlan { get; set; }
    public EnumSourcePlan SourcePlan { get; set; }
    public EnumTypeAction TypeAction { get; set; }
    public EnumStatusPlan StatusPlan { get; set; }
    public string TextEnd { get; set; }
    public DateTime? DateEnd { get; set; }
    public byte Evaluation { get; set; }
    public string Result { get; set; }
    public string TextEndManager { get; set; }
    public EnumStatusPlanApproved StatusPlanApproved { get; set; }
    public EnumStatus Status { get; set; }
    public EnumTypeViewPlan TypeViewPlan;
    public EnumNewAction NewAction { get; set; }
  }
}
