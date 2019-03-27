using System.Collections.Generic;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador de Mensageria
  /// </summary>
  [Produces("application/json")]
  [Route("logmessages")]
  public class LogMessagesController : Controller
  {
    private readonly IServiceLogMessages service;

    #region Constructor
    /// <summary>
    /// Construtor do controle
    /// </summary>
    /// <param name="_service">Serviço da Mensageria</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public LogMessagesController(IServiceLogMessages _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Messageria
    /// <summary>
    /// Listar mensagens do colaborador
    /// </summary>
    /// <param name="id">Identificação do colaborador</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para mensagem</param>
    /// <returns>Lista de mensagens para o colaborador</returns>
    [Authorize]
    [HttpGet]
    [Route("listperson/{id}")]
    public List<ViewListLogMessages> ListPerson(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListLogMessages> result = service.ListPerson(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Listar as mensagens da equipe do gestor
    /// </summary>
    /// <param name="id">Identificador do gestor</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para mensagem</param>
    /// <returns>Lista de mensagens para o gestor</returns>
    [Authorize]
    [HttpGet]
    [Route("listmanager/{id}")]
    public List<ViewListLogMessages> ListManager(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListLogMessages> result = service.ListManager(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Nova mensagem
    /// </summary>
    /// <param name="view">Objeto de manteunção do log de mensagens</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("new")]
    public IActionResult Post([FromBody]ViewCrudLogMessages view)
    {
      return Ok(service.New(view));
    }
    /// <summary>
    /// Buscar objeto para manutenção
    /// </summary>
    /// <param name="id">Identificador do log de mensageria</param>
    /// <returns>Objeto de manutenção da mensageria</returns>
    [Authorize]
    [HttpGet]
    [Route("get")]
    public ViewCrudLogMessages List(string id)
    {
      return service.Get(id);
    }
    /// <summary>
    /// Alterar uma mensagem do log de mensagerias
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody]ViewCrudLogMessages view)
    {
      return Ok(service.Update(view));
    }
    /// <summary>
    /// Excluir uma mensagem
    /// </summary>
    /// <param name="id">Identificador da mensagem</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public IActionResult Delete(string id)
    {
      return Ok(service.Remove(id));
    }
    #endregion

    #region Old
    [HttpPost]
    [Route("old/new")]
    public string Post([FromBody]LogMessages view)
    {
      return service.New(view);
    }

    [Authorize]
    [HttpGet]
    [Route("old/list")]
    public List<LogMessages> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/listmanager/{id}")]
    public List<LogMessages> ListManagerOld(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListManagerOld(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/listperson/{id}")]
    public List<LogMessages> ListPersonOld(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPersonOld(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/get")]
    public LogMessages ListOld(string id)
    {
      return service.GetOld(id);
    }

    [Authorize]
    [HttpPut]
    [Route("old/update")]
    public string Update([FromBody]LogMessages view)
    {
      return service.Update(view);
    }

    [Authorize]
    [HttpDelete]
    [Route("old/delete/{id}")]
    public string DeleteOld(string id)
    {
      return service.RemoveOld(id);
    }
    #endregion

  }
}