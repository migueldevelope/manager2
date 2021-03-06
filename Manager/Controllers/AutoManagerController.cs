﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tools;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador para auto gestão de equipe
  /// </summary>
  [Produces("application/json")]
  [Route("automanager")]
  public class AutoManagerController : DefaultController
  {
    private readonly IServiceAutoManager service;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Serviço de auto gestão de equipe</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public AutoManagerController(IServiceAutoManager _service, IHttpContextAccessor contextAccessor): base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Auto Gestão
    /// <summary>
    /// Retornar lista de aprovações para o gestor
    /// </summary>
    /// <param name="idmanager">Identificador do gestor</param>
    /// <returns>Lista de aprovações pendentes</returns>
    [Authorize]
    [HttpGet]
    [Route("{idmanager}/listapproved")]
    public async Task<List<ViewAutoManager>> ListApproved(string idmanager)
    {
      return await Task.Run(() =>service.ListApproved(idmanager));
    }
    /// <summary>
    /// Listar colaboradores sem gestão
    /// </summary>
    /// <param name="idmanager">Identificação do gestor</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do usuário</param>
    /// <returns>Lista de colaboradores sem gestão</returns>
    [Authorize]
    [HttpGet]
    [Route("{idmanager}/list")]
    public async Task<List<ViewAutoManagerPerson>> List(string idmanager,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(idmanager, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }
    /// <summary>
    /// Alterar o gestor do colaborador
    /// </summary>
    /// <param name="view">Informações do gestor</param>
    /// <param name="idperson">Identificador da pessoa</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("{idperson}/new")]
    public async Task<IActionResult> New([FromBody]ViewManager view, string idperson)
    {
      service.SetManagerPerson(view, idperson);
      return await Task.Run(() =>Ok("Auto manager added!"));
    }
    /// <summary>
    /// Aprovar o gestor do colaborador
    /// </summary>
    /// <param name="view">Workflow para aprovação</param>
    /// <param name="idperson">Identificador do colaborador</param>
    /// <param name="idmanager">Identificador do gestor</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("{idperson}/approved/{idmanager}")]
    public async Task<string> Approved([FromBody]ViewWorkflow view, string idperson, string idmanager)
    {
      return await Task.Run(() =>service.Approved(view, idperson, idmanager));
    }
    /// <summary>
    /// Não aprovar o colaborador para o gestor
    /// </summary>
    /// <param name="view">Workflow para negação</param>
    /// <param name="idperson">Identificador do colaborador</param>
    /// <param name="idmanager">Identificador do gestor</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("{idperson}/disapproved/{idmanager}")]
    public async Task<string> Disapproved([FromBody]ViewWorkflow view, string idperson, string idmanager)
    {
      return await Task.Run(() =>service.Disapproved(view, idperson, idmanager));
    }
    /// <summary>
    /// Exclusão do colaborador da equipe
    /// </summary>
    /// <param name="idperson">Identificação do colaborador</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("{idperson}/delete")]
    public async Task<IActionResult> Delete(string idperson)
    {
      service.DeleteManager(idperson);
      return await Task.Run(() =>Ok("Person deleted!"));
    }
    #endregion

    /// <summary>
    /// NÃO UTILIZANDO: Cancelar a solicitação de equipe
    /// </summary>
    /// <param name="idperson">Identificador do colaborador</param>
    /// <param name="idmanager">Identificador do gestor</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("{idperson}/canceled/{idmanager}")]
    public async Task<IActionResult> PutCanceled(string idperson, string idmanager)
    {
      service.Canceled(idperson, idmanager);
      return await Task.Run(() =>Ok("Auto manager canceled!"));
    }

  }
}