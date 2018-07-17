using System.Collections.Generic;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tools;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("automanager")]
  public class AutoManagerController : Controller
  {
    private readonly IServiceAutoManager service;


    public AutoManagerController(IHttpContextAccessor contextAccessor, IServiceAutoManager _service)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [Authorize]
    [HttpDelete]
    [Route("{idperson}/delete")]
    public ObjectResult Delete(string idperson)
    {
      service.DeleteManager(idperson);
      return Ok("Person deleted!");
    }

    [Authorize]
    [HttpGet]
    [Route("{idmanager}/listapproved")]
    public List<ViewAutoManager> GetList(string idmanager)
    {
      return service.GetApproved(idmanager);
    }

    [Authorize]
    [HttpGet]
    [Route("{idmanager}/list")]
    public List<ViewAutoManagerPerson> Get(string idmanager, string filter = "")
    {
      return service.List(idmanager, filter);
    }

    [Authorize]
    [HttpPut]
    [Route("{idperson}/new")]
    public string PutNew([FromBody]ViewManager view, string idperson)
    {
      var conn = ConnectionNoSqlService.GetConnetionServer();
      service.SetManagerPerson(view, idperson, conn.TokenServer);
      return "ok";
    }

    [Authorize]
    [HttpPut]
    [Route("{idperson}/approved/{idmanager}")]
    public string PutApproved([FromBody]ViewWorkflow view, string idperson, string idmanager)
    {
      service.Approved(view, idperson, idmanager);
      return "ok";
    }

    [Authorize]
    [HttpPut]
    [Route("{idperson}/disapproved/{idmanager}")]
    public string PutDisapproved([FromBody]ViewWorkflow view, string idperson, string idmanager)
    {
      service.Disapproved(view, idperson, idmanager);
      return "ok";
    }

    [Authorize]
    [HttpPut]
    [Route("{idperson}/canceled/{idmanager}")]
    public string PutCanceled(string idperson, string idmanager)
    {
      service.Canceled(idperson, idmanager);
      return "ok";
    }
  }
}