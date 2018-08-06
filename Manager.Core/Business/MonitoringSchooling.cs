using Manager.Core.Base;
using System.Collections.Generic;


namespace Manager.Core.Business
{
  public class MonitoringSchooling : BaseEntity
  {
    public Schooling Schooling { get; set; }
    public string CommentsPerson { get; set; }
    public string CommentsManager { get; set; }
    public List<Plan> Plans { get; set; }
  }
}
