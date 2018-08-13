using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServicePlan
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    IEnumerable<IEnumerable<List<Plan>>> ListPlans(ref long total, string id, string filter, int count, int page);
    IEnumerable<IEnumerable<List<Plan>>> ListPlansPerson(ref long total, string id, string filter, int count, int page);
  }
}
