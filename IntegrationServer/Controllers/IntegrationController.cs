using System;
using System.Collections.Generic;
using Manager.Core.Interfaces;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationServer.InfraController
{
  /// <summary>
  /// Controlador para executar a integração de funcionários
  /// </summary>
  [Produces("application/json")]
  [Route("integration")]
  public class IntegrationController : Controller
  {
    private readonly IServiceIntegration service;
    /// <summary>
    /// Inicializador do controlador de integração de funcionários
    /// </summary>
    /// <param name="_service">Interface de inicialização</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public IntegrationController(IServiceIntegration _service, IHttpContextAccessor contextAccessor)
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

    #region Dashboard
    /// <summary>
    /// Retornar o status da integração de funcionários
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("dashboard")]
    [ProducesResponseType(typeof(ViewIntegrationDashboard), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetStatusDashboard()
    {
      try
      {
        return Ok(service.GetStatusDashboard());
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    #endregion

    #region Company
    /// <summary>
    /// Listar as empresas identificadas na integração
    /// </summary>
    /// <param name="count">Opcional: quantidade de registros para retornar</param>
    /// <param name="page">Opcional: número da página para retornar</param>
    /// <param name="filter">Opcional: filtro no nome da empresa</param>
    /// <param name="all">Opcional: trazer todos os registros ou só registros com problema</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("company/list")]
    [ProducesResponseType(typeof(List<ViewListIntegrationCompany>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetCompanyList(int count = 10, int page = 1, string filter = "", bool all = false)
    {
      try
      {
        long total = 0;
        var result = service.CompanyList(ref total, count, page, filter, all);
        Response.Headers.Add("x-total-count", total.ToString());
        return Ok(result);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    /// <summary>
    /// Atualizar o registro de integração da empresa do ANALISA para com o da FOLHA
    /// </summary>
    /// <param name="idintegration">Identificador do registro de integração da empresa da folha de pagamento</param>
    /// <param name="idcompany">Identificador da empresa do ANALISA</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("company/update/{idintegration}/{idcompany}")]
    [ProducesResponseType(typeof(ViewListIntegrationCompany), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PostCompany(string idintegration, string idcompany)
    {
      try
      {
        return Ok(service.CompanyUpdate(idintegration, idcompany));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    /// <summary>
    /// Excluir uma empresa da integração de funcionários
    /// </summary>
    /// <param name="idintegration">Identificador da integração da empresa da folha de pagamento</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("company/delete/{idintegration}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteCompany(string idintegration)
    {
      try
      {
        return Ok(service.CompanyDelete(idintegration));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    #endregion

    #region Schooling
    /// <summary>
    /// Listar as escolaridades não identificadas na integração
    /// </summary>
    /// <param name="count">Opcional: quantidade de registros para retornar</param>
    /// <param name="page">Opcional: número da página para retornar</param>
    /// <param name="filter">Opcional: filtro no nome da escolaridade</param>
    /// <param name="all">Opcional: trazer todos os registros ou só registros com problema</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("schooling/list")]
    [ProducesResponseType(typeof(List<ViewListIntegrationSchooling>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetSchoolingList(int count = 10, int page = 1, string filter = "", bool all = false)
    {
      try
      {
        long total = 0;
        var result = service.SchoolingList(ref total, count, page, filter, all);
        Response.Headers.Add("x-total-count", total.ToString());
        return Ok(result);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    /// <summary>
    /// Atualizar o registro de integração da escolaridade do ANALISA para com o da FOLHA
    /// </summary>
    /// <param name="idintegration">Identificador do registro de integração da escolaridade da folha de pagamento</param>
    /// <param name="idschooling">Identificador da escolaridade do ANALISA</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("schooling/update/{idintegration}/{idschooling}")]
    [ProducesResponseType(typeof(ViewListIntegrationSchooling), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PostSchooling(string idintegration, string idschooling)
    {
      try
      {
        return Ok(service.SchoolingUpdate(idintegration, idschooling));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    /// <summary>
    /// Excluir uma escolaridade da integração de funcionários
    /// </summary>
    /// <param name="idintegration">Identificador da integração da escolaridade da folha de pagamento</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("schooling/delete/{idintegration}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SchoolingCompany(string idintegration)
    {
      try
      {
        return Ok(service.SchoolingDelete(idintegration));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    #endregion

    #region Establishment
    /// <summary>
    /// Listar os estabelecimentos não identificados na integração
    /// </summary>
    /// <param name="count">Opcional: quantidade de registros para retornar</param>
    /// <param name="page">Opcional: número da página para retornar</param>
    /// <param name="filter">Opcional: filtro no nome do estabelecimento</param>
    /// <param name="all">Opcional: trazer todos os registros ou só registros com problema</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("establishment/list")]
    [ProducesResponseType(typeof(List<ViewListIntegrationEstablishment>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetEstablishmentList(int count = 10, int page = 1, string filter = "", bool all = false)
    {
      try
      {
        long total = 0;
        var result = service.EstablishmentList(ref total, count, page, filter, all);
        Response.Headers.Add("x-total-count", total.ToString());
        return Ok(result);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    /// <summary>
    /// Atualizar o registro de integração do estabelecimento do ANALISA para com o da FOLHA
    /// </summary>
    /// <param name="idintegration">Identificador do registro de integração do estabelecimento da folha de pagamento</param>
    /// <param name="idestablishment">Identificador do estabelecimento no ANALISA</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("establishment/update/{idintegration}/{idestablishment}")]
    [ProducesResponseType(typeof(ViewListIntegrationEstablishment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PostEstablishment(string idintegration, string idestablishment)
    {
      try
      {
        return Ok(service.EstablishmentUpdate(idintegration, idestablishment));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    /// <summary>
    /// Excluir um estabelecimento da integração de funcionários
    /// </summary>
    /// <param name="idintegration">Identificador da integração do estabelecimento da folha de pagamento</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("establishment/delete/{idintegration}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult EstablishmentCompany(string idintegration)
    {
      try
      {
        return Ok(service.EstablishmentDelete(idintegration));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    #endregion

    #region Occupation
    /// <summary>
    /// Listar os cargos não identificados na integração
    /// </summary>
    /// <param name="count">Opcional: quantidade de registros para retornar</param>
    /// <param name="page">Opcional: número da página para retornar</param>
    /// <param name="filter">Opcional: filtro no nome do cargo</param>
    /// <param name="all">Opcional: trazer todos os registros ou só registros com problema</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("occupation/list")]
    [ProducesResponseType(typeof(List<ViewListIntegrationOccupation>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetOccupationList(int count = 10, int page = 1, string filter = "", bool all = false)
    {
      try
      {
        long total = 0;
        var result = service.OccupationList(ref total, count, page, filter, all);
        Response.Headers.Add("x-total-count", total.ToString());
        return Ok(result);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    /// <summary>
    /// Atualizar o registro de integração do cargo do ANALISA para com o da FOLHA
    /// </summary>
    /// <param name="idintegration">Identificador do registro de integração do cargo da folha de pagamento</param>
    /// <param name="idoccupation">Identificador do cargo no ANALISA</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("occupation/update/{idintegration}/{idoccupation}")]
    [ProducesResponseType(typeof(ViewListIntegrationOccupation), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PostOccupation(string idintegration, string idoccupation)
    {
      try
      {
        return Ok(service.OccupationUpdate(idintegration, idoccupation));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    /// <summary>
    /// Excluir um cargo da integração de funcionários
    /// </summary>
    /// <param name="idintegration">Identificador do cargo da folha de pagamento</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("occupation/delete/{idintegration}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult OccupationCompany(string idintegration)
    {
      try
      {
        return Ok(service.OccupationDelete(idintegration));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    #endregion

  }
}
