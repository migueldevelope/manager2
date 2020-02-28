using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudOccupationCareers : _ViewCrudBase
  {
    public decimal Accuracy { get; set; }
    public EnumOccupationColor Color {get;set;}
    public List<ViewListActivitie> Activities { get; set; }
    public List<ViewListScope> Scopes { get; set; }
    public List<ViewListSkill> SkillsCompany { get; set; }
    public List<ViewListSkill> SkillsGroup { get; set; }
    public List<ViewListSkill> SkillsOccupation { get; set; }
    public List<ViewCrudSchooling> Schollings { get; set; }
  }
}
