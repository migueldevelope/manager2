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
  /// Controlador de objetivos
  /// </summary>
  [Produces("application/json")]
  [Route("goals")]
  public class GoalsController : DefaultController
  {
    private readonly IServiceGoals service;

    #region Constructor
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">serviço dos objetivos</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public GoalsController(IServiceGoals _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
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
    public async Task<string> New([FromBody]ViewCrudGoal view)
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
    public async Task<List<ViewListGoal>> List( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListGoal> result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return  result;
    }
    /// <summary>
    /// Listar os objetivos
    /// </summary>
    /// <param name="id"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listmanager/{id}")]
    public async Task<List<ViewListGoal>> ListManager(string id,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListGoal> result = service.ListManager(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return  result;
    }

    /// <summary>
    /// Listar os objetivos
    /// </summary>
    /// <param name="id"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listcompany/{id}")]
    public async Task<List<ViewListGoal>> ListCompany(string id,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListGoal> result = service.ListCompany(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return  result;
    }

    /// <summary>
    /// Buscar um objetivo para manutenção
    /// </summary>
    /// <param name="id">Identificador do objetivo</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudGoal> Get(string id)
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
    public async Task<string> Update([FromBody]ViewCrudGoal view)
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
    public async Task<string> Delete(string id)
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
    public async Task<string> NewGoalsPeriod([FromBody]ViewCrudGoalPeriod view)
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
    public async Task<List<ViewCrudGoalPeriod>> ListGoalsPeriod( int count = 10, int page = 1, string filter = "")
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
    public async Task<ViewCrudGoalPeriod> GetGoalsPeriod(string id)
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
    public async Task<string> UpdateGoalsPeriod([FromBody]ViewCrudGoalPeriod view)
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
    public async Task<string> DeleteGoalsPeriod(string id)
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
    public async Task<string> NewGoalsCompany([FromBody]ViewCrudGoalCompany view)
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
    public async Task<List<ViewCrudGoalItem>> ListGoalsCompany(string idgoalsperiod, string idcompany,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGoalsCompany(idgoalsperiod, idcompany, ref total, count, page, filter);
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
    public async Task<List<ViewListGoalCompany>> ListGoalsCompany( int count = 10, int page = 1, string filter = "")
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
    public async Task<ViewCrudGoalCompany> GetGoalsCompany(string id)
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
    public async Task<string> UpdateGoalsCompany([FromBody]ViewCrudGoalCompany view)
    {
      return service.UpdateGoalsCompany(view);
    }
    /// <summary>
    /// Alterar um objetivo da empresa em um período
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updategoalscompanyachievement")]
    public async Task<string> UpdateGoalsCompanyAchievement([FromBody]ViewCrudAchievement view)
    {
      return service.UpdateGoalsCompanyAchievement(view);
    }

    /// <summary>
    /// Excluir um objetivo da empresa num período específico
    /// </summary>
    /// <param name="id">Identificador do objetivo da empresa no período</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deletegoalscompany/{id}")]
    public async Task<string> DeleteGoalsCompany(string id)
    {
      return service.DeleteGoalsCompany(id);
    }
    #endregion

    #region Manager Goals

    /// <summary>
    /// Inclusão de novos objetivos do período e empresa
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("newgoalsmanagerportal")]
    public async Task<string> NewGoalsManagerPortal([FromBody]ViewCrudGoalManagerPortal view)
    {
      return service.NewGoalsManagerPortal(view);
    }

    /// <summary>
    /// Inclusão de novos objetivos do período e empresa
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("newgoalsmanager")]
    public async Task<string> NewGoalsManager([FromBody]ViewCrudGoalManager view)
    {
      return service.NewGoalsManager(view);
    }
    /// <summary>
    /// Listar os objetivos da empresa e do período
    /// </summary>
    /// <param name="idgoalsperiod"></param>
    /// <param name="idmanager"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listgoalsmanager/{idgoalsperiod}/{idmanager}")]
    public async Task<ViewListGoalsItem> ListGoalsManager(string idgoalsperiod, string idmanager,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGoalsManager(idgoalsperiod, idmanager, ref total, count, page, filter);
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
    [Route("listgoalsmanager")]
    public async Task<List<ViewListGoalManager>> ListGoalsManager( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGoalsManager(ref total, count, page, filter);
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
    [Route("getgoalsmanager/{id}")]
    public async Task<ViewCrudGoalManager> GetGoalsManager(string id)
    {
      return service.GetGoalsManager(id);
    }

    /// <summary>
    /// Buscar um objetivo de empresa para um período
    /// </summary>
    /// <param name="id">Identificador do objetivo no período e empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getgoalsmanagerportal/{id}")]
    public async Task<ViewCrudGoalManagerPortal> GetGoalsManagerPortal(string id)
    {
      return service.GetGoalsManagerPortal(id);
    }

    /// <summary>
    /// Alterar um objetivo da empresa em um período
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updategoalsmanager")]
    public async Task<string> UpdateGoalsManager([FromBody]ViewCrudGoalManager view)
    {
      return service.UpdateGoalsManager(view);
    }

    /// <summary>
    /// Alterar um objetivo da empresa em um período
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updategoalsmanagerportal")]
    public async Task<string> UpdateGoalsManagerPortal([FromBody]ViewCrudGoalManagerPortal view)
    {
      return service.UpdateGoalsManagerPortal(view);
    }


    /// <summary>
    /// Alterar um objetivo da empresa em um período
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updategoalsmanagerachievement")]
    public async Task<string> UpdateGoalsManagerAchievement([FromBody]ViewCrudAchievement view)
    {
      return service.UpdateGoalsManagerAchievement(view);
    }

    /// <summary>
    /// Excluir um objetivo da empresa num período específico
    /// </summary>
    /// <param name="id">Identificador do objetivo da empresa no período</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deletegoalsmanager/{id}")]
    public async Task<string> DeleteGoalsManager(string id)
    {
      return service.DeleteGoalsManager(id);
    }
    #endregion

    #region Person Goals
    /// <summary>
    /// Inclusão de novos objetivos do período e empresa
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("newgoalspersonportal")]
    public async Task<string> NewGoalsPersonPortal([FromBody]ViewCrudGoalPerson view)
    {
      return service.NewGoalsPersonPortal(view);
    }

    ///// <summary>
    ///// Inclusão de novos objetivos do período e empresa
    ///// </summary>
    ///// <param name="view">Objeto de manutenção</param>
    ///// <returns></returns>
    //[Authorize]
    //[HttpPost]
    //[Route("newgoalsperson")]
    //public async Task< string NewGoalsPerson([FromBody]ViewCrudGoalPerson view)
    //{
    //  return service.NewGoalsPerson(view);
    //}
    /// <summary>
    /// Listar os objetivos da empresa e do período
    /// </summary>
    /// <param name="idgoalsperiod"></param>
    /// <param name="idperson"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listgoalsperson/{idgoalsperiod}/{idperson}")]
    public async Task<ViewListGoalsItem> ListGoalsPerson(string idgoalsperiod, string idperson,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGoalsPerson(idgoalsperiod, idperson, ref total, count, page, filter);
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
    [Route("listgoalsperson")]
    public async Task<List<ViewListGoalPerson>> ListGoalsPerson( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGoalsPerson(ref total, count, page, filter);
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
    [Route("getgoalsperson/{id}")]
    public async Task<ViewCrudGoalPerson> GetGoalsPerson(string id)
    {
      return service.GetGoalsPerson(id);
    }
    ///// <summary>
    ///// Alterar um objetivo da empresa em um período
    ///// </summary>
    ///// <param name="view">Objeto de manutenção</param>
    ///// <returns></returns>
    //[Authorize]
    //[HttpPut]
    //[Route("updategoalsperson")]
    //public async Task< string UpdateGoalsPerson([FromBody]ViewCrudGoalPerson view)
    //{
    //  return service.UpdateGoalsPerson(view);
    //}

    /// <summary>
    /// Alterar um objetivo da empresa em um período
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updategoalspersonportal")]
    public async Task<string> UpdateGoalsPersonPortal([FromBody]ViewCrudGoalPerson view)
    {
      return service.UpdateGoalsPersonPortal(view);
    }

    /// <summary>
    /// Alterar um objetivo da empresa em um período
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updategoalspersonachievement")]
    public async Task<string> UpdateGoalsPersonAchievement([FromBody]ViewCrudAchievement view)
    {
      return service.UpdateGoalsPersonAchievement(view);
    }

    /// <summary>
    /// Excluir um objetivo da empresa num período específico
    /// </summary>
    /// <param name="id">Identificador do objetivo da empresa no período</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deletegoalsperson/{id}")]
    public async Task<string> DeleteGoalsPerson(string id)
    {
      return service.DeleteGoalsPerson(id);
    }
    #endregion

    #region Person Goals Control

    /// <summary>
    /// Inclusão de novos objetivos do período e empresa
    /// </summary>
    /// <param name="idperson">Identificador da pessoa</param>
    /// <param name="idperiod">Identificador do período</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("newgoalspersoncontrol/{idperson}/{idperiod}")]
    public async Task<string> NewGoalsPersonControl(string idperson, string idperiod)
    {
      return service.NewGoalsPersonControl(idperson, idperiod);
    }

    /// <summary>
    /// Listar os objetivos
    /// </summary>
    /// <param name="id">Identificador da pessoa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listgoalspersoncontrolme/{id}")]
    public async Task<ViewListGoalPersonControl> ListGoalsPersonControlMe(string id)
    {
      long total = 0;
      var result = service.ListGoalsPersonControlMe(id);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Listar os objetivos
    /// </summary>
    /// <param name="id">Identificador do gestor</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listgoalspersoncontrol/{id}")]
    public async Task<List<ViewListGoalPersonControl>> ListGoalsPersonControl(string id,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGoalsPersonControl(id, ref total, count, page, filter);
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
    [Route("getgoalspersoncontrol/{id}")]
    public async Task<ViewCrudGoalPersonControl> GetGoalsPersonControl(string id)
    {
      return service.GetGoalsPersonControl(id);
    }
    /// <summary>
    /// Alterar um objetivo da empresa em um período
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updategoalspersoncontrol")]
    public async Task<string> UpdateGoalsPersonControl([FromBody]ViewCrudGoalPersonControl view)
    {
      return service.UpdateGoalsPersonControl(view);
    }


    /// <summary>
    /// Excluir um objetivo da empresa num período específico
    /// </summary>
    /// <param name="idperson">Identificador do objetivo da empresa no período</param>
    /// <param name="idperiod">Identificador do objetivo da empresa no período</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deletegoalspersoncontrol/{idperson}/{idperiod}")]
    public async Task<string> DeleteGoalsPersonControl(string idperson, string idperiod)
    {
      return service.DeleteGoalsPersonControl(idperson, idperiod);
    }
    #endregion
  }
}