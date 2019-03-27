﻿using Manager.Core.Base;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para entregas de onboarding
  /// </summary>
  public class OnBoardingActivities : BaseEntity
  {
    public Activitie Activitie { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsPerson { get; set; }
    public List<ListComments> Comments { get; set; }
    public EnumStatusView StatusViewManager { get; set; }
    public EnumStatusView StatusViewPerson { get; set; }
  }
}
