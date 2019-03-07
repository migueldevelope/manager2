using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Manager.Core.Interfaces;
using Tools;

namespace EvaluationMail.Controllers
{
  [Produces("application/json")]
  [Route("sendmail")]
  public class SendMailController : Controller
  {
    private readonly IServiceSendGrid service;
    public SendMailController(IHttpContextAccessor contextAccessor, IServiceSendGrid _service)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    [Authorize]
    [HttpPost]
    [Route("{idmail}")]
    public ObjectResult PostMail(string idmail)
    {
      try
      {
        var conn = ConnectionNoSqlService.GetConnetionServer();
        return Ok(service.Send(idmail, conn.SendGridKey));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}