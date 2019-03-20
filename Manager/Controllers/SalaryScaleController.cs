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
    [Route("list/{idcompany}")]
    public List<SalaryScale> List(string idcompany, string idestablishment, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(idcompany, ref total, count, page, filter);
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

    [Authorize]
    [HttpPost]
    [Route("addgrade/{idsalaryscale}")]
    public string PostGrade([FromBody]Grade view, string idsalaryscale)
    {
      return service.AddGrade(view,idsalaryscale );
    }

    [Authorize]
    [HttpPut]
    [Route("updatesalaryscale")]
    public string UpdateSalary([FromBody]ViewUpdateSalaryScale view)
    {
      return service.UpdateSalaryScale(view);
    }

    [Authorize]
    [HttpPost]
    [Route("newsalaryscale")]
    public string PostSalary([FromBody]ViewNewSalaryScale view)
    {
      return service.NewSalaryScale(view);
    }

    [Authorize]
    [HttpGet]
    [Route("listgrade/{idsalaryscale}")]
    public List<Grade> ListGrade(string idsalaryscale, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGrade(idsalaryscale, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listgrades/{idcompany}")]
    public List<SalaryScaleGrade> ListGrades(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGrades(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }


    [Authorize]
    [HttpPut]
    [Route("updatestep")]
    public string UpdateGrade([FromBody]ViewStep view)
    {
      return service.UpdateStep(view.idsalaryscale, view.idgrade, view.Step, view.Salary);
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
    [Route("updategrade/{idsalaryscale}")]
    public string UpdateGrade([FromBody]Grade view, string idsalaryscale)
    {
      return service.UpdateGrade(view,idsalaryscale);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletegrade/{id}/{idsalaryscale}")]
    public string DeleteGrade(string id, string idsalaryscale)
    {
      return service.RemoveGrade(id, idsalaryscale);
    }
  }
}