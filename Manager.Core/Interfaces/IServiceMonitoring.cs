using Manager.Core.Business;
using Manager.Core.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Core.Interfaces
{
  public interface IServiceMonitoring
  {
    List<Monitoring> ListMonitoringsWait(string idmanager, ref long total, string filter, int count, int page);
    List<Monitoring> ListMonitoringsEnd(string idmanager, ref long total, string filter, int count, int page);
    Monitoring GetMonitorings(string id);
    List<Skill> GetSkills(string idperson);
    Monitoring PersonMonitoringsWait(string idmanager);
    List<Monitoring> PersonMonitoringsEnd(string idmanager);
    Monitoring NewMonitoring(Monitoring monitoring, string idperson);
    string UpdateMonitoring(Monitoring monitoring, string idperson);
    void SetUser(IHttpContextAccessor contextAccessor);
    string RemoveAllMonitoring(string idperson);
    string RemoveMonitoring(string idmonitoring);
    List<Monitoring> GetListExclud(ref long total, string filter, int count, int page);
    string RemoveLastMonitoring(string idperson);
    MonitoringActivities GetMonitoringActivities(string idmonitoring, string idactivitie);
    string RemoveMonitoringActivities(string idmonitoring, string idactivitie);
    string UpdateMonitoringActivities(string idmonitoring, MonitoringActivities activitie);
    string AddMonitoringActivities(string idmonitoring, Activitie activitie);
    List<ListComments> AddComments(string idmonitoring, string iditem, ListComments comments);
    string UpdateComments(string idmonitoring, string iditem, ListComments comments);
    string DeleteComments(string idmonitoring, string iditem, string idcomments);
    List<ListComments> GetListComments(string idmonitoring, string iditem);
    string UpdateCommentsView(string idmonitoring, string iditem, EnumUserComment userComment);
    bool ValidComments(string id);
    List<Plan> AddPlan(string idmonitoring, string iditem, Plan plan);
    List<Plan> UpdatePlan(string idmonitoring, string iditem, Plan plan);
  }
}
