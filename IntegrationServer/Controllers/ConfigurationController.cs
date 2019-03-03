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
  [Route("configuration")]
  public class ConfigurationController : Controller
  {
    private readonly IServiceIntegration service;
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
    /// <summary>
    /// Ler todos parâmetros de integração
    /// </summary>
    /// <returns>View dos parâmetros de integração</returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(ViewCrudIntegrationParameter), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetParameter()
    {
      try
      {
        return Ok(service.GetIntegrationParameter());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
    /// <summary>
    /// Atualizar os parâmetros de integração
    /// </summary>
    /// <param name="view">View da integração</param>
    /// <returns>View da integração atualizado</returns>
    [Authorize]
    [HttpPost]
    [Route("mode")]
    [ProducesResponseType(typeof(ViewCrudIntegrationParameter), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SetParameterMode([FromBody]ViewCrudIntegrationParameter view)
    {
      try
      {
        return Ok(service.SetIntegrationParameter(view));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
