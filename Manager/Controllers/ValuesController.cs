using System.Collections.Generic;
using Manager.Core.Interfaces;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// Controle para verficação de API ativa
  /// </summary>
  [Route("api/[controller]")]
  public class ValuesController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    public IServiceOnBoarding service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceOnboarding"></param>
    /// <param name="contextAccessor"></param>
    public ValuesController(IServiceOnBoarding serviceOnboarding, IHttpContextAccessor contextAccessor)
    {
      service = serviceOnboarding;
      service.SetUser(contextAccessor);
    }

    /// <summary>
    /// Método único para demonstração de API REST ativa
    /// </summary>
    /// <returns>String com a versão da API REST</returns>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      long total = 0;
      //service.ListExcluded(ref total, "", 1, 1);
      return new string[] { "version", "0.000000036" };
    }

    /// <summary>
    /// Test mail
    /// </summary>
    /// <returns></returns>
    [Route("mail")]
    [HttpGet]
    public IEnumerable<string> Mail()
    {
      service.MailTest();
      return new string[] { "test", "0.000000035" };
    }

  }
}
