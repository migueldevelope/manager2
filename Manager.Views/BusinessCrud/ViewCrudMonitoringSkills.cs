﻿using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMonitoringSkills : _ViewCrud
  {
    public ViewListSkill Skill { get; set; }
    public string CommentsPerson { get; set; }
    public string CommentsManager { get; set; }
    public string Praise { get; set; }
    public List<ViewCrudPlan> Plans { get; set; }
    public List<ViewCrudComment> Comments { get; set; }
    public EnumStatusView StatusViewManager { get; set; }
    public EnumStatusView StatusViewPerson { get; set; }
  }
}
