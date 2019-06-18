using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IntegrationServer.Controllers
{
  /// <summary>
  /// Controlador de Configuração da Integração
  /// </summary>
  [Produces("application/json")]
  [Route("integrationserver/configuration")]
  public class ConfigurationController : Controller
  {
    private readonly IServiceIntegration service;

    #region Constructor
    /// <summary>
    /// Inicializador do controlador de configuração da integração
    /// </summary>
    /// <param name="_service">Serviço de Integração</param>
    /// <param name="contextAccessor">Token de autenticação</param>
    public ConfigurationController(IServiceIntegration _service, IHttpContextAccessor contextAccessor)
    {
      try
      {
        service = _service;
        service.SetUser(contextAccessor);
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #region Parameter
    /// <summary>
    /// Ler todos parâmetros de integração
    /// </summary>
    /// <returns>View dos parâmetros de integração</returns>
    [Authorize]
    [HttpGet]
    public ViewCrudIntegrationParameter GetParameter()
    {
      return service.GetIntegrationParameter();
    }
    /// <summary>
    /// Atualizar os parâmetros de integração
    /// </summary>
    /// <param name="view">View da integração</param>
    /// <returns>View da integração atualizado</returns>
    [Authorize]
    [HttpPost]
    [Route("mode")]
    public ViewCrudIntegrationParameter SetParameterMode([FromBody]ViewCrudIntegrationParameter view)
    {
      return service.SetIntegrationParameter(view);
    }
    #endregion

  }
}
