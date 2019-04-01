using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("mandatorytraining")]
  public class MandatoryTrainingController : Controller
  {
    private readonly IServiceMandatoryTraining service;


    #region constructor
    public MandatoryTrainingController(IServiceMandatoryTraining _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion


    #region mandatorytraining
    [Authorize]
    [HttpPost]
    [Route("addcompany")]
    public string AddCompany([FromBody] ViewCrudCompanyMandatory view)
    {
      return service.AddCompany(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addoccuaption")]
    public string AddOccuaption([FromBody]ViewCrudOccupationMandatory view)
    {
      return service.AddOccupation(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addperson")]
    public string AddPerson([FromBody]ViewCrudPersonMandatory view)
    {
      return service.AddPerson(view);
    }


    [Authorize]
    [HttpGet]
    [Route("listcompany/{idcourse}")]
    public List<ViewListCompany> ListCompany(string idcourse, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCompany(idcourse, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listoccupation/{idcourse}/{idcompany}")]
    public List<ViewListOccupation> ListOccupation(string idcourse, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOccupation(idcourse, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("trainingplanlist/{idmanager}/{type}/{origin}")]
    public List<ViewTrainingPlanList> ListTrainingPlanPersonList(string idmanager, EnumTypeUser type, EnumOrigin origin, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTrainingPlanPersonList(idmanager, type, origin, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listperson/{idcourse}/{idcompany}")]
    public List<ViewListPerson> ListPerson(string idcourse, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPerson(idcourse, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }


    [Authorize]
    [HttpGet]
    [Route("listtrainingplanperson/{idperson}")]
    public List<ViewTrainingPlan> ListTrainingPlanPerson(string idperson, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTrainingPlanPerson(idperson, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }


    [Authorize]
    [HttpDelete]
    [Route("removecompany/{idcourse}/{idcompany}")]
    public string RemoveCompany(string idcourse, string idcompany)
    {
      return service.RemoveCompany(idcourse, idcompany);
    }

    [Authorize]
    [HttpDelete]
    [Route("removeoccupation/{idcourse}/{idoccupation}")]
    public string RemoveOccupation(string idcourse, string idoccupation)
    {
      return service.RemoveOccupation(idcourse, idoccupation);
    }

    [Authorize]
    [HttpDelete]
    [Route("removeperson/{idcourse}/{idperson}")]
    public string RemovePerson(string idcourse, string idperson)
    {
      return service.RemovePerson(idcourse, idperson);
    }

    [Authorize]
    [HttpDelete]
    [Route("removetrainingplan/{id}")]
    public string RemoveTrainingPlan(string id)
    {
      return service.RemoveTrainingPlan(id);
    }


    /// <summary>
    /// Atualizar informações do plano
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatetrainingplan")]
    public string UpdateTrainingPlan([FromBody]ViewCrudTrainingPlan view)
    {
      return service.UpdateTrainingPlan(view);
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
    public List<ViewCrudTrainingPlan> ListTrainingPlan(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTrainingPlan(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
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
    public List<ViewCrudTrainingPlan> ListTrainingPlan(string idcompany, string iduser, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTrainingPlan(idcompany, iduser, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Inclusão de novo plano de treinamento
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("newtrainingplan")]
    public string NewTrainingPlan([FromBody]ViewCrudTrainingPlan view)
    {
      return service.NewTrainingPlan(view);
    }

    /// <summary>
    /// Busca informações para editar plano de treinamento
    /// </summary>
    /// <param name="id">Identificador do plano de treinamento</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("gettrainingplan/{id}")]
    public ViewCrudTrainingPlan GetTrainingPlan(string id)
    {
      return service.GetTrainingPlan(id);
    }

    /// <summary>
    /// Busca informações para editar treinamento obrigatório
    /// </summary>
    /// <param name="idcourse">Identificador do curso</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getmandatorytraining/{idcourse}")]
    public ViewCrudMandatoryTraining GetMandatoryTraining(string idcourse)
    {
      return service.GetMandatoryTraining(idcourse);
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
    public List<ViewCrudMandatoryTraining> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    #endregion

    #region old

    [Authorize]
    [HttpPut]
    [Route("old/updatetrainingplan")]
    public string UpdateTrainingPlanOld([FromBody]TrainingPlan view)
    {
      return service.UpdateTrainingPlanOld(view);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listtrainingplan/{idcompany}")]
    public List<TrainingPlan> ListTrainingPlanOld(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTrainingPlanOld(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/listtrainingplan/{idcompany}/{idperson}")]
    public List<TrainingPlan> ListTrainingPlanOld(string idcompany, string idperson, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTrainingPlanOld(idcompany, idperson, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpPost]
    [Route("old/newtrainingplan")]
    public string NewTrainingPlanOld([FromBody]TrainingPlan view)
    {
      return service.NewTrainingPlanOld(view);
    }

    [Authorize]
    [HttpGet]
    [Route("old/gettrainingplan/{id}")]
    public TrainingPlan GetTrainingPlanOld(string id)
    {
      return service.GetTrainingPlanOld(id);
    }

    [Authorize]
    [HttpGet]
    [Route("old/getmandatorytraining/{idcourse}")]
    public MandatoryTraining GetMandatoryTrainingOld(string idcourse)
    {
      return service.GetMandatoryTrainingOld(idcourse);
    }


    [Authorize]
    [HttpGet]
    [Route("old/list")]
    public List<MandatoryTraining> ListOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOld(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    #endregion


  }
}