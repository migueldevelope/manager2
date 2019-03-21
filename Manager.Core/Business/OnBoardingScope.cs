using Manager.Core.Base;
using Manager.Core.Enumns;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Coleção para escopo do onboarding
  /// </summary>
  public class OnBoardingScope : BaseEntity
  {
    public Scope Scope { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsPerson { get; set; }
    public List<ListComments> Comments { get; set; }
    public EnumStatusView StatusViewManager { get; set; }
    public EnumStatusView StatusViewPerson { get; set; }
  }
}
