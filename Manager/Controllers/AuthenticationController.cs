using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    public ObjectResult PostNewAuthentication([FromBody]ViewAuthentication userLogin)
    {
      try
      {
        return Ok(service.Authentication(userLogin));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    /// <summary>
    /// Aceite do Termo de Serviço
    /// </summary>
    /// <param name="iduser">Identificador do usuário</param>
    /// <returns>Informações de login e token de segurança, caso haja problema retorna a mensagem com o problema</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route("checktermofservice/{iduser}")]
    public string CheckTermOfService(string iduser)
    {
      service.CheckTermOfService(iduser);
      return "ok";
    }

    #endregion

  }
}
