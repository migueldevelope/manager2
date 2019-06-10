using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServicePlan
  {
    #region Plan
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task SetAttachment(string idplan, string idmonitoring, string url, string fileName, string attachmentid);
    Task<string> RemoveStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan);
    Task<string> RemovePlanActivity(string id);

    Task<List<ViewGetPlan>> ListPlans( string id,ref  long total,  string filter, int count,
      int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait);
    Task<List<ViewGetPlan>> ListPlansPerson( string id,ref  long total,  string filter, int count,
      int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait);
    Task<ViewGetPlan> GetPlan(string idmonitoring, string idplan);
    Task<string> UpdatePlan(string idmonitoring, ViewCrudPlan viewPlan);
    Task<string> NewPlan(string idmonitoring, string idplanold, ViewCrudPlan viewPlan);
    Task<string> NewUpdatePlan(string idmonitoring, List<ViewCrudNewPlanUp> viewPlan);
    Task<List<ViewPlanShort>> ListPlans( string id,ref  long total,  string filter, int count, int page);
    Task<List<ViewPlanShort>> ListPlansPerson( string id,ref  long total,  string filter, int count, int page);
    Task<List<ViewListPlanStruct>> ListPlansStruct(ref  long total,  string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte structplan);
    Task<string> NewStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, ViewCrudStructPlan structplan);
    Task<ViewCrudStructPlan> GetStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan);
    Task<string> UpdateStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, ViewCrudStructPlan structplanedit);
    Task<List<ViewPlanActivity>> ListPlanActivity(ref  long total,  string filter, int count, int page);
    Task<ViewPlanActivity> GetPlanActivity(string id);
    Task<string> NewPlanActivity(ViewPlanActivity model);
    Task<string> UpdatePlanActivity(ViewPlanActivity model);
    Task<ViewListPlanStruct> GetPlanStruct(string idmonitoring, string idplan);
    #endregion


  }
}
