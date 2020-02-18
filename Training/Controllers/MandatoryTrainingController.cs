using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Training.Controllers
{
  /// <summary>
  /// 
  /// </summary>
  [Produces("application/json")]
  [Route("mandatorytraining")]
  public class MandatoryTrainingController : DefaultController
  {
    private readonly IServiceMandatoryTraining service;

    #region constructor
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_service"></param>
    /// <param name="contextAccessor"></param>
    public MandatoryTrainingController(IServiceMandatoryTraining _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region mandatorytraining

    /// <summary>
    /// 
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addcompany")]
    public List<string> AddCompany([FromBody] ViewCrudCompanyMandatory view)
    {
      return service.AddCompany(view);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addoccuaption")]
    public List<string> AddOccuaption([FromBody]ViewCrudOccupationMandatory view)
    {
      return service.AddOccupation(view);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addperson")]
    public List<string> AddPerson([FromBody]ViewCrudPersonMandatory view)
    {
      return service.AddPerson(view);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idcompany"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listcompanygroup/{idcompany}")]
    public async Task<List<ViewListCourse>> ListCompanyGroup(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCompanyGroup(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idoccupation"></param>
    /// <param name="idcompany"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listoccupationgroup/{idoccupation}/{idcompany}")]
    public async Task<List<ViewListCourse>> ListOccupationGroup(string idoccupation, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOccupationGroup(idoccupation, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Lista pessoas
    /// </summary>
    /// <param name="idperson">Identificador curso</param>
    /// <param name="idcompany">Identificador empresa</param>
    /// <param name="count">contador</param>
    /// <param name="page">pagina</param>
    /// <param name="filter">filtro</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listpersongroup/{idperson}/{idcompany}")]
    public async Task<List<ViewListCourse>> ListPersonGroup(string idperson, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPersonGroup(idperson, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="idcourse"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listcompany/{idcourse}")]
    public async Task<List<ViewListCompany>> ListCompany(string idcourse, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCompany(idcourse, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idcourse"></param>
    /// <param name="idcompany"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listoccupation/{idcourse}/{idcompany}")]
    public async Task<List<ViewListOccupation>> ListOccupation(string idcourse, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOccupation(idcourse, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Lista pessoas
    /// </summary>
    /// <param name="idcourse">Identificador curso</param>
    /// <param name="idcompany">Identificador empresa</param>
    /// <param name="count">contador</param>
    /// <param name="page">pagina</param>
    /// <param name="filter">filtro</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listperson/{idcourse}/{idcompany}")]
    public async Task<List<ViewListPersonBase>> ListPerson(string idcourse, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPerson(idcourse, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// remove empresa
    /// </summary>
    /// <param name="idcourse"></param>
    /// <param name="idcompany"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("removecompany/{idcourse}/{idcompany}")]
    public async Task<string> RemoveCompany(string idcourse, string idcompany)
    {
      return await Task.Run(() => service.RemoveCompany(idcourse, idcompany));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idcourse"></param>
    /// <param name="idoccupation"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("removeoccupation/{idcourse}/{idoccupation}")]
    public async Task<string> RemoveOccupation(string idcourse, string idoccupation)
    {
      return await Task.Run(() => service.RemoveOccupation(idcourse, idoccupation));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idcourse"></param>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("removeperson/{idcourse}/{idperson}")]
    public async Task<string> RemovePerson(string idcourse, string idperson)
    {
      return await Task.Run(() => service.RemovePerson(idcourse, idperson));
    }

    /// <summary>
    /// Busca informações para editar treinamento obrigatório
    /// </summary>
    /// <param name="idcourse">Identificador do curso</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getmandatorytraining/{idcourse}")]
    public async Task<ViewCrudMandatoryTraining> GetMandatoryTraining(string idcourse)
    {
      return await Task.Run(() => service.GetMandatoryTraining(idcourse));
    }

    /// <summary>
    /// Lista treinamentos obrigatórios
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public async Task<List<ViewCrudMandatoryTraining>> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    #endregion

    #region trainingplan

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmanager"></param>
    /// <param name="type"></param>
    /// <param name="origin"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("trainingplanlist/{idmanager}/{type}/{origin}")]
    public async Task<List<ViewListTrainingPlan>> ListTrainingPlanPersonList(string idmanager, EnumTypeUser type, EnumOrigin origin, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTrainingPlanPersonList(idmanager, type, origin, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmanager"></param>
    /// <param name="type"></param>
    /// <param name="origin"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("trainingplanlistmanager/{idmanager}/{type}/{origin}")]
    public async Task<List<ViewListTrainingPlanManager>> ListTrainingPlanPersonManager(string idmanager, EnumTypeUser type, EnumOrigin origin, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTrainingPlanPersonManager(idmanager, type, origin, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// lista treinamento obrigatório 
    /// </summary>
    /// <param name="idperson">identificaodor pessoa</param>
    /// <param name="count">contador</param>
    /// <param name="page">pagina</param>
    /// <param name="filter">filtro</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listtrainingplanperson/{idperson}")]
    public async Task<ViewListTrainingPlan> ListTrainingPlanPerson(string idperson, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTrainingPlanPerson(idperson, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("removetrainingplan/{id}")]
    public async Task<string> RemoveTrainingPlan(string id)
    {
      return await Task.Run(() => service.RemoveTrainingPlan(id));
    }


    /// <summary>
    /// Atualizar informações do plano
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatetrainingplan")]
    public async Task<string> UpdateTrainingPlan([FromBody]ViewCrudTrainingPlan view)
    {
      return await Task.Run(() => service.UpdateTrainingPlan(view));
    }

    /// <summary>
    /// Lista de plano de treinamento da empresa
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listtrainingplan/{idcompany}")]
    public async Task<List<ViewCrudTrainingPlan>> ListTrainingPlan(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTrainingPlan(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Lista planos de treinamento de um contrato
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="iduser">Identificador do usuario</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listtrainingplan/{idcompany}/{iduser}")]
    public async Task<List<ViewCrudTrainingPlan>> ListTrainingPlan(string idcompany, string iduser, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTrainingPlan(idcompany, iduser, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Inclusão de novo plano de treinamento
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("newtrainingplan")]
    public async Task<string> NewTrainingPlan([FromBody]ViewCrudTrainingPlan view)
    {
      return await Task.Run(() => service.NewTrainingPlan(view));
    }

    /// <summary>
    /// Busca informações para editar plano de treinamento
    /// </summary>
    /// <param name="id">Identificador do plano de treinamento</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("gettrainingplan/{id}")]
    public async Task<ViewCrudTrainingPlan> GetTrainingPlan(string id)
    {
      return await Task.Run(() => service.GetTrainingPlan(id));
    }

    #endregion



  }
}