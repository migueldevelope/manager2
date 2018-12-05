using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("logmessages")]
  public class LogMessagesController : Controller
  {
    private readonly IServiceLogMessages service;

    public LogMessagesController(IServiceLogMessages _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [HttpPost]
    [Route("new")]
    public string Post([FromBody]LogMessages view)
    {
      return service.New(view);
    }

    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<LogMessages> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listmanager/{id}")]
    public List<LogMessages> ListManager(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListManager(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listperson/{id}")]
    public List<LogMessages> ListPerson(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPerson(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("get")]
    public LogMessages List(string id)
    {
      return service.Get(id);
    }

    [Authorize]
    [HttpPut]
    [Route("update")]
    public string Update([FromBody]LogMessages view)
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

  }
}