using Manager.Core.Interfaces;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ManagerMessages.Controllers
{
  /// <summary>
  /// Controlador para acreditação
  /// </summary>
  [Produces("application/json")]
  [Route("onboarding")]
  public class OnboardingController : Controller
  {
    private readonly IServiceNotification service;

    #region constructor
    /// <summary>
    /// Construtor do controle
    /// </summary>
    /// <param name="_service">Serviço da acreditação</param>
    /// <param name="contextAccessor">Token de autenticação</param>
    public OnboardingController(IServiceNotification _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Onboarding
    /// <summary>
    /// Embarque iniciado mas aguardando ação do colaborador a mais de 5 dias, lista pendências
    /// </summary>
    /// <param name="action">Ação que deve ser executada 0-Listagem, 1-Enviar e-mail, 2-Enviar SMS, 3-Enviar Watts</param>
    /// <returns></returns>
    [HttpPost]
    [Route("employeewaiting")]
    public async Task<IActionResult> EmployeeWaiting(byte action)
    {
      try
      {
        return await Task.Run(() => Ok(service.EmployeeWaiting((EnumActionNotification)action)));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
    /// <summary>
    /// Embarque iniciado mas aguardando ação do colaborador a mais de 5 dias, lista pendências
    /// </summary>
    /// <param name="action">Ação que deve ser executada 0-Listagem, 1-Enviar e-mail, 2-Enviar SMS, 3-Enviar Watts</param>
    /// <returns></returns>
    [HttpPost]
    [Route("managerwaiting")]
    public async Task<IActionResult> ManagerWaiting(byte action)
    {
      try
      {
        return await Task.Run(() => Ok(service.ManagerWaiting((EnumActionNotification)action)));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
    #endregion
  }
}
