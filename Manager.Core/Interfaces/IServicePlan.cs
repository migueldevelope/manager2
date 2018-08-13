using Manager.Core.Business;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServicePlan
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    List<ViewPlan> ListPlans(ref long total, string id, string filter, int count, int page);
    List<ViewPlan> ListPlansPerson(ref long total, string id, string filter, int count, int page);
  }
}
