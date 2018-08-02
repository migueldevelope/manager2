using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Manager.Core.Views;
using Manager.Core.Interfaces;

namespace EvaluationMail.Controllers
{
  [Produces("application/json")]
  [Route("mailmessage")]
  public class MailMessageController : Controller
  {
    private readonly IServiceMailMessage service;

    public MailMessageController(IServiceMailMessage _service)
    {
      service = _service;
    }

    [HttpGet]
    [Route("{id}")]
    public ViewMailMessage Get(string id)
    {
      return this.service.GetMessage(id);
    }
  }
}