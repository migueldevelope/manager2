﻿using System;
using System.Collections.Generic;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Integration;
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

    #region Constructor
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
    #endregion

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
    public ViewIntegrationDashboard GetStatusDashboard()
    {
      return service.GetStatusDashboard();
    }
    /// <summary>
    /// Verificar se tem pendência de integração
    /// </summary>
    /// <returns>Mensagem com pendência de integração ou vazio se estiver tudo resolvido.</returns>
    [Authorize]
    [HttpGet]
    [Route("status")]
    public IActionResult GetStatusIntegration()
    {
      try
      {
        return Ok(service.GetStatusIntegration());
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
    /// <returns>Lista de empresas na integração</returns>
    [Authorize]
    [HttpGet]
    [Route("company/list")]
    public List<ViewListIntegrationCompany> GetCompanyList(int count = 10, int page = 1, string filter = "", bool all = false)
    {
      long total = 0;
      List<ViewListIntegrationCompany> result = service.CompanyList(ref total, count, page, filter, all);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Atualizar o registro de integração da empresa do ANALISA para com o da FOLHA
    /// </summary>
    /// <param name="idintegration">Identificador do registro de integração da empresa da folha de pagamento</param>
    /// <param name="idcompany">Identificador da empresa do ANALISA</param>
    /// <returns>Objeto atualizado</returns>
    [Authorize]
    [HttpPost]
    [Route("company/update/{idintegration}/{idcompany}")]
    public ViewListIntegrationCompany PostCompany(string idintegration, string idcompany)
    {
      return service.CompanyUpdate(idintegration, idcompany);
    }
    /// <summary>
    /// Excluir uma empresa da integração de funcionários
    /// </summary>
    /// <param name="idintegration">Identificador da integração da empresa da folha de pagamento</param>
    /// <returns>Retorna Ok caso exclusão seja bem sucedida</returns>
    [Authorize]
    [HttpDelete]
    [Route("company/delete/{idintegration}")]
    public string DeleteCompany(string idintegration)
    {
      return service.CompanyDelete(idintegration);
    }
    /// <summary>
    /// Listar as empresas
    /// </summary>
    /// <returns>Lista de empresas</returns>
    [Authorize]
    [HttpGet]
    [Route("company/list/root")]
    public List<ViewListCompany> GetCompanyRootList()
    {
      long total = 0;
      List<ViewListCompany> result = service.CompanyRootList(ref total);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
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
    /// <returns>Lista de estabelecimentos na integração</returns>
    [Authorize]
    [HttpGet]
    [Route("establishment/list")]
    public List<ViewListIntegrationEstablishment> GetEstablishmentList(int count = 10, int page = 1, string filter = "", bool all = false)
    {
      long total = 0;
      var result = service.EstablishmentList(ref total, count, page, filter, all);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Atualizar o registro de integração do estabelecimento do ANALISA para com o da FOLHA
    /// </summary>
    /// <param name="idintegration">Identificador do registro de integração do estabelecimento da folha de pagamento</param>
    /// <param name="idestablishment">Identificador do estabelecimento no ANALISA</param>
    /// <returns>Objeto atualizado</returns>
    [Authorize]
    [HttpPost]
    [Route("establishment/update/{idintegration}/{idestablishment}")]
    public ViewListIntegrationEstablishment PostEstablishment(string idintegration, string idestablishment)
    {
      return service.EstablishmentUpdate(idintegration, idestablishment);
    }
    /// <summary>
    /// Excluir um estabelecimento da integração de funcionários
    /// </summary>
    /// <param name="idintegration">Identificador da integração do estabelecimento da folha de pagamento</param>
    /// <returns>Retorna Ok caso exclusão seja bem sucedida</returns>
    [Authorize]
    [HttpDelete]
    [Route("establishment/delete/{idintegration}")]
    public string EstablishmentCompany(string idintegration)
    {
      return service.EstablishmentDelete(idintegration);
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
    /// <returns>Lista de cargos na integração</returns>
    [Authorize]
    [HttpGet]
    [Route("occupation/list")]
    public List<ViewListIntegrationOccupation> GetOccupationList(int count = 10, int page = 1, string filter = "", bool all = false)
    {
      long total = 0;
      var result = service.OccupationList(ref total, count, page, filter, all);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Atualizar o registro de integração do cargo do ANALISA para com o da FOLHA
    /// </summary>
    /// <param name="idintegration">Identificador do registro de integração do cargo da folha de pagamento</param>
    /// <param name="idoccupation">Identificador do cargo no ANALISA</param>
    /// <returns>Objeto atualizado</returns>
    [Authorize]
    [HttpPost]
    [Route("occupation/update/{idintegration}/{idoccupation}")]
    public ViewListIntegrationOccupation PostOccupationUpdate(string idintegration, string idoccupation)
    {
      return service.OccupationUpdate(idintegration, idoccupation);
    }
    /// <summary>
    /// Dividir o cargo da folha por centro de custo
    /// </summary>
    /// <param name="idintegration">Identificador do registro de integração do cargo da folha de pagamento</param>
    /// <returns>Objeto atualizado</returns>
    [Authorize]
    [HttpPost]
    [Route("occupation/split/{idintegration}")]
    public string PostOccupationSplit(string idintegration)
    {
      return service.OccupationSplit(idintegration);
    }
    /// <summary>
    /// Juntar o cargo da folha que estava separado
    /// </summary>
    /// <param name="idintegration">Identificador do registro de integração do cargo da folha de pagamento</param>
    /// <returns>Objeto atualizado</returns>
    [Authorize]
    [HttpPost]
    [Route("occupation/join/{idintegration}")]
    public string PostOccupationJoin(string idintegration)
    {
      return service.OccupationJoin(idintegration);
    }
    /// <summary>
    /// Excluir um cargo da integração de funcionários
    /// </summary>
    /// <param name="idintegration">Identificador do cargo da folha de pagamento</param>
    /// <returns>Retorna Ok caso exclusão seja bem sucedida</returns>
    [Authorize]
    [HttpDelete]
    [Route("occupation/delete/{idintegration}")]
    public string DeleteOccupation(string idintegration)
    {
      return service.OccupationDelete(idintegration);
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
    /// <returns>Lista de escolaridades na integração</returns>
    [Authorize]
    [HttpGet]
    [Route("schooling/list")]
    public List<ViewListIntegrationSchooling> GetSchoolingList(int count = 10, int page = 1, string filter = "", bool all = false)
    {
      long total = 0;
      var result = service.SchoolingList(ref total, count, page, filter, all);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Atualizar o registro de integração da escolaridade do ANALISA para com o da FOLHA
    /// </summary>
    /// <param name="idintegration">Identificador do registro de integração da escolaridade da folha de pagamento</param>
    /// <param name="idschooling">Identificador da escolaridade do ANALISA</param>
    /// <returns>Objeto atualizado</returns>
    [Authorize]
    [HttpPost]
    [Route("schooling/update/{idintegration}/{idschooling}")]
    public ViewListIntegrationSchooling PostSchooling(string idintegration, string idschooling)
    {
      return service.SchoolingUpdate(idintegration, idschooling);
    }
    /// <summary>
    /// Excluir uma escolaridade da integração de funcionários
    /// </summary>
    /// <param name="idintegration">Identificador da integração da escolaridade da folha de pagamento</param>
    /// <returns>Retorna Ok caso exclusão seja bem sucedida</returns>
    [Authorize]
    [HttpDelete]
    [Route("schooling/delete/{idintegration}")]
    public string SchoolingCompany(string idintegration)
    {
      return service.SchoolingDelete(idintegration);
    }
    #endregion

    #region ProcessLevelTwo
    /// <summary>
    /// Listar de subprocessos do cliente
    /// </summary>
    /// <returns>Lista de subprocessos</returns>
    [Authorize]
    [HttpGet]
    [Route("processleveltwo/list")]
    public List<ViewListProcessLevelTwo> GetProcessLevelTwoList()
    {
      long total = 0;
      var result = service.ProcessLevelTwoList(ref total);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    #endregion

    #region Skill
    /// <summary>
    /// Atualizar o registro de integração da escolaridade do ANALISA para com o da FOLHA
    /// </summary>
    /// <param name="view">Competência para validar</param>
    /// <returns>Objeto atualizado</returns>
    [Authorize]
    [HttpPost]
    [Route("skill")]
    public ViewCrudSkill PostSkill([FromBody] ViewCrudSkill view)
    {
      return service.IntegrationSkill(view);
    }
    #endregion

    #region Occupation Profile
    /// <summary>
    /// Atualizar o cargo e o mapa do cargo através de integração com o ANALISA
    /// </summary>
    /// <param name="view">Cargo para validar</param>
    /// <returns>Objeto atualizado</returns>
    [Authorize]
    [HttpPost]
    [Route("profile")]
    public ViewIntegrationProfileOccupation PostOccupation([FromBody] ViewIntegrationProfileOccupation view)
    {
      return service.IntegrationProfile(view);
    }
    /// <summary>
    /// Listar todos os cargos ativos para exportação
    /// </summary>
    /// <returns>Objeto atualizado</returns>
    [Authorize]
    [HttpGet]
    [Route("occupations")]
    public List<ViewListOccupation> GetExportOccupation()
    {
      return service.GetExportOccupations();
    }
    /// <summary>
    /// Retorna o mapa do cargo
    /// </summary>
    /// <param name="id">Identificador do mapa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("occupations/{id}")]
    public ViewMapOccupation GetExportOccupation(string id)
    {
      return service.GetExportOccupation(id);
    }
    /// <summary>
    /// Retorna os grupos ocupacionais por ordem de importância
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("exportgroups")]
    public List<ViewListGroup> GetExportGroups()
    {
      return service.GetExportGroups();
    }
    #endregion

  }
}
