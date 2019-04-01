using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServicePlan
  {
    #region Plan
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetAttachment(string idplan, string idmonitoring, string url, string fileName, string attachmentid);
    string RemoveStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan);
    string RemovePlanActivity(string id);


    List<ViewGetPlan> ListPlans(ref long total, string id, string filter, int count,
      int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait);
    List<ViewGetPlan> ListPlansPerson(ref long total, string id, string filter, int count,
      int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait);
    ViewGetPlan GetPlan(string idmonitoring, string idplan);
    string UpdatePlan(string idmonitoring, ViewCrudPlan viewPlan);
    string NewPlan(string idmonitoring, string idplanold, ViewCrudPlan viewPlan);
    string NewUpdatePlan(string idmonitoring, List<ViewCrudNewPlanUp> viewPlan);
    List<ViewPlanShort> ListPlans(ref long total, string id, string filter, int count, int page);
    List<ViewPlanShort> ListPlansPerson(ref long total, string id, string filter, int count, int page);
    List<ViewListPlanStruct> ListPlansStruct(ref long total, string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte structplan);
    string NewStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, ViewCrudStructPlan structplan);
    ViewCrudStructPlan GetStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan);
    string UpdateStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, ViewCrudStructPlan structplanedit);
    List<ViewPlanActivity> ListPlanActivity(ref long total, string filter, int count, int page);
    ViewPlanActivity GetPlanActivity(string id);
    string NewPlanActivity(ViewPlanActivity model);
    string UpdatePlanActivity(ViewPlanActivity model);
    ViewListPlanStruct GetPlanStruct(string idmonitoring, string idplan);
    #endregion


    #region Old
    
    ViewPlan GetPlanOld(string idmonitoring, string idplan);
    string UpdatePlanOld(string idmonitoring, Plan viewPlan);
    string NewPlanOld(string idmonitoring, string idplanold, Plan viewPlan);
    string NewUpdatePlanOld(string idmonitoring, List<ViewPlanNewUp> viewPlan);
    
    List<ViewPlan> ListPlansPersonOld(ref long total, string id, string filter, int count,
       int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait);
    List<ViewPlan> ListPlansOld(ref long total, string id, string filter, int count, int page);
    List<ViewPlan> ListPlansOld(ref long total, string id, string filter, int count,
      int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait);
    List<ViewPlanShort> ListPlansPersonOld(ref long total, string id, string filter, int count, int page);
    List<ViewPlanStruct> ListPlansStructOld(ref long total, string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte structplan);
    string NewStructPlanOld(string idmonitoring, string idplan, EnumSourcePlan sourceplan, StructPlan structplan);
    StructPlan GetStructPlanOld(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan);
    string UpdateStructPlanOld(string idmonitoring, string idplan, EnumSourcePlan sourceplan, StructPlan structplanedit);
    List<PlanActivity> ListPlanActivityOld(ref long total, string filter, int count, int page);
    PlanActivity GetPlanActivityOld(string id);
    string NewPlanActivityOld(PlanActivity model);
    string UpdatePlanActivityOld(PlanActivity model);
    ViewPlanStruct GetPlanStructOld(string idmonitoring, string idplan);
    #endregion

  }
}
