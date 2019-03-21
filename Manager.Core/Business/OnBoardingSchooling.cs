﻿using Manager.Core.Base;
using Manager.Core.Enumns;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Coleção para escolaridade do onboarding
  /// </summary>
  public class OnBoardingSchooling : BaseEntity
  {
    public Schooling Schooling { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsPerson { get; set; }
    public List<ListComments> Comments { get; set; }
    public EnumStatusView StatusViewManager { get; set; }
    public EnumStatusView StatusViewPerson { get; set; }
  }
}
