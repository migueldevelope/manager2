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
  /// Controlador do Checkpoint
  /// </summary>
  [Produces("application/json")]
  [Route("checkpoint")]
  public class CheckpointController : DefaultController
  {
    private readonly IServiceCheckpoint service;

    #region Constructor
    /// <summary>
    /// Contrutor do controlador
    /// </summary>
    /// <param name="_service">Servico do checkpoint</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public CheckpointController(IServiceCheckpoint _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Checkpoint
    /// <summary>
    /// Listar pendências de checkpoint para gestor
    /// </summary>
    /// <param name="idmanager">Identificador do gestor</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do colaborador</param>
    /// <returns>Lista de pendência de checkpoint</returns>
    [Authorize]
    [HttpGet]
    [Route("listwaitmanager/{idmanager}")]
    public async Task<List<ViewListCheckpoint>> ListWaitManager(string idmanager,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListWaitManager(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }
    /// <summary>
    /// Listar status de checkpoint para colaborador
    /// </summary>
    /// <param name="idperson">Identificação do colaborador</param>
    /// <returns>Objeto de lista de checkpoint</returns>
    [Authorize]
    [HttpGet]
    [Route("listwaitperson/{idperson}")]
    public async Task<ViewListCheckpoint> ListWaitPerson(string idperson)
    {
      return await service.ListWaitPerson(idperson);
    }
    /// <summary>
    /// Inclusão de novo checkpoint
    /// </summary>
    /// <param name="idperson">Identificador do colaborador</param>
    /// <returns>Objeto de visibilidade de checkpoint</returns>
    [Authorize]
    [HttpPost]
    [Route("new/{idperson}")]
    public async Task<ViewListCheckpoint> NewCheckpoint(string idperson)
    {
      return await service.NewCheckpoint(idperson);
    }
    /// <summary>
    /// Buscar checkpoint para manutenção
    /// </summary>
    /// <param name="id">Identificador do checkpoint</param>
    /// <returns>Objeto de manuten~ção do checkpoint</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudCheckpoint> GetCheckpoint(string id)
    {
      return await service.GetCheckpoint(id);
    }
    /// <summary>
    /// Alterar objeto de checkpoint
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> UpdateCheckpoint([FromBody]ViewCrudCheckpoint view)
    {
      return Ok(await  service.UpdateCheckpoint(view));
    }
    /// <summary>
    /// Buscar checkpoint finalizado para mostrar no histórico
    /// </summary>
    /// <param name="id">Identificador do checkpoint</param>
    /// <returns>Objeto de manutenção do checkpoint</returns>
    [Authorize]
    [HttpGet]
    [Route("personcheckpointend/{id}")]
    public async Task<ViewCrudCheckpoint> PersonCheckpointEnd(string id)
    {
      return await service.PersonCheckpointEnd(id);
    }
    /// <summary>
    /// Remover um checkpoint
    /// </summary>
    /// <param name="id">Identificador do checkpoint</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<string> DeleteCheckpoint(string id)
    {
      return await service.DeleteCheckpoint(id);
    }
    /// <summary>
    /// Listar checkpoints finalizados
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do colaborador</param>
    /// <returns>Lista de checkpoint finalizados</returns>
    [Authorize]
    [HttpGet]
    [Route("listended")]
    public async Task<List<ViewListCheckpoint>> ListEnded( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEnded(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }
    #endregion

  }
}