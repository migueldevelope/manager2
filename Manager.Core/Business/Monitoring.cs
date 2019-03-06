using Manager.Core.Base;
using Manager.Core.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Monitoring : BaseEntity
  {
    public Person Person { get; set; }
    public DateTime? DateBeginPerson { get; set; }
    public DateTime? DateBeginManager { get; set; }
    public DateTime? DateBeginEnd { get; set; }
    public DateTime? DateEndPerson { get; set; }
    public DateTime? DateEndManager { get; set; }
    public DateTime? DateEndEnd { get; set; }
    public string CommentsPerson { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsEnd { get; set; }
    public List<MonitoringSkills> SkillsCompany { get; set; }
    public List<MonitoringSchooling> Schoolings { get; set; }
    public List<MonitoringActivities> Activities { get; set; }
    public EnumStatusMonitoring StatusMonitoring { get; set; }
  }
}
