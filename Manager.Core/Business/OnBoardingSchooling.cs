using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class OnBoardingSchooling : BaseEntity
  {
    public Schooling Schooling { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsPerson { get; set; }
  }
}
