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

    [AllowAnonymous]
    [HttpPost]
    [Route("unimed")]
    public ObjectResult PostUnimed()
    {
      try
      {
        string username = "apiadv";
        string password = "ad6072616b467db08f60918070e03622" + DateTime.Now.ToString("ddMMyyyyHHmm");
        string password2 = Tools.EncryptServices.GetMD5HashTypeTwo(password).ToLower();

        using (var client = new HttpClient())
        {

          client.DefaultRequestHeaders.Add("Autorization", "Basic " + password);
          client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(new UTF8Encoding().GetBytes(username + ":" + password2)));
          client.BaseAddress = new Uri("https://apip1.unimednordesters.com.br");

          var data = new
          {
            channel = "apiadv",
            parametros = new
            {
              usuario = "toten",
              senha = "unimed"
            }
          };
          var json = JsonConvert.SerializeObject(data);
          var content = new StringContent(json);
          content.Headers.ContentType.MediaType = "application/json";
          client.DefaultRequestHeaders.Add("ContentType", "application/json");
          var result = client.PostAsync("/", content).Result;
          var resultContent = result.Content.ReadAsStringAsync().Result;
        }


        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

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
    #endregion

  }
}
