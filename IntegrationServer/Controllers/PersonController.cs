using System;
using System.Collections.Generic;
using System.Globalization;
using Manager.Core.Business.Integration;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Manager.Views.Integration;
using Manager.Views.Integration.V2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationServer.InfraController
{
  /// <summary>
  /// Controlador para integração de funcionários
  /// </summary>
  [Produces("application/json")]
  [Route("person")]
  public class PersonController : Controller
  {
    private readonly IServiceIntegration service;
    private readonly IServicePerson servicePerson;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Serviço de integração</param>
    /// <param name="_servicePerson">Serviço específico da pessoa</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public PersonController(IServiceIntegration _service, IServicePerson _servicePerson, IHttpContextAccessor contextAccessor)
    {
      try
      {
        service = _service;
        servicePerson = _servicePerson;
        service.SetUser(contextAccessor);
        servicePerson.SetUser(contextAccessor);
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #region Colaborador V2
    /// <summary>
    /// Integração com objeto único do colaborador versão 2
    /// </summary>
    /// <param name="view">Objeto de integração completo do colaborador</param>
    /// <response code="200">Informações sobre a integração do colaborador</response>
    /// <response code="400">Problemas na integração do colaborador</response>
    /// <returns>Objeto de retorno da integração </returns>
    [Authorize]
    [HttpPost]
    [Route("v2/completo")]
    [ProducesResponseType(typeof(ColaboradorV2Retorno), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ColaboradorV2Retorno), StatusCodes.Status400BadRequest)]
    public ObjectResult V2Completo([FromBody]ColaboradorV2Completo view)
    {
      try
      {
        return Ok(service.IntegrationV2(view));
      }
      catch (Exception e)
      {
        return BadRequest(new ColaboradorV2Retorno()
        {
          Mensagem = new List<string> { e.Message },
          Situacao = "Erro"
        });
      }
    }
    /// <summary>
    /// Consulta última posição de integração do colaborador
    /// </summary>
    /// <param name="view">Objeto de identificação do colaborador</param>
    /// <response code="200">Informações sobre a integração do colaborador</response>
    /// <response code="400">Problemas na integração do colaborador</response>
    /// <returns>Objeto de retorno da integração do colaborador</returns>
    [Authorize]
    [HttpPost]
    [Route("v2/consulta")]
    [ProducesResponseType(typeof(ColaboradorV2), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public ObjectResult V2Consulta([FromBody]ColaboradorV2Base view)
    {
      try
      {
        return Ok(service.GetV2(view));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    /// <summary>
    /// Consulta posição de integração do colaborador por identificador
    /// </summary>
    /// <param name="id">Identificação do registro de integração do colaborador</param>
    /// <response code="200">Informações sobre a integração do colaborador</response>
    /// <response code="400">Problemas na integração do colaborador</response>
    /// <returns>Objeto de retorno da integração do colaborador</returns>
    [Authorize]
    [HttpGet]
    [Route("v2/consulta/{id}")]
    [ProducesResponseType(typeof(ColaboradorV2), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public ObjectResult V2ConsultaId(string id)
    {
      try
      {
        return Ok(service.GetV2(id));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    /// <summary>
    /// Demissão do colaborador
    /// </summary>
    /// <param name="view">Objeto de demissão do colaborador</param>
    /// <response code="200">Informações sobre a demissão do colaborador</response>
    /// <response code="400">Problemas na demissão do colaborador</response>
    /// <returns>Objeto de retorno da integração </returns>
    [ProducesResponseType(typeof(ColaboradorV2Retorno), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ColaboradorV2Retorno), StatusCodes.Status200OK)]
    [Authorize]
    [HttpPut]
    [Route("v2/demissao")]
    public ObjectResult DemissaoV2([FromBody]ColaboradorV2Demissao view)
    {
      {
        try
        {
          return Ok(service.IntegrationV2(view));
        }
        catch (Exception e)
        {
          return BadRequest(new ColaboradorV2Retorno()
          {
            Mensagem = new List<string> { e.Message },
            Situacao = "Erro"
          });
        }
      }
    }
    /// <summary>
    /// Lista de colaboradores ativos para demissão por ausência uso exclusivo da FLUID STATE
    /// </summary>
    /// <response code="200">Lista de colaboradores ativos</response>
    /// <response code="400">Problemas na geração da lista</response>
    /// <returns>Objeto de retorno da integração </returns>
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<ColaboradorV2Base>), StatusCodes.Status200OK)]
    [Authorize]
    [HttpGet]
    [Route("v2/ativos")]
    public ObjectResult ActiveV2()
    {
      {
        try
        {
          return Ok(service.GetActiveV2());
        }
        catch (Exception e)
        {
          return BadRequest(e.Message);
        }
      }
    }
    #endregion

    #region PayrollEmployee
    /// <summary>
    /// Atualizar integração de colaborador pendente
    /// </summary>
    /// <param name="id">Identificador do registro de integração do colaborador pendente</param>
    /// <response code="200">Informações sobre a integração do colaborador</response>
    /// <response code="400">Problemas na integração do colaborador</response>
    /// <returns>Mensagem de sucesso ou erro</returns>
    [Authorize]
    [HttpPost]
    [Route("payroll/{id}")]
    [ProducesResponseType(typeof(ColaboradorV2Retorno), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public ObjectResult Payroll(string id)
    {
      try
      {
        return Ok(service.IntegrationPayroll(id));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    #endregion

    #region Person
    /// <summary>
    /// Rertorna usuários com seus contratos filtrados pelo nickname
    /// </summary>
    /// <param name="nickname"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<ViewUserNickName>), StatusCodes.Status200OK)]
    [Authorize]
    [HttpGet]
    [Route("listusernickname/{nickname}")]
    public ObjectResult GetPersonNickName(string nickname)
    {
      {
        try
        {
          return Ok(servicePerson.GetPersonNickName(nickname));
        }
        catch (Exception e)
        {
          return BadRequest(e.Message);
        }
      }
    }
    #endregion
  }
}
