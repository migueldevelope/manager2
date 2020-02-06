using System;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationServer.Controllers
{
  /// <summary>
  /// Controle de Autenticação
  /// </summary>
  [Produces("application/json")]
  [Route("auth")]
  public class AuthenticationController : Controller
  {
    private readonly IServiceAuthentication service;

    #region Constructor
    /// <summary>
    /// Contrutor do controle
    /// </summary>
    /// <param name="_service">Serviço de autenticação</param>
    public AuthenticationController(IServiceAuthentication _service)
    {
      service = _service;
    }
    #endregion

    #region Authentication
    /// <summary>
    /// Autenticação de usuário
    /// </summary>
    /// <param name="userLogin">Objeto de autenticação de usuário</param>
    /// <returns>Informações de login e token de segurança, caso haja problema retorna a mensagem com o problema</returns>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(ViewPerson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ObjectResult> Post([FromBody]ViewAuthentication userLogin)
    {
      try
      {
        return await Task.Run(() => Ok(service.Authentication(userLogin, false)));
      }
      catch (Exception e)
      {
        return await Task.Run(() => BadRequest(e.Message));
      }
    }
    #endregion

  }
}
