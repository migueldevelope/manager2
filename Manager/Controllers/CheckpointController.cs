using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Interfaces;
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
    [Authorize]
    [HttpGet]
    [Route("list/{idmanager}")]
    public List<Checkpoint> List(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCheckpointsWaitOld(idmanager, ref total, filter, count, page);
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