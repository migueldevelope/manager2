using System.Collections.Generic;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador de objetivos
  /// </summary>
  [Produces("application/json")]
  [Route("goals")]
  public class GoalsController : Controller
  {
    private readonly IServiceGoals service;

    #region Constructor
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">serviço dos objetivos</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public GoalsController(IServiceGoals _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Goals
    /// <summary>
    /// Inserir novo objetivo
    /// </summary>
    /// <param name="view">Objeto de manutenção de objetivos</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("new")]
    public string New([FromBody]ViewCrudGoal view)
    {
      return service.New(view);
    }
    /// <summary>
    /// Listar os objetivos
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<ViewListGoal> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListGoal> result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Buscar um objetivo para manutenção
    /// </summary>
    /// <param name="id">Identificador do objetivo</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public ViewCrudGoal Get(string id)
    {
      return service.Get(id);
    }
    /// <summary>
    /// Alterar o objetivo
    /// </summary>
    /// <param name="view">Objeto de manutenção do objetivo</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public string Update([FromBody]ViewCrudGoal view)
    {
      return service.Update(view);
    }
    /// <summary>
    /// Excluir um objetivo
    /// </summary>
    /// <param name="id">Identificador do objetivo</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public string Delete(string id)
    {
      return service.Delete(id);
    }
    #endregion

    #region Period Goals
    /// <summary>
    /// Cadastrar um novo período de objetivos
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("newgoalsperiod")]
    public string NewGoalsPeriod([FromBody]ViewCrudGoalPeriod view)
    {
      return service.NewGoalsPeriod(view);
    }
    /// <summary>
    /// Listar os períodos de objetivos
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listgoalsperiod")]
    public List<ViewCrudGoalPeriod> ListGoalsPeriod(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGoalsPeriod(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Buscar um período de objetivo para manutenção
    /// </summary>
    /// <param name="id">Identificador do período de objetivos</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getgoalsperiod/{id}")]
    public ViewCrudGoalPeriod GetGoalsPeriod(string id)
    {
      return service.GetGoalsPeriod(id);
    }
    /// <summary>
    /// Alterar um período de objetivo
    /// </summary>
    /// <param name="view">Objetivo de manutenção do período de objetivo</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updategoalsperiod")]
    public string UpdateGoalsPeriod([FromBody]ViewCrudGoalPeriod view)
    {
      return service.UpdateGoalsPeriod(view);
    }
    /// <summary>
    /// Excluir um período de objetivos
    /// </summary>
    /// <param name="id">Identificador do período de objetivo</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deletegoalsperiod/{id}")]
    public string DeleteGoalsPeriod(string id)
    {
      return service.DeleteGoalsPeriod(id);
    }
    #endregion

    #region Company Goals
    /// <summary>
    /// Inclusão de novos objetivos do período e empresa
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("newgoalscompany")]
    public string NewGoalsCompany([FromBody]ViewCrudGoalCompany view)
    {
      return service.NewGoalsCompany(view);
    }
    /// <summary>
    /// Listar os objetivos da empresa e do período
    /// </summary>
    /// <param name="idgoalsperiod"></param>
    /// <param name="idcompany"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listgoalscompany/{idgoalsperiod}/{idcompany}")]
    public List<ViewListGoalCompany> ListGoalsCompany(string idgoalsperiod, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGoalsCompany(idgoalsperiod, idcompany,ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Listar os objetivos
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listgoalscompany")]
    public List<ViewListGoalCompany> ListGoalsCompany(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGoalsCompany(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Buscar um objetivo de empresa para um período
    /// </summary>
    /// <param name="id">Identificador do objetivo no período e empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getgoalscompany/{id}")]
    public ViewCrudGoalCompany GetGoalsCompany(string id)
    {
      return service.GetGoalsCompany(id);
    }
    /// <summary>
    /// Alterar um objetivo da empresa em um período
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updategoalscompany")]
    public string UpdateGoalsCompany([FromBody]ViewCrudGoalCompany view)
    {
      return service.UpdateGoalsCompany(view);
    }
    /// <summary>
    /// Excluir um objetivo da empresa num período específico
    /// </summary>
    /// <param name="id">Identificador do objetivo da empresa no período</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deletegoalscompany/{id}")]
    public string DeleteGoalsCompany(string id)
    {
      return service.DeleteGoalsCompany(id);
    }
    #endregion

  }
}