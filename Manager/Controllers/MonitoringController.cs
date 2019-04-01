using System.Collections.Generic;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
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
  /// Controlador para acompanhamento
  /// </summary>
  [Produces("application/json")]
  [Route("monitoring")]
  public class MonitoringController : Controller
  {
    private readonly IServiceMonitoring service;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Serviço de acompanhamento</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public MonitoringController(IServiceMonitoring _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Monitoring
    /// <summary>
    /// Remover todos os monitoramentos de uma pessoa
    /// </summary>
    /// <param name="idperson">Identificador da pessoa</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteall/{idperson}")]
    public string RemoveAllMonitoring(string idperson)
    {
      return service.RemoveAllMonitoring(idperson);
    }
    /// <summary>
    /// Exclusão de um monitoramento
    /// </summary>
    /// <param name="idmonitoring">Identificador do monitoramento</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{idmonitoring}")]
    public string RemoveOnBoarding(string idmonitoring)
    {
      return service.RemoveMonitoring(idmonitoring);
    }
    /// <summary>
    /// Exclusão do último monitoramento
    /// </summary>
    /// <param name="idperson">Identificação da pessoa</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deletelast/{idperson}")]
    public string RemoveLastMonitoring(string idperson)
    {
      return service.RemoveLastMonitoring(idperson);
    }
    /// <summary>
    /// Exclusão de atividade do monitoramento
    /// </summary>
    /// <param name="idmonitoring">Identificador do monitoramento</param>
    /// <param name="idactivitie">Identificador da atividade</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("removemonitoringactivities/{idmonitoring}/{idactivitie}")]
    public string RemoveMonitoringActivities(string idmonitoring, string idactivitie)
    {
      return service.RemoveMonitoringActivities(idmonitoring, idactivitie);
    }
    /// <summary>
    /// Exclusão de compentário
    /// </summary>
    /// <param name="idmonitoring">Identificador do monitoramento</param>
    /// <param name="iditem">Identificador do item</param>
    /// <param name="idcomments">Identificador do comentário</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deletecomments/{idmonitoring}/{iditem}/{idcomments}")]
    public string DeleteComments(string idmonitoring, string iditem, string idcomments)
    {
      return service.DeleteComments(idmonitoring, iditem, idcomments);
    }
    /// <summary>
    /// Alteração de comentário
    /// </summary>
    /// <param name="idmonitoring">Identificador do monitoramento</param>
    /// <param name="iditem">Identificador do item</param>
    /// <param name="usercomment">Tipo de usuário do comentário</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatecommentsview/{idmonitoring}/{iditem}/{usercomment}")]
    public string UpdateCommentsView(string idmonitoring, string iditem, EnumUserComment usercomment)
    {
      return service.UpdateCommentsView(idmonitoring, iditem, usercomment);
    }
    /// <summary>
    /// Alteração do comentário
    /// </summary>
    /// <param name="idmonitoring">Identificador do monitoramento</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("validcomments/{idmonitoring}")]
    public bool UpdateCommentsView(string idmonitoring)
    {
      return service.ValidComments(idmonitoring);
    }
    /// <summary>
    /// Inclusão monitoring
    /// </summary>
    /// <param name="idperson">Identificador contrato</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("new/{idperson}")]
    public ViewListMonitoring NewMonitoring(string idperson)
    {
      return service.NewMonitoring(idperson);
    }
    /// <summary>
    /// Atualiza informações monitogin
    /// </summary>
    /// <param name="monitoring">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public string UpdateMonitoring([FromBody]ViewCrudMonitoring monitoring)
    {
      return service.UpdateMonitoring(monitoring);
    }
    /// <summary>
    /// Lista monitoring finalizado para gestor
    /// </summary>
    /// <param name="idmanager">Identificador Gestor</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listend/{idmanager}")]
    public List<ViewListMonitoring> ListMonitoringsEnd(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListMonitoringsEnd(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Lista monitoring para exclusão
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getlistexclud")]
    public List<ViewListMonitoring> GetListExclud(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetListExclud(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Lista monitoring em andamento para gestor
    /// </summary>
    /// <param name="idmanager"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("list/{idmanager}")]
    public List<ViewListMonitoring> ListMonitoringsWait(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListMonitoringsWait(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Lista monitoring para pessoa
    /// </summary>
    /// <param name="idmanager">Identificador contrato</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("personend/{idmanager}")]
    public List<ViewListMonitoring> PersonMonitoringsEnd(string idmanager)
    {
      return service.PersonMonitoringsEnd(idmanager);
    }
    /// <summary>
    /// Lista monitoring para pessoa
    /// </summary>
    /// <param name="idmanager">Identificador do contrato</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("personwait/{idmanager}")]
    public ViewListMonitoring PersonMonitoringsWait(string idmanager)
    {
      return service.PersonMonitoringsWait(idmanager);
    }
    /// <summary>
    /// Busca informação monitoring para editar
    /// </summary>
    /// <param name="id">Identificador monitoring</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public ViewCrudMonitoring GetMonitoring(string id)
    {
      return service.GetMonitorings(id);
    }
    /// <summary>
    /// Lista skills
    /// </summary>
    /// <param name="idperson">Identificador do contrato</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getskills/{idperson}")]
    public List<ViewListSkill> GetSkills(string idperson)
    {
      return service.GetSkills(idperson);
    }
    /// <summary>
    /// Busca informações para editar entrega
    /// </summary>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <param name="idactivitie">Identificador entrega</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getmonitoringactivities/{idmonitoring}/{idactivitie}")]
    public ViewCrudMonitoringActivities GetMonitoringActivities(string idmonitoring, string idactivitie)
    {
      return service.GetMonitoringActivities(idmonitoring, idactivitie);
    }
    /// <summary>
    /// Atualiza entrega monitoring
    /// </summary>
    /// <param name="activitie">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatemonitoringactivities/{idmonitoring}")]
    public string UpdateMonitoringActivities([FromBody]ViewCrudMonitoringActivities activitie, string idmonitoring)
    {
      return service.UpdateMonitoringActivities(idmonitoring, activitie);
    }
    /// <summary>
    /// Adiciona um entrega no monitoring
    /// </summary>
    /// <param name="activitie">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addmonitoringactivities/{idmonitoring}")]
    public string AddMonitoringActivities([FromBody] ViewCrudActivities activitie, string idmonitoring)
    {
      return service.AddMonitoringActivities(idmonitoring, activitie);
    }
    /// <summary>
    /// Lista comentarios de um item do monitoring
    /// </summary>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <param name="iditem">Identificador Item</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listcomments/{idmonitoring}/{iditem}")]
    public List<ListComments> GetListComments(string idmonitoring, string iditem)
    {
      return service.GetListCommentsOld(idmonitoring, iditem);
    }
    /// <summary>
    /// Inclusão comentario
    /// </summary>
    /// <param name="comments">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador Monitoring</param>
    /// <param name="iditem">Identificador item monitoring</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addcomments/{idmonitoring}/{iditem}")]
    public List<ViewCrudComment> AddComments([FromBody]ViewCrudComment comments, string idmonitoring, string iditem)
    {
      return service.AddComments(idmonitoring, iditem, comments);
    }
    /// <summary>
    /// Atualiza comentario item do monitoring
    /// </summary>
    /// <param name="comments">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <param name="iditem">Identificador item do monitoring</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatecomments/{idmonitoring}/{iditem}")]
    public string UpdateComments([FromBody]ViewCrudComment comments, string idmonitoring, string iditem)
    {
      return service.UpdateComments(idmonitoring, iditem, comments);
    }
    /// <summary>
    /// Adiciona um plano
    /// </summary>
    /// <param name="plan">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <param name="iditem">Identificador item</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addplan/{idmonitoring}/{iditem}")]
    public List<ViewCrudPlan> AddPlan([FromBody]ViewCrudPlan plan, string idmonitoring, string iditem)
    {
      return service.AddPlan(idmonitoring, iditem, plan);
    }
    /// <summary>
    /// Atualiza informações do plano dentro de um item do monitoring
    /// </summary>
    /// <param name="plan">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <param name="iditem">Identificador item do monitoring</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateplan/{idmonitoring}/{iditem}")]
    public List<ViewCrudPlan> UpdatePlan([FromBody]ViewCrudPlan plan, string idmonitoring, string iditem)
    {
      return service.UpdatePlan(idmonitoring, iditem, plan);
    }
    #endregion

    #region Old

    [Authorize]
    [HttpPost]
    [Route("old/new/{idperson}")]
    public Monitoring NewMonitoringOld([FromBody]Monitoring monitoring, string idperson)
    {
      return service.NewMonitoringOld(monitoring, idperson);
    }

    [Authorize]
    [HttpPut]
    [Route("old/update/{idperson}")]
    public string UpdateMonitoringOld([FromBody]Monitoring monitoring, string idperson)
    {
      return service.UpdateMonitoringOld(monitoring, idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listend/{idmanager}")]
    public List<Monitoring> ListEndOld(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListMonitoringsEndOld(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/getlistexclud")]
    public List<Monitoring> GetListExcludOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetListExcludOld(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/list/{idmanager}")]
    public List<Monitoring> ListOld(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListMonitoringsWaitOld(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/personend/{idmanager}")]
    public List<Monitoring> ListEndPersonOld(string idmanager)
    {
      return service.PersonMonitoringsEndOld(idmanager);
    }

    [Authorize]
    [HttpGet]
    [Route("old/personwait/{idmanager}")]
    public Monitoring ListPersonOld(string idmanager)
    {
      return service.PersonMonitoringsWaitOld(idmanager);
    }

    [Authorize]
    [HttpGet]
    [Route("old/get/{id}")]
    public Monitoring GetMonitoringOld(string id)
    {
      return service.GetMonitoringsOld(id);
    }

    [Authorize]
    [HttpGet]
    [Route("old/getskills/{idperson}")]
    public List<Skill> GetSkillsOld(string idperson)
    {
      return service.GetSkillsOld(idperson);
    }


    [Authorize]
    [HttpGet]
    [Route("old/getmonitoringactivities/{idmonitoring}/{idactivitie}")]
    public MonitoringActivities GetMonitoringActivitiesOld(string idmonitoring, string idactivitie)
    {
      return service.GetMonitoringActivitiesOld(idmonitoring, idactivitie);
    }


    [Authorize]
    [HttpPut]
    [Route("old/updatemonitoringactivities/{idmonitoring}")]
    public string UpdateMonitoringActivitiesOld([FromBody]MonitoringActivities activitie, string idmonitoring)
    {
      return service.UpdateMonitoringActivitiesOld(idmonitoring, activitie);
    }

    [Authorize]
    [HttpPost]
    [Route("old/addmonitoringactivities/{idmonitoring}")]
    public string AddMonitoringActivitiesOld([FromBody] Activitie activitie, string idmonitoring)
    {
      return service.AddMonitoringActivitiesOld(idmonitoring, activitie);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listcomments/{idmonitoring}/{iditem}")]
    public List<ListComments> GetListCommentsOld(string idmonitoring, string iditem)
    {
      return service.GetListCommentsOld(idmonitoring, iditem);
    }

    [Authorize]
    [HttpPost]
    [Route("old/addcomments/{idmonitoring}/{iditem}")]
    public List<ListComments> AddCommentsOld([FromBody]ListComments comments, string idmonitoring, string iditem)
    {
      return service.AddCommentsOld(idmonitoring, iditem, comments);
    }


    [Authorize]
    [HttpPut]
    [Route("old/updatecomments/{idmonitoring}/{iditem}")]
    public string UpdateCommentsOld([FromBody]ListComments comments, string idmonitoring, string iditem)
    {
      return service.UpdateCommentsOld(idmonitoring, iditem, comments);
    }


    [Authorize]
    [HttpPost]
    [Route("old/addplan/{idmonitoring}/{iditem}")]
    public List<Plan> AddPlanOld([FromBody]Plan plan, string idmonitoring, string iditem)
    {
      return service.AddPlanOld(idmonitoring, iditem, plan);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updateplan/{idmonitoring}/{iditem}")]
    public List<Plan> UpdatePlanOld([FromBody]Plan plan, string idmonitoring, string iditem)
    {
      return service.UpdatePlanOld(idmonitoring, iditem, plan);
    }

    #endregion

  }
}