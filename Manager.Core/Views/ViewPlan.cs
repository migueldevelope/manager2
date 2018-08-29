using Manager.Core.Business;
using Manager.Core.Enumns;
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
    public byte Bomb { get; set; }
  }
}
