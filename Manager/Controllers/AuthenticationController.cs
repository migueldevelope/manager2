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
        return await Task.Run(() =>Ok(service.Authentication(userLogin)));
        //test
        //return await Task.Run(() =>Ok(new { id = "1" });
      }
      catch (Exception e)
      {
        return await Task.Run(() =>BadRequest(e.Message));
      }
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("unimed")]
    public  string unimed()
    {
      try
      {
        service.GetUnimedAsync("testeinfra", "unimed");
        return "";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("altercontract/{idperson}")]
    public async Task<string> AlterContract(string idperson)
    {
      return await Task.Run(() =>service.AlterContract(idperson));
    }

    #endregion

  }
}
