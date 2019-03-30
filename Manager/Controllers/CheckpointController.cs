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
  /// Controlador do Checkpoint
  /// </summary>
  [Produces("application/json")]
  [Route("checkpoint")]
  public class CheckpointController : Controller
  {
    private readonly IServiceCheckpoint service;

    #region Constructor
    /// <summary>
    /// Contrutor do controlador
    /// </summary>
    /// <param name="_service">Servico do checkpoint</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public CheckpointController(IServiceCheckpoint _service, IHttpContextAccessor contextAccessor)
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
    public List<ViewListCheckpoint> ListCheckpointWait(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListWaitManager(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Listar status de checkpoint para colaborador
    /// </summary>
    /// <param name="idperson">Identificação do colaborador</param>
    /// <returns>Objeto de lista de checkpoint</returns>
    [Authorize]
    [HttpGet]
    [Route("listwaitperson/{idperson}")]
    public ViewListCheckpoint ListWaitPerson(string idperson)
    {
      return service.ListWaitPerson(idperson);
    }
    /// <summary>
    /// Inclusão de novo checkpoint
    /// </summary>
    /// <param name="idperson">Identificador do colaborador</param>
    /// <returns>Objeto de visibilidade de checkpoint</returns>
    [Authorize]
    [HttpPost]
    [Route("new/{idperson}")]
    public ViewListCheckpoint NewCheckpoint(string idperson)
    {
      return service.NewCheckpoint(idperson);
    }
    /// <summary>
    /// Buscar checkpoint para manutenção
    /// </summary>
    /// <param name="id">Identificador do checkpoint</param>
    /// <returns>Objeto de manuten~ção do checkpoint</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public ViewCrudCheckpoint GetCheckpoint(string id)
    {
      return service.GetCheckpoint(id);
    }
    /// <summary>
    /// Alterar objeto de checkpoint
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public IActionResult UpdateCheckpoint([FromBody]ViewCrudCheckpoint view)
    {
      return Ok(service.UpdateCheckpoint(view));
    }
    /// <summary>
    /// Buscar checkpoint finalizado para mostrar no histórico
    /// </summary>
    /// <param name="idperson">Identificador da pessoa</param>
    /// <returns>Objeto de manutenção do checkpoint</returns>
    [Authorize]
    [HttpGet]
    [Route("personcheckpointend/{idperson}")]
    public ViewCrudCheckpoint PersonCheckpointEnd(string idperson)
    {
      return service.PersonCheckpointEnd(idperson);
    }
    /// <summary>
    /// Remover um checkpoint
    /// </summary>
    /// <param name="idcheckpoint">Identificador do checkpoint</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{idcheckpoint}")]
    public string RemoveCheckpoint(string idcheckpoint)
    {
      return service.RemoveCheckpoint(idcheckpoint);
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
    [Route("getlistexclud")]
    public List<ViewListCheckpoint> GetListExclud(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetListExclud(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    #endregion

    #region old
    [Authorize]
    [HttpPost]
    [Route("old/new/{idperson}")]
    public Checkpoint PostOld([FromBody]Checkpoint checkpoint, string idperson)
    {
      return service.NewCheckpointOld(checkpoint, idperson);
    }

    [Authorize]
    [HttpPut]
    [Route("old/update/{idperson}")]
    public string PutOld([FromBody]Checkpoint checkpoint, string idperson)
    {
      return service.UpdateCheckpointOld(checkpoint, idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listend/{idmanager}")]
    public List<Checkpoint> ListEndOld(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCheckpointsEndOld(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/list/{idmanager}")]
    public List<Checkpoint> ListOld(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCheckpointsWaitOld(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/get/{id}")]
    public Checkpoint GetCheckpointOld(string id)
    {
      return service.GetCheckpointsOld(id);
    }

    [Authorize]
    [HttpGet]
    [Route("old/getlistexclud")]
    public List<Checkpoint> GetListExcludOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetListExcludOld(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpDelete]
    [Route("old/delete/{idperson}")]
    public string RemoveCheckpointOld(string idperson)
    {
      return service.RemoveCheckpointOld(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("old/personcheckpointend/{idperson}")]
    public Checkpoint PersonCheckpointEndOld(string idperson)
    {
      return service.PersonCheckpointEndOld(idperson);
    }


    [Authorize]
    [HttpGet]
    [Route("old/listcheckpointswaitperson/{idperson}")]
    public Checkpoint ListCheckpointsWaitPersonOld(string idperson)
    {
      return service.ListCheckpointsWaitPersonOld(idperson);
    }
    #endregion

  }
}