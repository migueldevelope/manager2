using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMonitoring: _ViewCrud
  {
    public ViewListPersonInfo Person { get; set; }
    public string CommentsEnd { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsPerson { get; set; }
    public List<ViewCrudMonitoringSkills> SkillsCompany { get; set; }
    public List<ViewCrudMonitoringSchooling> Schoolings { get; set; }
    public List<ViewCrudMonitoringActivities> Activities { get; set; }
    public EnumStatusMonitoring StatusMonitoring { get; set; }
  }
}
