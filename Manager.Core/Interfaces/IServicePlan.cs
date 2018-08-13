using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServicePlan
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    List<Plan> ListPlans(ref long total, string id, string filter, int count, int page);
    List<Plan> ListPlansPerson(ref long total, string id, string filter, int count, int page);
  }
}
