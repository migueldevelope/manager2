using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListPlan : _ViewListBase
  {
    public string Description { get; set; }
    public DateTime? Deadline { get; set; }
    public List<ViewListSkill> Skills { get; set; }
    public EnumTypePlan TypePlan { get; set; }
    public EnumSourcePlan SourcePlan { get; set; }
  }
}
