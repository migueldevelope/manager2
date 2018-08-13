using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Tools;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("plan")]
  public class PlanController : Controller
  {
    private readonly IServicePlan service;

    public PlanController(IServicePlan _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [Authorize]
    [HttpGet]
    [Route("listplans/{id}")]
    public List<ViewPlan> ListPlans(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlans(ref total, id, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }


    [Authorize]
    [HttpGet]
    [Route("listplansperson/{id}")]
    public List<ViewPlan> ListPlansPerson(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlansPerson(ref total, id, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

  }
}