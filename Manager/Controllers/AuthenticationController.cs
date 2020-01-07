using System;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [ProducesResponseType(typeof(ViewPerson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ObjectResult> Post([FromBody]ViewAuthentication userLogin)
    {
      try
      {
        return await Task.Run(() =>Ok(service.Authentication(userLogin, true)));
      }
      catch (Exception e)
      {
        return await Task.Run(() =>BadRequest(e.Message));
      }
    }

    /// <summary>
    /// Authentication version 2
    /// </summary>
    /// <param name="userLogin"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    [Route("v2")]
    [ProducesResponseType(typeof(ViewPerson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ObjectResult> PostV2([FromBody]ViewAuthentication userLogin)
    {
      try
      {
        return await Task.Run(() => Ok(service.AuthenticationV2(userLogin, EnumTypeAuth.Default)));
      }
      catch (Exception e)
      {
        return await Task.Run(() => BadRequest(e.Message));
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="idaccount"></param>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("altercontract/{idperson}/{idaccount}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<string> AlterContract(string idperson, string idaccount)
    {
      try
      {
        return await Task.Run(() => service.AlterContract(idperson, idaccount));
      }
      catch (Exception e)
      {
        return await Task.Run(() => e.Message);
      }
    }
    #endregion

  }
}
