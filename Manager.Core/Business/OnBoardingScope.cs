using Manager.Core.Base;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class OnBoardingScope : BaseEntity
  {
    public Scope Scope { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsPerson { get; set; }
    public List<ListComments> Comments { get; set; }
  }
}
