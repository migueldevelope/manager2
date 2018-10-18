using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServicePlan
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    List<ViewPlan> ListPlans(ref long total, string id, string filter, int count,
      int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end);
    List<ViewPlan> ListPlansPerson(ref long total, string id, string filter, int count,
      int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end);
    ViewPlan GetPlan(string idmonitoring, string idplan);
    string UpdatePlan(string idmonitoring, Plan viewPlan);
    string NewPlan(string idmonitoring, string idplanold, Plan viewPlan);
    void SetAttachment(string idplan, string idmonitoring,string url, string fileName, string attachmentid);
    string NewUpdatePlan(string idmonitoring, List<ViewPlanNewUp> viewPlan);
    List<ViewPlanShort> ListPlans(ref long total, string id, string filter, int count, int page);
    List<ViewPlanShort> ListPlansPerson(ref long total, string id, string filter, int count, int page);
    List<ViewPlanStruct> ListPlansStruct(ref long total, string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte structplan);
    string NewStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, StructPlan structplan);
    string RemoveStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan);
    StructPlan GetStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan);
    string UpdateStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, StructPlan structplanedit);
    List<PlanActivity> ListPlanActivity(ref long total, string filter, int count, int page);
    PlanActivity GetPlanActivity(string id);
    string NewPlanActivity(PlanActivity model);
    string UpdatePlanActivity(PlanActivity model);
    string RemovePlanActivity(string id);
    ViewPlanStruct GetPlanStruct(string idmonitoring, string idplan);

  }
}
