using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador da empresa
  /// </summary>
  [Produces("application/json")]
  [Route("objective")]
  public class ObjectiveController : Controller
  {
    private readonly IServiceObjective service;

    #region Constructor
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">Serviço da empresa</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public ObjectiveController(IServiceObjective _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Objective
    /// <summary>
    /// Listar as empresas
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome da empresa</param>
    /// <returns>Lista de empresas cadastradas</returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public async Task<List<ViewListObjective>> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListObjective> result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());

      return await Task.Run(() => result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idkeyresult"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getimpedimentsiniciatives/{idkeyresult}")]
    public async Task<List<ViewCrudImpedimentsIniciatives>> GetImpedimentsIniciatives(string idkeyresult, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetImpedimentsIniciatives(idkeyresult, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Cadastrar uma nova empresa
    /// </summary>
    /// <param name="view">Objeto de cadastro da empresa</param>
    /// <returns></returns>
    [HttpPost]
    [Route("new")]
    public async Task<ViewCrudObjective> Post([FromBody]ViewCrudObjective view)
    {
      return await Task.Run(() => service.New(view));
    }
    /// <summary>
    /// Retorar a empresa para manutenção
    /// </summary>
    /// <param name="id">Identificador da empresa</param>
    /// <returns>Objeto de manutenção da empresa</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudObjective> Get(string id)
    {
      return await Task.Run(() => service.Get(id));
    }
    /// <summary>
    /// Alterar a empresa
    /// </summary>
    /// <param name="view">Objeto de manutenção da empresa</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<ViewCrudObjective> Update([FromBody]ViewCrudObjective view)
    {
      return await Task.Run(() => service.Update(view));
    }
    /// <summary>
    /// Excluir uma empresa
    /// </summary>
    /// <param name="id">Identificação da empresa</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<string> Delete(string id)
    {
      return await Task.Run(() => service.Delete(id));
    }
    #endregion

    #region KeyResult
    /// <summary>
    /// Listar estabelecimentos
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do estabelecimento</param>
    /// <returns>Lista de estabelecimentos</returns>
    [Authorize]
    [HttpGet]
    [Route("listkeyresult")]
    public async Task<List<ViewListKeyResult>> ListKeyResult(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListKeyResult> result = service.ListKeyResult(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Listar estabelecimento de uma empresa
    /// </summary>
    /// <param name="idobjective">Identificador da empresa</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do estabelecimento</param>
    /// <returns>Lista de estabelecimentos da empresa</returns>
    [Authorize]
    [HttpGet]
    [Route("listkeyresult/{idobjective}")]
    public async Task<List<ViewListKeyResult>> ListKeyResult(string idobjective, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListKeyResult> result = service.ListKeyResult(idobjective, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Novo estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("newkeyresult")]
    public async Task<ViewCrudKeyResult> PostKeyResult([FromBody]ViewCrudKeyResult view)
    {
      return await Task.Run(() => service.NewKeyResult(view));
    }
    /// <summary>
    /// Buscar estabelecimento para manutenção
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Objeto de manutenção do estabelecimento</returns>
    [Authorize]
    [HttpGet]
    [Route("getkeyresult/{id}")]
    public async Task<ViewCrudKeyResult> ListKeyResult(string id)
    {
      return await Task.Run(() => service.GetKeyResult(id));
    }
    /// <summary>
    /// Alterar o estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updatekeyresult")]
    public async Task<ViewCrudKeyResult> UpdateKeyResult([FromBody]ViewCrudKeyResult view)
    {
      return await Task.Run(() => service.UpdateKeyResult(view));
    }
    /// <summary>
    /// Deletar um estabelecimento
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletekeyresult/{id}")]
    public async Task<string> DeleteKeyResult(string id)
    {
      return await Task.Run(() => service.DeleteKeyResult(id));
    }
    #endregion

    #region Dimension
    /// <summary>
    /// Listar estabelecimentos
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do estabelecimento</param>
    /// <returns>Lista de estabelecimentos</returns>
    [Authorize]
    [HttpGet]
    [Route("listdimension")]
    public async Task<List<ViewListDimension>> ListDimension(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListDimension> result = service.ListDimension(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Novo estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("newdimension")]
    public async Task<ViewCrudDimension> PostDimension([FromBody]ViewCrudDimension view)
    {
      return await Task.Run(() => service.NewDimension(view));
    }
    /// <summary>
    /// Buscar estabelecimento para manutenção
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Objeto de manutenção do estabelecimento</returns>
    [Authorize]
    [HttpGet]
    [Route("getdimension/{id}")]
    public async Task<ViewCrudDimension> ListDimension(string id)
    {
      return await Task.Run(() => service.GetDimension(id));
    }
    /// <summary>
    /// Alterar o estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updatedimension")]
    public async Task<ViewCrudDimension> UpdateDimension([FromBody]ViewCrudDimension view)
    {
      return await Task.Run(() => service.UpdateDimension(view));
    }
    /// <summary>
    /// Deletar um estabelecimento
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletedimension/{id}")]
    public async Task<string> DeleteDimension(string id)
    {
      return await Task.Run(() => service.DeleteDimension(id));
    }
    #endregion

    #region PendingCheckinObjective
    /// <summary>
    /// Listar estabelecimentos
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do estabelecimento</param>
    /// <returns>Lista de estabelecimentos</returns>
    [Authorize]
    [HttpGet]
    [Route("listpendingcheckinobjective")]
    public async Task<List<ViewListPendingCheckinObjective>> ListPendingCheckinObjective(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListPendingCheckinObjective> result = service.ListPendingCheckinObjective(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Novo estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("newpendingcheckinobjective")]
    public async Task<ViewCrudPendingCheckinObjective> PostPendingCheckinObjective([FromBody]ViewCrudPendingCheckinObjective view)
    {
      return await Task.Run(() => service.NewPendingCheckinObjective(view));
    }
    /// <summary>
    /// Buscar estabelecimento para manutenção
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Objeto de manutenção do estabelecimento</returns>
    [Authorize]
    [HttpGet]
    [Route("getpendingcheckinobjective/{id}")]
    public async Task<ViewCrudPendingCheckinObjective> ListPendingCheckinObjective(string id)
    {
      return await Task.Run(() => service.GetPendingCheckinObjective(id));
    }
    /// <summary>
    /// Alterar o estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updatependingcheckinobjective")]
    public async Task<ViewCrudPendingCheckinObjective> UpdatePendingCheckinObjective([FromBody]ViewCrudPendingCheckinObjective view)
    {
      return await Task.Run(() => service.UpdatePendingCheckinObjective(view));
    }
    /// <summary>
    /// Deletar um estabelecimento
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletependingcheckinobjective/{id}")]
    public async Task<string> DeletePendingCheckinObjective(string id)
    {
      return await Task.Run(() => service.DeletePendingCheckinObjective(id));
    }
    #endregion

    #region specific

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getparticipantcard")]
    public async Task<ViewListObjectiveParticipantCard> GetParticipantCard()
    {
      return await Task.Run(() => service.GetParticipantCard());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getobjectiveeditparticipant")]
    public async Task<List<ViewListObjectiveEdit>> GetObjectiveEditParticipant()
    {
      return await Task.Run(() => service.GetObjectiveEditParticipant());
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="idobjective"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getobjectiveeditresponsible/{idobjective}")]
    public async Task<List<ViewListObjectiveEdit>> GetObjectiveEditResponsible(string idobjective)
    {
      return await Task.Run(() => service.GetObjectiveEditResponsible(idobjective));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getlistmanager")]
    public async Task<List<ViewListPersonPhoto>> GetListManager()
    {
      return await Task.Run(() => service.GetListManager());
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getlistemployee")]
    public async Task<List<ViewListPersonPhoto>> GetListEmployee()
    {
      return await Task.Run(() => service.GetListEmployee());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getresponsiblecard")]
    public async Task<ViewListObjectiveResponsibleCard> GetResponsibleCard()
    {
      return await Task.Run(() => service.GetResponsibleCard());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getdetailresposibleobjective")]
    public async Task<List<ViewListDetailResposibleObjective>> GetDetailResposibleObjective(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetDetailResposibleObjective(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idkeyresult"></param>
    /// <param name="achievement"></param>
    /// <param name="result"></param>
    /// <param name="view"></param>
    /// <param name="idcheckin"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateresultkeyresult/{idkeyresult}/{achievement}/{result}/{idcheckin}")]
    public async Task<ViewCrudKeyResult> UpdateResultKeyResult([FromBody]ViewText view, string idkeyresult, string idcheckin, decimal achievement, decimal result)
    {
      return await Task.Run(() => service.UpdateResultKeyResult(idkeyresult, idcheckin, achievement, result, view));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idkeyresult"></param>
    /// <param name="view"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addparticipants/{idkeyresult}")]
    public async Task<string> AddParticipants([FromBody]ViewCrudParticipantKeyResult view, string idkeyresult)
    {
      return await Task.Run(() => service.AddParticipants(idkeyresult, view));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idkeyresult"></param>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteparticipants/{idkeyresult}/{idperson}")]
    public async Task<string> DeleteParticipants(string idkeyresult, string idperson)
    {
      return await Task.Run(() => service.DeleteParticipants(idkeyresult, idperson));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idobjective"></param>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addeditors/{idobjective}/{idperson}")]
    public async Task<List<ViewListPersonPhoto>> AddEditors(string idobjective, string idperson)
    {
      return await Task.Run(() => service.AddEditors(idobjective, idperson));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idobjetctive"></param>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteeditor/{idobjetctive}/{idperson}")]
    public async Task<string> DeleteEditor(string idobjetctive, string idperson)
    {
      return await Task.Run(() => service.DeleteEditor(idobjetctive, idperson));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idcheckin"></param>
    /// <param name="view"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addimpediment/{idcheckin}")]
    public async Task<List<ViewCrudImpedimentsIniciatives>> AddImpediment([FromBody]ViewCrudImpedimentsIniciatives view, string idcheckin)
    {
      return await Task.Run(() => service.AddImpediment(idcheckin, view));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idcheckin"></param>
    /// <param name="idimpediment"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteimpediment/{idcheckin}/{idimpediment}")]
    public async Task<List<ViewCrudImpedimentsIniciatives>> DeleteImpediment(string idcheckin, string idimpediment)
    {
      return await Task.Run(() => service.DeleteImpediment(idcheckin, idimpediment));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idcheckin"></param>
    /// <param name="view"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addinitiatives/{idcheckin}")]
    public async Task<List<ViewCrudImpedimentsIniciatives>> AddInitiatives([FromBody]ViewCrudImpedimentsIniciatives view, string idcheckin)
    {
      return await Task.Run(() => service.AddInitiatives(idcheckin, view));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idcheckin"></param>
    /// <param name="idiniciative"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteiniciative/{idcheckin}/{idiniciative}")]
    public async Task<List<ViewCrudImpedimentsIniciatives>> DeleteIniciative(string idcheckin, string idiniciative)
    {
      return await Task.Run(() => service.DeleteIniciative(idcheckin, idiniciative));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idimpediment"></param>
    /// <param name="idkeyresult"></param>
    /// <param name="like"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("likeimpediment/{idimpediment}/{idkeyresult}/{like}")]
    public async Task<string> LikeImpediment(string idimpediment, string idkeyresult, bool like)
    {
      return await Task.Run(() => service.LikeImpediment(idimpediment, idkeyresult, like));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idiniciatives"></param>
    /// <param name="idkeyresult"></param>
    /// <param name="like"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("likeiniciative/{idiniciatives}/{idpendingcheckin}/{like}")]
    public async Task<string> LikeIniciative(string idiniciatives, string idkeyresult, bool like)
    {
      return await Task.Run(() => service.LikeIniciative(idiniciatives, idkeyresult, like));
    }
    #endregion
  }
}