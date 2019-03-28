using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudOnboarding : _ViewCrud
  {
    public ViewInfoPerson Person { get; set; }
    public ViewListOccupation Occupation { get; set; }
    public string CommentsPerson { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsEnd { get; set; }
    public List<ViewCrudOnboardingSkill> SkillsCompany { get; set; }
    public List<ViewCrudOnboardingSkill> SkillsGroup { get; set; }
    public List<ViewCrudOnboardingSkill> SkillsOccupation { get; set; }
    public List<ViewCrudOnboardingScope> Scopes { get; set; }
    public List<ViewCrudOnboardingSchooling> Schoolings { get; set; }
    public List<ViewCrudOnboardingActivitie> Activities { get; set; }
    public EnumStatusOnBoarding StatusOnBoarding { get; set; }
  }
}
