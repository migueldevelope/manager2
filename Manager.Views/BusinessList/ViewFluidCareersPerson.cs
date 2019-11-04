using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewFluidCareersPerson : _ViewListBase
  {
    public string Occupation { get; set; }
    public string Group { get; set; }
    public string Sphere { get; set; }
    public List<ViewListSkill> SkillsCompany { get; set; }
    public List<ViewListSkill> SkillsGroup { get; set; }
    public List<ViewListSkill> SkillsOccupation { get; set; }
  }
}
