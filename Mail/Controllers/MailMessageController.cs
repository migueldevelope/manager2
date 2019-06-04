using Microsoft.AspNetCore.Mvc;
using Manager.Core.Views;
using Manager.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EvaluationMail.Controllers
{
  /// <summary>
  /// Controlador de Mensagem por E-mail
  /// </summary>
  [Produces("application/json")]
  [Route("mailmessage")]
  public class MailMessageController : Controller
  {
    private readonly IServiceMailMessage service;
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">Serviço envolvido</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public MailMessageController(IServiceMailMessage _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    /// <summary>
    /// Buscar uma mensagem de e-mail
    /// </summary>
    /// <param name="id">Identificador da mensagem</param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    public async Task<ViewMailMessage> Get(string id)
    {
      return await this.service.GetMessage(id);
    }
  }
}