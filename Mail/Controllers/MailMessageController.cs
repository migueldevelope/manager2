using Microsoft.AspNetCore.Mvc;
using Manager.Core.Views;
using Manager.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EvaluationMail.Controllers
{
  [Produces("application/json")]
  [Route("mailmessage")]
  public class MailMessageController : Controller
  {
    private readonly IServiceMailMessage service;
    public MailMessageController(IServiceMailMessage _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    [HttpGet]
    [Route("{id}")]
    public ViewMailMessage Get(string id)
    {
      return this.service.GetMessage(id);
    }
  }
}