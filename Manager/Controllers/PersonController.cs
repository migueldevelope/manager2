using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tools;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("person")]
  public class PersonController : Controller
  {
    private readonly IServicePerson service;

    public PersonController(IHttpContextAccessor contextAccessor, IServicePerson _service)
    {
      try
      {
        service = _service;
        service.SetUser(contextAccessor);
      }
      catch (Exception)
      {
        throw;
      }

    }

    [Authorize]
    [HttpGet]
    [Route("personalinformation/{idPerson}")]
    public ViewPersonDetail GetPerson(string idPerson)
    {
      return service.GetPersonDetail(idPerson);
    }


    [Authorize]
    [HttpGet]
    [Route("directteam/{idPerson}")]
    public List<ViewPersonTeam> GetPersonTeam(string idPerson, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetPersonTeam(ref total, idPerson, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }


    [Authorize]
    [HttpGet]
    [Route("photo/{idPerson}")]
    public string GetPhoto(string idPerson)
    {
      return service.GetPhoto(idPerson);
    }

    [Authorize]
    [HttpPut]
    [Route("alterpass/{idPerson}")]
    public string AlterPass([FromBody]ViewAlterPass view, string idPerson)
    {
      return service.AlterPassword(view, idPerson);
    }


    [HttpPut]
    [Route("forgotpassword/{foreign}/alter")]
    public string ForgotPassword([FromBody]ViewAlterPass view, string foreign)
    {
      return service.AlterPasswordForgot(view, foreign);
    }

    [HttpPut]
    [Route("forgotpassword/{mail}")]
    public string ForgotPassword([FromBody]ViewForgotPassword view, string mail)
    {
      var conn = ConnectionNoSqlService.GetConnetionServer();
      var sendGridKey = conn.SendGridKey;
      return service.ForgotPassword(mail, view, sendGridKey).Result;
    }


    [Authorize]
    [HttpGet]
    [Route("listpersons")]
    public List<ViewPersonList> ListPersons(string filter = "")
    {
      return service.GetPersons(filter);
    }

    [Authorize]
    [HttpGet]
    [Route("{idperson}/head")]
    public ViewPersonHead Head(string idperson)
    {
      return service.Head(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<Person> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetPersonsCrud(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("{idperson}/edit")]
    public Person GetEdit(string idperson)
    { 
      return service.GetPersonCrud(idperson); ;
    }


    [Authorize]
    [HttpPost]
    [Route("new")]
    public string Post([FromBody] ViewPersonsCrud person)
    {
      return service.NewPerson(person); 
    }

    [Authorize]
    [HttpPut]
    [Route("update/{id}")]
    public string Put([FromBody] ViewPersonsCrud person, string id)
    {
      return service.UpdatePerson(id,person);
    }

    [Authorize]
    [HttpGet]
    [Route("listoccupation")]
    public List<Occupation> ListOccupation(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOccupation(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listcompany")]
    public List<Company> ListCompany(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCompany(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listmanager")]
    public List<Person> ListManager(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListManager(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }



  }
}