﻿using Manager.Core.Business;
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
    Plan GetPlan(string idmonitoring, string idplan);
    string UpdatePlan(string idmonitoring, Plan viewPlan);
  }
}
