using Manager.Core.Business;
using Manager.Core.Enumns;
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
    [Route("listplans/{id}")]
    public List<ViewPlanShort> ListPlans(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlans(ref total, id, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listplansperson/{id}")]
    public List<ViewPlanShort> ListPlansPerson(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlansPerson(ref total, id, filter, count, page);
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
    public string NewPlan([FromBody]Plan plan, string idmonitoring, string idplanold)
    {
      return service.NewPlan(idmonitoring, idplanold, plan);
    }

    [Authorize]
    [HttpPut]
    [Route("newupdateplan/{idmonitoring}")]
    public string NewUpdatePlan([FromBody]List<ViewPlanNewUp> plan, string idmonitoring)
    {
      return service.NewUpdatePlan(idmonitoring, plan);
    }


    [Authorize]
    [HttpGet]
    [Route("listplansstruct/{id}/{filter}/{count}/{page}/{activities}/{skillcompany}/{schooling}/{structplan}")]
    List<ViewPlanStruct> ListPlansStruct(string id, string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte structplan)
    {
      long total = 0;
      var result = service.ListPlansStruct(ref total, id, filter, count, page, activities, skillcompany, schooling, structplan);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpPost]
    [Route("newstructplan/{idmonitoring}/{idplan}/{sourceplan}")]
    string NewStructPlan([FromBody] StructPlan structplan, string idmonitoring, string idplan, EnumSourcePlan sourceplan)
    {
      return service.NewStructPlan(idmonitoring, idplan, sourceplan, structplan);
    }

    [Authorize]
    [HttpDelete]
    [Route("removestructplan/{idmonitoring}/{idplan}/{sourceplan}/{idstructplan}")]
    string RemoveStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan)
    {
      return service.RemoveStructPlan(idmonitoring, idplan, sourceplan, idstructplan);
    }

    [Authorize]
    [HttpGet]
    [Route("getstructplan/{idmonitoring}/{idplan}/{sourceplan}/{idstructplan}")]
    StructPlan GetStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan)
    {
      return service.GetStructPlan(idmonitoring, idplan, sourceplan, idstructplan);
    }


    [Authorize]
    [HttpGet]
    [Route("getplanstruct/{idmonitoring}/{idplan}")]
    ViewPlanStruct GetPlanStruct(string idmonitoring, string idplan)
    {
      return service.GetPlanStruct(idmonitoring, idplan);
    }

    [Authorize]
    [HttpPut]
    [Route("updatestructplan/{idmonitoring}/{idplan}/{sourceplan}")]
    string UpdateStructPlan([FromBody]StructPlan structplanedit, string idmonitoring, string idplan, EnumSourcePlan sourceplan)
    {
      return service.UpdateStructPlan(idmonitoring, idplan, sourceplan, structplanedit);
    }

    [Authorize]
    [HttpGet]
    [Route("listplanactivity/{id}/{filter}/{count}/{page}")]
    List<PlanActivity> ListPlanActivity(string id, string filter, int count, int page)
    {
      long total = 0;
      var result = service.ListPlanActivity(ref total, id, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("getplanactivity/{id}")]
    PlanActivity GetPlanActivity(string id)
    {
      return service.GetPlanActivity(id);
    }

    [Authorize]
    [HttpPost]
    [Route("newplanactivity")]
    string NewPlanActivity(PlanActivity model)
    {
      return service.NewPlanActivity(model);
    }

    [Authorize]
    [HttpPut]
    [Route("updateplanactivity")]
    string UpdatePlanActivity(PlanActivity model)
    {
      return service.UpdatePlanActivity(model);
    }

    [Authorize]
    [HttpDelete]
    [Route("removeplanactivity/{id}")]
    string RemovePlanActivity(string id)
    {
      return service.RemovePlanActivity(id);
    }


  }
}