using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para skills de onboarding
  /// </summary>
  public class OnBoardingSkills : BaseEntity
  {
    public Skill Skill { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsPerson { get; set; }
    public List<ListComments> Comments { get; set; }
    public EnumStatusView StatusViewManager { get; set; }
    public EnumStatusView StatusViewPerson { get; set; }
  }
}
