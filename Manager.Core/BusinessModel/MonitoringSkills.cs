using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using System.Collections.Generic;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para skills de monitoring
  /// </summary>
  public class MonitoringSkills : BaseEntity
  {
    public Skill Skill { get; set; }
    public string CommentsPerson { get; set; }
    public string CommentsManager { get; set; }
    public string Praise { get; set; }
    public List<Plan> Plans { get; set; }
    public List<ListComments> Comments { get; set; }
    public EnumStatusView StatusViewManager { get; set; }
    public EnumStatusView StatusViewPerson { get; set; }
  }
}
