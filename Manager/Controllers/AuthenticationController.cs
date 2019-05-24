using System;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// Controle de Autenticação
  /// </summary>
  [Produces("application/json")]
  [Route("authentication")]
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
    public async Task<ObjectResult> PostNewAuthentication([FromBody]ViewAuthentication userLogin)
    {
      try
      {
        return Ok(service.Authentication(userLogin));
        //test
        //return Ok(new { id = "1" });
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    #endregion

  }
}
