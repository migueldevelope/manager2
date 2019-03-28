using System;
using System.Collections.Generic;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
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
  /// <summary>
  /// Controlador de Pessoas
  /// </summary>
  [Produces("application/json")]
  [Route("person")]
  public class PersonController : Controller
  {
    private readonly IServicePerson service;

    #region Constructor
    /// <summary>
    /// Contrutor do controlador
    /// </summary>
    /// <param name="_service">Serviço da pessoa</param>
    /// <param name="contextAccessor">Autorização</param>
    public PersonController(IServicePerson _service, IHttpContextAccessor contextAccessor)
    {
      try
      {
        service = _service;
        service.SetUser(contextAccessor);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Person
    /// <summary>
    /// Listar pessoas da base de dados
    /// </summary>
    /// <param name="type">Tipo do usuário que está fazendo a consulta</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome da pessoa</param>
    /// <returns>Lista de pessoas da tela de manutenção</returns>
    [Authorize]
    [HttpGet]
    [Route("list/{type}")]
    public List<ViewListPersonCrud> GetList(EnumTypeUser type, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetPersons(ref total, count, page, filter, type);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Buscar informações da pessoa para alteração
    /// </summary>
    /// <param name="id">Identificador da pessoa</param>
    /// <returns>Objeto de alteração da pessoa</returns>
    [Authorize]
    [HttpGet]
    [Route("edit/{id}")]
    public ViewCrudPerson GetEdit(string id)
    {
      return service.GetPersonCrud(id);
    }
    /// <summary>
    /// Incluir uma nova pessoa
    /// </summary>
    /// <param name="view">Objeto de manutenção da pessoa</param>
    /// <returns>Objeto da pessoa incluída</returns>
    [Authorize]
    [HttpPost]
    [Route("new")]
    public ViewCrudPerson Post([FromBody] ViewCrudPerson view)
    {
      return service.NewPerson(view);
    }
    /// <summary>
    /// Alterar uma pessoa
    /// </summary>
    /// <param name="view">Objeto de manutenção da pessoa</param>
    /// <returns>Objeto da pessoa alterada</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public ViewCrudPerson Put([FromBody] ViewCrudPerson view)
    {
      return service.UpdatePerson(view);
    }

    /// <summary>
    /// Lista time de gestor
    /// </summary>
    /// <param name="idPerson">Identificador gestor</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("directteam/{idPerson}")]
    public List<ViewListPersonTeam> GetPersonTeam(string idPerson, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetPersonTeam(ref total, idPerson, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }


    /// <summary>
    /// Inclusao de usuario e contrato
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addpersonuser")]
    public string AddPersonUser([FromBody]ViewCrudPersonUser view)
    {
      return service.AddPersonUser(view);
    }

    /// <summary>
    /// Atualiza informãções de usuario e contrato
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatepersonuser")]
    public string UpdatePersonUser([FromBody]ViewCrudPersonUser view)
    {
      return service.UpdatePersonUser(view);
    }

    /// <summary>
    /// Lista as tabelas salarias filtrando o cargo
    /// </summary>
    /// <param name="idoccupation">Identificador cargo</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listsalaryscale/{idoccupation}")]
    public List<ViewListSalaryScalePerson> ListSalaryScale(string idoccupation)
    {
      return service.ListSalaryScale(idoccupation);
    }


    #endregion

    #region Person Old
    [Authorize]
    [HttpGet]
    [Route("personalinformation/{idPerson}")]
    public ViewPersonDetail GetPerson(string idPerson)
    {
      return service.GetPersonDetail(idPerson);
    }


    [Authorize]
    [HttpGet]
    [Route("old/directteam/{idPerson}")]
    public List<ViewPersonTeam> GetPersonTeamOld(string idPerson, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetPersonTeamOld(ref total, idPerson, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }


    [Authorize]
    [HttpGet]
    [Route("photo/{idPerson}")]
    public string GetPhoto(string idPerson)
    {
      return service.GetPhoto(idPerson);
    }


    [Authorize]
    [HttpGet]
    [Route("listpersons")]
    public List<ViewPersonList> ListPersons(string filter = "")
    {
      return service.GetPersons(filter);
    }

    [Authorize]
    [HttpGet]
    [Route("listpersons/{idcompany}")]
    public List<ViewListPerson> ListPersons(string idcompany, string filter = "")
    {
      return service.GetPersons(idcompany, filter);
    }

    [Authorize]
    [HttpGet]
    [Route("{idperson}/head")]
    public ViewPersonHead Head(string idperson)
    {
      return service.Head(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("old/list/{type}")]
    public List<Person> ListOld(EnumTypeUser type, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetPersonsCrud(type, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/{idperson}/edit")]
    public Person GetEditOld(string idperson)
    {
      return service.GetPersonCrudOld(idperson); ;
    }

    [Authorize]
    [HttpPost]
    [Route("old/new")]
    public Person Post([FromBody] Person person)
    {
      return service.NewPersonView(person);
    }

    [Authorize]
    [HttpPut]
    [Route("old/update")]
    public string Put([FromBody] Person person)
    {
      service.UpdatePersonView(person);
      return "ok";
    }

    [Authorize]
    [HttpGet]
    [Route("listoccupation")]
    public List<ViewListOccupation> ListOccupation(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOccupation(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listcompany")]
    public List<ViewListCompany> ListCompany(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCompany(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listmanager")]
    public List<ViewListPerson> ListManager(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListManager(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpPost]
    [Route("old/addpersonuser")]
    public string AddPersonUserOld([FromBody]ViewPersonUser view)
    {
      return service.AddPersonUserOld(view);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updatepersonuser")]
    public string UpdatePersonUserOld([FromBody]ViewPersonUser view)
    {
      return service.UpdatePersonUserOld(view);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listsalaryscale/{idoccupation}")]
    public List<SalaryScalePerson> ListSalaryScaleOld(string idoccupation)
    {
      return service.ListSalaryScaleOld(idoccupation);
    }
    #endregion

  }
}