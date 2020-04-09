using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessView
{
  public class ViewReportsMonitoring
  {
    public string Name { get; set; }
    public DateTime? DateAdm { get; set; }
    public string Schooling { get; set; }
    public string Manager { get; set; }
    public string Occupation { get; set; }
    public EnumTypeJourney TypeJourney { get; set; }

    public string CommentsPerson { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsEnd { get; set; }

    public string Concept { get; set; }
    public EnumTypeSkill TypeSkill { get; set; }
    public long Order { get; set; }
    public string Complement { get; set; }
    public EnumTypeSchooling Type { get; set; }
    public EnumTypeItem TypeItem { get; set; }
    public string NameItem { get; set; }
    public string _idItem { get; set; }
    public string Comments { get; set; }
    public string Praise { get; set; }

    public string NamePlan { get; set; }
    public string DescriptionPlan { get; set; }
    public DateTime? Deadline { get; set; }
    public EnumSourcePlan SourcePlan { get; set; }
    public EnumStatusPlan StatusPlan { get; set; }
    public EnumStatusPlanApproved StatusPlanApproved { get; set; }
    public string TextEndManager { get; set; }
    public string TextEnd { get; set; }
    public byte Evaluation { get; set; }
    public string SkillPlan { get; set; }
    public string ConceptPlan { get; set; }

    public EnumUserComment UserComment { get; set; }
  }
}
