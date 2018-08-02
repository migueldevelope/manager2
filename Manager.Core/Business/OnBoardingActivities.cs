using Manager.Core.Base;

namespace Manager.Core.Business
{
    public class OnBoardingActivities: BaseEntity
    {
    public Activitie Activitie { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsPerson { get; set; }
  }
}
