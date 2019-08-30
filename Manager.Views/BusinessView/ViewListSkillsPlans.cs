using Manager.Views.BusinessList;
using System.Collections.Generic;

namespace Manager.Views.BusinessView
{
  public class ViewListSkillsPlans
  {
    public List<ViewListSkill> SkillsCompany { get; set; }
    public List<ViewListSkill> SkillsGroup { get; set; }
    public List<ViewListSkill> SkillsOccupation { get; set; }
  }
}
