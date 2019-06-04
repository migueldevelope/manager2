﻿using System;
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
    public async Task<string> AddCompany([FromBody] ViewCrudCompanyMandatory view)
    {
      return await service.AddCompany(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addoccuaption")]
    public async Task<string> AddOccuaption([FromBody]ViewCrudOccupationMandatory view)
    {
      return await service.AddOccupation(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addperson")]
    public async Task<string> AddPerson([FromBody]ViewCrudPersonMandatory view)
    {
      return await service.AddPerson(view);
    }


    [Authorize]
    [HttpGet]
    [Route("listcompany/{idcourse}")]
    public async Task<List<ViewListCompany>> ListCompany(string idcourse, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCompany(idcourse, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }

    [Authorize]
    [HttpGet]
    [Route("listoccupation/{idcourse}/{idcompany}")]
    public async Task<List<ViewListOccupation>> ListOccupation(string idcourse, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOccupation(idcourse, idcompany, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }

    [Authorize]
    [HttpGet]
    [Route("trainingplanlist/{idmanager}/{type}/{origin}")]
    public async Task<List<ViewTrainingPlanList>> ListTrainingPlanPersonList(string idmanager, EnumTypeUser type, EnumOrigin origin, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTrainingPlanPersonList(idmanager, type, origin, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }

    [Authorize]
    [HttpGet]
    [Route("listperson/{idcourse}/{idcompany}")]
    public async Task<List<ViewListPerson>> ListPerson(string idcourse, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPerson(idcourse, idcompany, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }


    [Authorize]
    [HttpGet]
    [Route("listtrainingplanperson/{idperson}")]
    public async Task<List<ViewTrainingPlan>> ListTrainingPlanPerson(string idperson, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListTrainingPlanPerson(idperson, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }


    [Authorize]
    [HttpDelete]
    [Route("removecompany/{idcourse}/{idcompany}")]
    public async Task<string> RemoveCompany(string idcourse, string idcompany)
    {
      return await service.RemoveCompany(idcourse, idcompany);
    }

    [Authorize]
    [HttpDelete]
    [Route("removeoccupation/{idcourse}/{idoccupation}")]
    public async Task<string> RemoveOccupation(string idcourse, string idoccupation)
    {
      return await service.RemoveOccupation(idcourse, idoccupation);
    }

    [Authorize]
    [HttpDelete]
    [Route("removeperson/{idcourse}/{idperson}")]
    public async Task<string> RemovePerson(string idcourse, string idperson)
    {
      return await service.RemovePerson(idcourse, idperson);
    }

    [Authorize]
    [HttpDelete]
    [Route("removetrainingplan/{id}")]
    public async Task<string> RemoveTrainingPlan(string id)
    {
      return await service.RemoveTrainingPlan(id);
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
      return await service.UpdateTrainingPlan(view);
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
      var result = service.ListTrainingPlan(idcompany, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
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
      var result = service.ListTrainingPlan(idcompany, iduser, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
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
      return await service.NewTrainingPlan(view);
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
      return await service.GetTrainingPlan(id);
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
      return await service.GetMandatoryTraining(idcourse);
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
      var result = service.List(count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }

    #endregion



  }
}