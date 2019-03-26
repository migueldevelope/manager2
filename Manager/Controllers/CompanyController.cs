using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador do cadastro de empresas
  /// </summary>
  [Produces("application/json")]
  [Route("company")]
  public class CompanyController : Controller
  {
    private readonly IServiceCompany service;

    #region Constructor
    public CompanyController(IServiceCompany _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    #endregion


    /// <summary>
    /// Inclusão de nova empresa
    /// </summary>
    /// <param name="view">Objeto do CRUD</param>
    /// <returns></returns>
    [HttpPost]
    [Route("new")]
    public string Post([FromBody]ViewCrudCompany view)
    {
      return service.New(view);
    }

    /// <summary>
    /// Lista as empresas
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<ViewListCompany> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Busca informações para editar uma empresa
    /// </summary>
    /// <param name="id">Identificador da empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public ViewCrudCompany List(string id)
    {
      return service.Get(id);
    }

    /// <summary>
    /// Atualiza informações de uma empresa
    /// </summary>
    /// <param name="view">Objeto do CRUD</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public string Update([FromBody]ViewCrudCompany view)
    {
      return service.Update(view);
    }

    /// <summary>
    /// Excluí uma empresa
    /// </summary>
    /// <param name="id">Identificador da empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public string Delete(string id)
    {
      return service.Remove(id);
    }

    /// <summary>
    /// Inclui um novo estabelecimento
    /// </summary>
    /// <param name="view">Objeto do CRUD</param>
    /// <returns></returns>
    [HttpPost]
    [Route("newestablishment")]
    public string PostEstablishment([FromBody]ViewCrudEstablishment view)
    {
      return service.NewEstablishment(view);
    }

    /// <summary>
    /// Lista os estabelecimentos filtrando por empresa
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listestablishment/{idcompany}")]
    public List<ViewListEstablishment> ListEstablishment(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEstablishment(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Lista os estabelecimentos
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listestablishment")]
    public List<ViewListEstablishment> ListEstablishment(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEstablishment(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Busca informações para editar um estabelecimento
    /// </summary>
    /// <param name="id">Identificador</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getestablishment/{id}")]
    public ViewCrudEstablishment ListEstablishment(string id)
    {
      return service.GetEstablishment(id);
    }


    /// <summary>
    /// Atualiza informações de um estabelecimento
    /// </summary>
    /// <param name="view">Objeto do CRUD</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateestablishment")]
    public string UpdateEstablishment([FromBody]ViewCrudEstablishment view)
    {
      return service.UpdateEstablishment(view);
    }

    /// <summary>
    /// Exclui um estabelecimento
    /// </summary>
    /// <param name="id">Identificador</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteestablishment/{id}")]
    public string DeleteEstablishment(string id)
    {
      return service.RemoveEstablishment(id);
    }


    #region Old
    [HttpPost]
    [Route("old/new")]
    public string PostOld([FromBody]Company view)
    {
      return service.NewOld(view);
    }

    [Authorize]
    [HttpGet]
    [Route("old/list")]
    public List<Company> ListOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOld(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/get/{id}")]
    public Company ListOld(string id)
    {
      return service.GetOld(id);
    }

    [Authorize]
    [HttpPut]
    [Route("old/update")]
    public string UpdateOld([FromBody]Company view)
    {
      return service.UpdateOld(view);
    }


    [HttpPost]
    [Route("old/newestablishment")]
    public string PostEstablishmentOld([FromBody]Establishment view)
    {
      return service.NewEstablishmentOld(view);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listestablishment/{idcompany}")]
    public List<Establishment> ListEstablishmentOld(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEstablishmentOld(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/listestablishment")]
    public List<Establishment> ListEstablishmentOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEstablishmentOld(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/getestablishment/{id}")]
    public Establishment ListEstablishmentOld(string id)
    {
      return service.GetEstablishmentOld(id);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updateestablishment")]
    public string UpdateEstablishmentOld([FromBody]Establishment view)
    {
      return service.UpdateEstablishmentOld(view);
    }

    #endregion

  }
}