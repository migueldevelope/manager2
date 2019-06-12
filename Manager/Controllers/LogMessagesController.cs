using System.Collections.Generic;
using System.Threading.Tasks;
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
  public class LogMessagesController : DefaultController
  {
    private readonly IServiceLogMessages service;

    #region Constructor
    /// <summary>
    /// Construtor do controle
    /// </summary>
    /// <param name="_service">Serviço da Mensageria</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public LogMessagesController(IServiceLogMessages _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
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
    public async Task<List<ViewListLogMessages>> ListPerson(string id,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListLogMessages> result = service.ListPerson(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return  result;
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
    public async Task<List<ViewListLogMessages>> ListManager(string id,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListLogMessages> result = service.ListManager(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return  result;
    }
    /// <summary>
    /// Nova mensagem
    /// </summary>
    /// <param name="view">Objeto de manteunção do log de mensagens</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("new")]
    public async Task<IActionResult> New([FromBody]ViewCrudLogMessages view)
    {
      return Ok( service.New(view));
    }
    /// <summary>
    /// Buscar objeto para manutenção
    /// </summary>
    /// <param name="id">Identificador do log de mensageria</param>
    /// <returns>Objeto de manutenção da mensageria</returns>
    [Authorize]
    [HttpGet]
    [Route("get")]
    public async Task<ViewCrudLogMessages> Get(string id)
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
    public async Task<IActionResult> Update([FromBody]ViewCrudLogMessages view)
    {
      return Ok( service.Update(view));
    }
    /// <summary>
    /// Excluir uma mensagem
    /// </summary>
    /// <param name="id">Identificador da mensagem</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      return Ok( service.Delete(id));
    }
    #endregion

  }
}