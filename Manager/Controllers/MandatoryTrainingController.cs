using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("mandatorytraining")]
  public class MandatoryTrainingController : Controller
  {
    private readonly IServiceMandatoryTraining service;

    public MandatoryTrainingController(IServiceMandatoryTraining _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [Authorize]
    [HttpPost]
    [Route("addcompany")]
    public string AddCompany([FromBody] ViewAddCompanyMandatory view)
    {
      return service.AddCompany(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addoccuaption")]
    public string AddOccuaption([FromBody]ViewAddOccupationMandatory view)
    {
      return service.AddOccuaption(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addperson")]
    public string AddPerson([FromBody]ViewAddPersonMandatory view)
    {
      return service.AddPerson(view);
    }

    [Authorize]
    [HttpGet]
    [Route("gettrainingplan/{id}")]
    public TrainingPlan GetTrainingPlan(string id)
    {
      return service.GetTrainingPlan(id);
    }

    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<MandatoryTraining> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listcompany/{idcourse}")]
    public List<Company> ListCompany(string idcourse, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCompany(idcourse, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listoccupation/{idcourse}/{idcompany}")]
    public List<Occupation> ListOccupation(string idcourse, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOccupation(idcourse, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listperson/{idcourse}/{idcompany}")]
    public List<Person> ListPerson(string idcourse, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPerson(idcourse, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listtrainingplan/{idcompany}")]
    public List<TrainingPlan> ListTrainingPlan(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTrainingPlan(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpPost]
    [Route("newtrainingplan")]
    public string NewTrainingPlan([FromBody]TrainingPlan view)
    {
      return service.NewTrainingPlan(view);
    }

    [Authorize]
    [HttpDelete]
    [Route("removecompany/{idcourse}/{idcompany}")]
    public string RemoveCompany(string idcourse, string idcompany)
    {
      return service.RemoveCompany(idcourse, idcompany);
    }

    [Authorize]
    [HttpDelete]
    [Route("removeoccupation/{idcourse}/{idoccupation}")]
    public string RemoveOccupation(string idcourse, string idoccupation)
    {
      return service.RemoveOccupation(idcourse, idoccupation);
    }

    [Authorize]
    [HttpDelete]
    [Route("removeperson/{idcourse}/{idperson}")]
    public string RemovePerson(string idcourse, string idperson)
    {
      return service.RemovePerson(idcourse, idperson);
    }

    [Authorize]
    [HttpDelete]
    [Route("removetrainingplan/{id}")]
    public string RemoveTrainingPlan(string id)
    {
      return service.RemoveTrainingPlan(id);
    }

    [Authorize]
    [HttpPut]
    [Route("updatetrainingplan")]
    public string UpdateTrainingPlan([FromBody]TrainingPlan view)
    {
      return service.UpdateTrainingPlan(view);
    }

  }
}