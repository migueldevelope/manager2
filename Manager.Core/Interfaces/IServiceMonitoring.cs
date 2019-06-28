using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceMonitoring
  {
    #region Monitoring
     string RemoveAllMonitoring(string idperson);
     string RemoveMonitoring(string idmonitoring);
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
     string RemoveLastMonitoring(string idperson);
     bool ValidComments(string id);
     string UpdateCommentsView(string idmonitoring, string iditem, EnumUserComment userComment);
     string DeleteComments(string idmonitoring, string iditem, string idcomments);
     string RemoveMonitoringActivities(string idmonitoring, string idactivitie);


     List<ViewListMonitoring> ListMonitoringsWait(string idmanager, ref  long total,  string filter, int count, int page);
     List<ViewListMonitoring> ListMonitoringsEnd(string idmanager, ref  long total,  string filter, int count, int page);
     ViewCrudMonitoring GetMonitorings(string id);
     List<ViewListSkill> GetSkills(string idperson);
     ViewListMonitoring PersonMonitoringsWait(string idmanager);
     List<ViewListMonitoring> PersonMonitoringsEnd(string idmanager);
     ViewListMonitoring NewMonitoring(string idperson);
     string UpdateMonitoring(ViewCrudMonitoring view);
     List<ViewListMonitoring> GetListExclud(ref  long total,  string filter, int count, int page);
     ViewCrudMonitoringActivities GetMonitoringActivities(string idmonitoring, string idactivitie);
     string UpdateMonitoringActivities(string idmonitoring, ViewCrudMonitoringActivities view);
     string AddMonitoringActivities(string idmonitoring, ViewCrudActivities view);
     List<ViewCrudComment> AddComments(string idmonitoring, string iditem, ViewCrudComment comments);
     string UpdateComments(string idmonitoring, string iditem, ViewCrudComment comments);
     List<ViewCrudComment> GetListComments(string idmonitoring, string iditem);
     List<ViewCrudPlan> AddPlan(string idmonitoring, string iditem, ViewCrudPlan plan);
     List<ViewCrudPlan> UpdatePlan(string idmonitoring, string iditem, ViewCrudPlan plan);

    #endregion

  }
}
