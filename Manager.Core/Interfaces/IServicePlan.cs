using Manager.Core.Business;
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

    string NewUpdatePlan(string idmonitoring, List<ViewPlanNewUp> viewPlan);
  }
}
