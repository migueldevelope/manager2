using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Tools;

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
    [Route("n/list/{type}")]
    public List<ViewListPersonCrud> GetList(EnumTypeUser type, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetPersons(ref total, count, page, filter, type);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }


    [Authorize]
    [HttpGet]
    [Route("n/listsalaryscale/{idoccupation}")]
    public List<SalaryScalePerson> ListSalaryScale(string idoccupation)
    {
      return service.ListSalaryScale(idoccupation);
    }

    /// <summary>
    /// Buscar informações da pessoa para alteração
    /// </summary>
    /// <param name="id">Identificador da pessoa</param>
    /// <returns>Objeto de alteração da pessoa</returns>
    [Authorize]
    [HttpGet]
    [Route("n/edit/{idperson}")]
    public ViewCrudPerson GetEdit(string id)
    {
      return service.GetPersonCrud(id);
    }
    /// <summary>
    /// Incluir uma nova pessoa
    /// </summary>
    /// <param name="view">Objeto de manutenção da pessoa</param>
    /// <returns>Objeto da pessoa incluído</returns>
    [Authorize]
    [HttpPost]
    [Route("n/new")]
    public ViewCrudPerson Post([FromBody] ViewCrudPerson view)
    {
      return service.NewPerson(view);
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
    [Route("directteam/{idPerson}")]
    public List<ViewPersonTeam> GetPersonTeam(string idPerson, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetPersonTeam(ref total, idPerson, filter, count, page);
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
    public List<Person> ListPersons(string idcompany, string filter = "")
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
    [Route("list/{type}")]
    public List<Person> ListOld(EnumTypeUser type, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetPersonsCrud(type, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("{idperson}/edit")]
    public Person GetEditOld(string idperson)
    {
      return service.GetPersonCrudOld(idperson); ;
    }

    [Authorize]
    [HttpPost]
    [Route("new")]
    public Person Post([FromBody] Person person)
    {
      return service.NewPersonView(person);
    }

    [Authorize]
    [HttpPut]
    [Route("update")]
    public string Put([FromBody] Person person)
    {
      service.UpdatePersonView(person);
      return "ok";
    }

    [Authorize]
    [HttpGet]
    [Route("listoccupation")]
    public List<Occupation> ListOccupation(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOccupation(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listcompany")]
    public List<Company> ListCompany(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCompany(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listmanager")]
    public List<Person> ListManager(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListManager(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpPost]
    [Route("addpersonuser")]
    public string AddPersonUser([FromBody]ViewPersonUser view)
    {
      return service.AddPersonUser(view);
    }

    [Authorize]
    [HttpPut]
    [Route("updatepersonuser")]
    public string UpdatePersonUser([FromBody]ViewPersonUser view)
    {
      return service.UpdatePersonUser(view);
    }
    #endregion

  }
}