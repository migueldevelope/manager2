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
    Task<string> RemoveAllMonitoring(string idperson);
    Task<string> RemoveMonitoring(string idmonitoring);
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task<string> RemoveLastMonitoring(string idperson);
    Task<bool> ValidComments(string id);
    Task<string> UpdateCommentsView(string idmonitoring, string iditem, EnumUserComment userComment);
    Task<string> DeleteComments(string idmonitoring, string iditem, string idcomments);
    Task<string> RemoveMonitoringActivities(string idmonitoring, string idactivitie);


    Task<List<ViewListMonitoring>> ListMonitoringsWait(string idmanager,  string filter, int count, int page);
    Task<List<ViewListMonitoring>> ListMonitoringsEnd(string idmanager,  string filter, int count, int page);
    Task<ViewCrudMonitoring> GetMonitorings(string id);
    Task<List<ViewListSkill>> GetSkills(string idperson);
    Task<ViewListMonitoring> PersonMonitoringsWait(string idmanager);
    Task<List<ViewListMonitoring>> PersonMonitoringsEnd(string idmanager);
    Task<ViewListMonitoring> NewMonitoring(string idperson);
    Task<string> UpdateMonitoring(ViewCrudMonitoring view);
    Task<List<ViewListMonitoring>> GetListExclud( string filter, int count, int page);
    Task<ViewCrudMonitoringActivities> GetMonitoringActivities(string idmonitoring, string idactivitie);
    Task<string> UpdateMonitoringActivities(string idmonitoring, ViewCrudMonitoringActivities view);
    Task<string> AddMonitoringActivities(string idmonitoring, ViewCrudActivities view);
    Task<List<ViewCrudComment>> AddComments(string idmonitoring, string iditem, ViewCrudComment comments);
    Task<string> UpdateComments(string idmonitoring, string iditem, ViewCrudComment comments);
    Task<List<ViewCrudComment>> GetListComments(string idmonitoring, string iditem);
    Task<List<ViewCrudPlan>> AddPlan(string idmonitoring, string iditem, ViewCrudPlan plan);
    Task<List<ViewCrudPlan>> UpdatePlan(string idmonitoring, string iditem, ViewCrudPlan plan);

    #endregion

  }
}
