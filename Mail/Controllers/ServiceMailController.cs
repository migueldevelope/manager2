using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Manager.Core.Interfaces;
using Tools;

namespace EvaluationMail.Controllers
{
  [Produces("application/json")]
  [Route("servicemail")]
  public class ServiceMailController : Controller
  {
    private readonly IServiceMail service;
    private string apiSendGridKey;
    private string path;
    public ServiceMailController(IHttpContextAccessor contextAccessor, IServiceMail _service)
    {

      var conn = ConnectionNoSqlService.GetConnetionServer();
      apiSendGridKey = conn.SendGridKey;
      path = conn.TokenServer;
      service = _service;
    }

  }
}