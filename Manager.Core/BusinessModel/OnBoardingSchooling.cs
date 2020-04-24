using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para escolaridade do onboarding
  /// </summary>
  public class OnBoardingSchooling : BaseEntityId
  {

    public ViewCrudSchoolingOccupation Schooling { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsPerson { get; set; }
    public List<ListComments> Comments { get; set; }
    public EnumStatusView StatusViewManager { get; set; }
    public EnumStatusView StatusViewPerson { get; set; }
  }
}
