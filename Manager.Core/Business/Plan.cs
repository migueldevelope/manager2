using Manager.Core.Base;
using Manager.Core.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Plan: BaseEntity
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? Deadline { get; set; }
    public List<Skill> Skills { get; set; }
    public Person UserInclude { get; set; }
    public DateTime? DateInclude { get; set; }
    public EnumTypePlan TypePlan { get; set; }
  }
}
