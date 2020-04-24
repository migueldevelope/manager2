using Manager.Views.BusinessList;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewMapOccupation : _ViewCrudBase
  {
    public List<ViewListProcessLevelTwo> Process { get; set; }
    public string SpecificRequirements { get; set; }
    public List<ViewListActivitie> Activities { get; set; }
    public List<ViewCrudSchoolingOccupation> Schooling { get; set; }
    public List<ViewListSkill> Skills { get; set; }
    public List<ViewListSkill> SkillsCompany { get; set; }
    public List<ViewListSkill> SkillsGroup { get; set; }
    public List<ViewListScope> ScopeGroup { get; set; }
    public ViewListCompany Company { get; set; }
    public ViewListGroup Group { get; set; }

  }
}
