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
    [Route("listplans/{id}/{activities}/{skillcompany}/{schooling}/{open}/{expired}/{end}")]
    public List<ViewPlan> ListPlans(string id, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlans(ref total, id, filter, count, page, activities, skillcompany, schooling, open, expired, end);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }


    [Authorize]
    [HttpGet]
    [Route("listplansperson/{id}/{activities}/{skillcompany}/{schooling}/{open}/{expired}/{end}")]
    public List<ViewPlan> ListPlansPerson(string id, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlansPerson(ref total, id, filter, count, page, activities, skillcompany, schooling, open, expired, end);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("getplan/{idmonitoring}/{idplan}")]
    public ViewPlan GetPlan(string idmonitoring, string idplan)
    {
      return service.GetPlan(idmonitoring, idplan);
    }

    [Authorize]
    [HttpPut]
    [Route("updateplan/{idmonitoring}")]
    public string UpdatePlan([FromBody]Plan plan, string idmonitoring)
    {
      return service.UpdatePlan(idmonitoring, plan);
    }

    [Authorize]
    [HttpPost]
    [Route("newplan/{idmonitoring}/{idplanold}")]
    public string UpdatePlan([FromBody]Plan plan, string idmonitoring, string idplanold)
    {
      return service.NewPlan(idmonitoring, idplanold, plan);
    }

  }
}