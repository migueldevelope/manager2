﻿using Manager.Core.Base;
using Manager.Core.Enumns;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class MonitoringActivitiesOld : BaseEntity
  {
    public Activitie Activities { get; set; }
    public string CommentsPerson { get; set; }
    public string CommentsManager { get; set; }
    public string Praise { get; set; }
    public List<PlanOld> Plans { get; set; }
    public EnumTypeAtivitie TypeAtivitie { get; set; }
    public List<ListComments> Comments { get; set; }
    public EnumStatusView StatusViewManager { get; set; }
    public EnumStatusView StatusViewPerson { get; set; }
  }
}