using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMonitoring: _ViewCrud
  {
    public string _idPerson { get; set; }
    public DateTime? DateBeginPerson { get; set; }
    public DateTime? DateBeginManager { get; set; }
    public DateTime? DateBeginEnd { get; set; }
    public DateTime? DateEndPerson { get; set; }
    public DateTime? DateEndManager { get; set; }
    public DateTime? DateEndEnd { get; set; }
    public string CommentsEnd { get; set; }
    public List<ViewCrudMonitoringSkills> SkillsCompany { get; set; }
    public List<ViewCrudMonitoringSchooling> Schoolings { get; set; }
    public List<ViewCrudMonitoringActivities> Activities { get; set; }
    public EnumStatusMonitoring StatusMonitoring { get; set; }
  }
}
