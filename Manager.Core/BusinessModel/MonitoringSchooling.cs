using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System.Collections.Generic;


namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para escolaridade de monitoring
  /// </summary>
  public class MonitoringSchooling : BaseEntityId
  {
    public ViewCrudSchooling Schooling { get; set; }
    public string CommentsPerson { get; set; }
    public string CommentsManager { get; set; }
    public string Praise { get; set; }
    public List<ViewCrudPlan> Plans { get; set; }
    public List<ListComments> Comments { get; set; }
    public EnumStatusView StatusViewManager { get; set; }
    public EnumStatusView StatusViewPerson { get; set; }
  }
}
