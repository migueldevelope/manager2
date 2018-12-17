using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("salaryscale")]
  public class SalaryScaleController : Controller
  {
    private readonly IServiceSalaryScale service;

    public SalaryScaleController(IServiceSalaryScale _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [Authorize]
    [HttpGet]
    [Route("list/{idcompany}/{idestablishment}")]
    public List<SalaryScale> List(string idcompany, string idestablishment, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(idcompany, idestablishment, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public SalaryScale List(string id)
    {
      return service.Get(id);
    }


    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public string Delete(string id)
    {
      return service.Remove(id);
    }

    [HttpPost]
    [Route("newgrade")]
    public string PostGrade([FromBody]Grade view)
    {
      return service.NewGrade(view);
    }

    [Authorize]
    [HttpGet]
    [Route("listgrade/{idcompany}")]
    public List<Grade> ListGrade(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGrade(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpPut]
    [Route("updatestep")]
    public string UpdateGrade([FromBody]ViewStep view)
    {
      return service.UpdateStep(view.idestablishment, view.idgrade, view.Step, view.Salary);
    }


    [Authorize]
    [HttpGet]
    [Route("getgrade/{id}")]
    public Grade ListGrade(string id)
    {
      return service.GetGrade(id);
    }

    [Authorize]
    [HttpPut]
    [Route("updategrade")]
    public string UpdateGrade([FromBody]Grade view)
    {
      return service.UpdateGrade(view);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletegrade/{id}")]
    public string DeleteGrade(string id)
    {
      return service.RemoveGrade(id);
    }
  }
}