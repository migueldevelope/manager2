using Manager.Core.Business;
using Manager.Core.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Views
{
  public class ViewPlanNewUp
  {
    public string _id { get; set; }
    public string _idAccount { get; set; }
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
    public string TextEndManager { get; set; }
    public EnumStatusPlanApproved StatusPlanApproved { get; set; }
    public EnumStatus Status { get; set; }
    public EnumTypeViewPlan TypeViewPlan { get; set; }
    public EnumNewAction NewAction { get; set; }
  }
}
