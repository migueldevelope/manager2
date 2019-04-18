using System;
using System.Collections.Generic;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador de Pessoas
  /// </summary>
  [Produces("application/json")]
  [Route("person")]
  public class PersonController : Controller
  {
    private readonly IServicePerson service;

    #region Constructor
    /// <summary>
    /// Contrutor do controlador
    /// </summary>
    /// <param name="_service">Serviço da pessoa</param>
    /// <param name="contextAccessor">Autorização</param>
    public PersonController(IServicePerson _service, IHttpContextAccessor contextAccessor)
    {
      try
      {
        service = _service;
        service.SetUser(contextAccessor);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Person
    /// <summary>
    /// Listar pessoas da base de dados
    /// </summary>
    /// <param name="type">Tipo do usuário que está fazendo a consulta</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome da pessoa</param>
    /// <returns>Lista de pessoas da tela de manutenção</returns>
    [Authorize]
    [HttpGet]
    [Route("list/{type}")]
    public List<ViewListPersonCrud> List(EnumTypeUser type, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter, type);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Buscar informações da pessoa para alteração
    /// </summary>
    /// <param name="id">Identificador da pessoa</param>
    /// <returns>Objeto de alteração da pessoa</returns>
    [Authorize]
    [HttpGet]
    [Route("edit/{id}")]
    public ViewCrudPerson Get(string id)
    {
      return service.Get(id);
    }
    /// <summary>
    /// Incluir uma nova pessoa
    /// </summary>
    /// <param name="view">Objeto de manutenção da pessoa</param>
    /// <returns>Objeto da pessoa incluída</returns>
    [Authorize]
    [HttpPost]
    [Route("new")]
    public ViewCrudPerson New([FromBody] ViewCrudPerson view)
    {
      return service.New(view);
    }
    /// <summary>
    /// Alterar uma pessoa
    /// </summary>
    /// <param name="view">Objeto de manutenção da pessoa</param>
    /// <returns>Objeto da pessoa alterada</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody] ViewCrudPerson view)
    {
      return Ok(service.Update(view));
    }

    /// <summary>
    /// Lista time de gestor
    /// </summary>
    /// <param name="idmanager">Identificador gestor</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("directteam/{idmanager}")]
    public List<ViewListPersonTeam> ListTeam(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTeam(ref total, idmanager, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    #endregion

    #region SalaryScale
    /// <summary>
    /// Lista as tabelas salarias filtrando o cargo
    /// </summary>
    /// <param name="idoccupation">Identificador cargo</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listsalaryscale/{idoccupation}")]
    public List<ViewListSalaryScalePerson> ListSalaryScale(string idoccupation)
    {
      return service.ListSalaryScale(idoccupation);
    }
    #endregion

  }
}