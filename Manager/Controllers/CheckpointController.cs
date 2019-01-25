using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("checkpoint")]
  public class CheckpointController : Controller
  {
    private readonly IServiceCheckpoint service;

    public CheckpointController(IServiceCheckpoint _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [Authorize]
    [HttpPost]
    [Route("new/{idperson}")]
    public Checkpoint Post([FromBody]Checkpoint checkpoint, string idperson)
    {
      return service.NewCheckpoint(checkpoint, idperson);
    }

    [Authorize]
    [HttpPut]
    [Route("update/{idperson}")]
    public string Put([FromBody]Checkpoint checkpoint, string idperson)
    {
      return service.UpdateCheckpoint(checkpoint, idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("listend/{idmanager}")]
    public List<Checkpoint> ListEnd(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCheckpointsEnd(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("list/{idmanager}")]
    public List<Checkpoint> List(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCheckpointsWait(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public Checkpoint GetCheckpoint(string id)
    {
      return service.GetCheckpoints(id);
    }

    [Authorize]
    [HttpGet]
    [Route("getlistexclud")]
    public List<Checkpoint> GetListExclud(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetListExclud(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpDelete]
    [Route("delete/{idperson}")]
    public string RemoveCheckpoint(string idperson)
    {
      return service.RemoveCheckpoint(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("personcheckpointend/{idperson}")]
    public Checkpoint PersonCheckpointEnd(string idperson)
    {
      return service.PersonCheckpointEnd(idperson);
    }


    [Authorize]
    [HttpGet]
    [Route("listcheckpointswaitperson/{idperson}")]
    public Checkpoint ListCheckpointsWaitPerson(string idperson)
    {
      return service.ListCheckpointsWaitPerson(idperson);
    }


  }
}