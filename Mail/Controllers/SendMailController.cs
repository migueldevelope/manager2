using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Manager.Core.Interfaces;
using Tools;
using Tools.Data;
using System.Threading.Tasks;

namespace EvaluationMail.Controllers
{
  /// <summary>
  /// Controlador de envio de e-mail
  /// </summary>
  [Produces("application/json")]
  [Route("sendmail")]
  public class SendMailController : Controller
  {
    private readonly IServiceSendGrid service;

    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Serviço de envio de e-mail</param>
    /// <param name="contextAccessor">token de segurança</param>
    public SendMailController(IServiceSendGrid _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    /// <summary>
    /// Enviar e-mail gravado 
    /// </summary>
    /// <param name="idmail">Identificador do e-mail</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("{idmail}")]
    public async Task<IActionResult> Send(string idmail)
    {
      try
      {
        Config conn = XmlConnection.ReadConfig();
        return Ok(service.Send(idmail, conn.SendGridKey));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}