using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("goals")]
  public class GoalsController : Controller
  {
    private readonly IServiceGoals service;

    public GoalsController(IServiceGoals _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [HttpPost]
    [Route("new")]
    public string Post([FromBody]Goals view)
    {
      return service.New(view);
    }

    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<Goals> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public Goals List(string id)
    {
      return service.Get(id);
    }

    [Authorize]
    [HttpPut]
    [Route("update")]
    public string Update([FromBody]Goals view)
    {
      return service.Update(view);
    }

    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public string Delete(string id)
    {
      return service.Remove(id);
    }

    [HttpPost]
    [Route("newgoalsperiod")]
    public string PostGoalsPeriod([FromBody]GoalsPeriod view)
    {
      return service.NewGoalsPeriod(view);
    }


    [Authorize]
    [HttpGet]
    [Route("listgoalsperiod")]
    public List<GoalsPeriod> ListGoalsPeriod(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGoalsPeriod(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("getgoalsperiod/{id}")]
    public GoalsPeriod ListGoalsPeriod(string id)
    {
      return service.GetGoalsPeriod(id);
    }

    [Authorize]
    [HttpPut]
    [Route("updategoalsperiod")]
    public string UpdateGoalsPeriod([FromBody]GoalsPeriod view)
    {
      return service.UpdateGoalsPeriod(view);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletegoalsperiod/{id}")]
    public string DeleteGoalsPeriod(string id)
    {
      return service.RemoveGoalsPeriod(id);
    }

    [HttpPost]
    [Route("newgoalscompany")]
    public string PostGoalsCompany([FromBody]GoalsCompany view)
    {
      return service.NewGoalsCompany(view);
    }

    [Authorize]
    [HttpGet]
    [Route("listgoalscompany/{idgoalsperiod}/{idcompany}")]
    public List<GoalsCompany> ListGoalsCompany(string idgoalsperiod, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGoalsCompany(idgoalsperiod, idcompany,ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listgoalscompany")]
    public List<GoalsCompany> ListGoalsCompany(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGoalsCompany(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("getgoalscompany/{id}")]
    public GoalsCompany ListGoalsCompany(string id)
    {
      return service.GetGoalsCompany(id);
    }

    [Authorize]
    [HttpPut]
    [Route("updategoalscompany")]
    public string UpdateGoalsCompany([FromBody]GoalsCompany view)
    {
      return service.UpdateGoalsCompany(view);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletegoalscompany/{id}")]
    public string DeleteGoalsCompany(string id)
    {
      return service.RemoveGoalsCompany(id);
    }
  }
}