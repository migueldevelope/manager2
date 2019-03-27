using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceMonitoring
  {
    #region Monitoring
    string RemoveAllMonitoring(string idperson);
    string RemoveMonitoring(string idmonitoring);
    void SetUser(IHttpContextAccessor contextAccessor);
    string RemoveLastMonitoring(string idperson);
    bool ValidComments(string id);
    string ScriptComments();
    string UpdateCommentsView(string idmonitoring, string iditem, EnumUserComment userComment);
    string DeleteComments(string idmonitoring, string iditem, string idcomments);
    string RemoveMonitoringActivities(string idmonitoring, string idactivitie);


    List<Monitoring> ListMonitoringsWait(string idmanager, ref long total, string filter, int count, int page);
    List<Monitoring> ListMonitoringsEnd(string idmanager, ref long total, string filter, int count, int page);
    Monitoring GetMonitorings(string id);
    List<Skill> GetSkills(string idperson);
    Monitoring PersonMonitoringsWait(string idmanager);
    List<Monitoring> PersonMonitoringsEnd(string idmanager);
    Monitoring NewMonitoring(Monitoring monitoring, string idperson);
    string UpdateMonitoring(Monitoring monitoring, string idperson);
    List<Monitoring> GetListExclud(ref long total, string filter, int count, int page);
    MonitoringActivities GetMonitoringActivities(string idmonitoring, string idactivitie);
    string UpdateMonitoringActivities(string idmonitoring, MonitoringActivities activitie);
    string AddMonitoringActivities(string idmonitoring, Activitie activitie);
    List<ListComments> AddComments(string idmonitoring, string iditem, ListComments comments);
    string UpdateComments(string idmonitoring, string iditem, ListComments comments);
    List<ListComments> GetListComments(string idmonitoring, string iditem);
    List<Plan> AddPlan(string idmonitoring, string iditem, Plan plan);
    List<Plan> UpdatePlan(string idmonitoring, string iditem, Plan plan);
    #endregion



    #region Old
    #endregion
  }
}
