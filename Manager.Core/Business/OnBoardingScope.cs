using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class OnBoardingScope : BaseEntity
  {
    public Scope Scope { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsPerson { get; set; }
  }
}
