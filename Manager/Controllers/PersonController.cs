using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
  [Route("manager/person")]
  public class PersonController : DefaultController
  {
    private readonly IServicePerson service;

    #region Constructor
    /// <summary>
    /// Contrutor do controlador
    /// </summary>
    /// <param name="_service">Serviço da pessoa</param>
    /// <param name="contextAccessor">Autorização</param>
    public PersonController(IServicePerson _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
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
    public async Task<List<ViewListPersonCrud>> List(EnumTypeUser type,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter, type);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }
    /// <summary>
    /// Buscar informações da pessoa para alteração
    /// </summary>
    /// <param name="id">Identificador da pessoa</param>
    /// <returns>Objeto de alteração da pessoa</returns>
    [Authorize]
    [HttpGet]
    [Route("edit/{id}")]
    public async Task<ViewCrudPerson> Get(string id)
    {
      return await Task.Run(() =>service.Get(id));
    }
    /// <summary>
    /// Incluir uma nova pessoa
    /// </summary>
    /// <param name="view">Objeto de manutenção da pessoa</param>
    /// <returns>Objeto da pessoa incluída</returns>
    [Authorize]
    [HttpPost]
    [Route("new")]
    public async Task<ViewCrudPerson> New([FromBody] ViewCrudPerson view)
    {
      return await Task.Run(() =>service.New(view));
    }
    /// <summary>
    /// Alterar uma pessoa
    /// </summary>
    /// <param name="view">Objeto de manutenção da pessoa</param>
    /// <returns>Objeto da pessoa alterada</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update([FromBody] ViewCrudPerson view)
    {
      return await Task.Run(() =>Ok( service.Update(view)));
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
    public async Task<List<ViewListPersonTeam>> ListTeam(string idmanager,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTeam(ref total, idmanager, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
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
    public async Task<List<ViewListSalaryScalePerson>> ListSalaryScale(string idoccupation)
    {
      return await Task.Run(() =>service.ListSalaryScale(idoccupation));
    }
    #endregion

    #region Person Auxiliar
    /// <summary>
    /// Listar os cargos para manutenção da pessoa
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do cargo</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listoccupation")]
    public async Task<List<ViewListOccupation>> ListOccupation( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOccupation(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }
    /// <summary>
    /// Listar as empresas para manutenção da pessoa
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome da empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listcompany")]
    public async Task<List<ViewListCompany>> ListCompany( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCompany(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }
    /// <summary>
    /// Listar os gestores para manutenção da pessoa
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome da pessoa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listmanager")]
    public async Task<List<ViewListPerson>> ListManager( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListManager(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }
    #endregion

  }
}