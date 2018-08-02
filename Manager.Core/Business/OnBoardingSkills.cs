using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class OnBoardingSkills : BaseEntity
  {
    public Skill Skill { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsPerson { get; set; }
  }
}
