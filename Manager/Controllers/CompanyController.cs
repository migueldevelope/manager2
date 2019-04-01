using System.Collections.Generic;
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
  /// Controlador da empresa
  /// </summary>
  [Produces("application/json")]
  [Route("company")]
  public class CompanyController : Controller
  {
    private readonly IServiceCompany service;

    #region Constructor
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">Serviço da empresa</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public CompanyController(IServiceCompany _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Company
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
    public List<ViewListCompany> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListCompany> result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Cadastrar uma nova empresa
    /// </summary>
    /// <param name="view">Objeto de cadastro da empresa</param>
    /// <returns></returns>
    [HttpPost]
    [Route("new")]
    public IActionResult Post([FromBody]ViewCrudCompany view)
    {
      return Ok(service.New(view));
    }
    /// <summary>
    /// Retorar a empresa para manutenção
    /// </summary>
    /// <param name="id">Identificador da empresa</param>
    /// <returns>Objeto de manutenção da empresa</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public ViewCrudCompany Get(string id)
    {
      return service.Get(id);
    }
    /// <summary>
    /// Alterar a empresa
    /// </summary>
    /// <param name="view">Objeto de manutenção da empresa</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody]ViewCrudCompany view)
    {
      return Ok(service.Update(view));
    }
    /// <summary>
    /// Excluir uma empresa
    /// </summary>
    /// <param name="id">Identificação da empresa</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public IActionResult Delete(string id)
    {
      return Ok(service.Delete(id));
    }
    #endregion

    #region Establishment
    /// <summary>
    /// Listar estabelecimentos
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do estabelecimento</param>
    /// <returns>Lista de estabelecimentos</returns>
    [Authorize]
    [HttpGet]
    [Route("listestablishment")]
    public List<ViewListEstablishment> ListEstablishment(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListEstablishment> result = service.ListEstablishment(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Listar estabelecimento de uma empresa
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do estabelecimento</param>
    /// <returns>Lista de estabelecimentos da empresa</returns>
    [Authorize]
    [HttpGet]
    [Route("listestablishment/{idcompany}")]
    public List<ViewListEstablishment> ListEstablishment(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListEstablishment> result = service.ListEstablishment(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Novo estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("newestablishment")]
    public IActionResult PostEstablishment([FromBody]ViewCrudEstablishment view)
    {
      return Ok(service.NewEstablishment(view));
    }
    /// <summary>
    /// Buscar estabelecimento para manutenção
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Objeto de manutenção do estabelecimento</returns>
    [Authorize]
    [HttpGet]
    [Route("getestablishment/{id}")]
    public ViewCrudEstablishment ListEstablishment(string id)
    {
      return service.GetEstablishment(id);
    }
    /// <summary>
    /// Alterar o estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updateestablishment")]
    public IActionResult UpdateEstablishment([FromBody]ViewCrudEstablishment view)
    {
      return Ok(service.UpdateEstablishment(view));
    }
    /// <summary>
    /// Deletar um estabelecimento
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteestablishment/{id}")]
    public IActionResult DeleteEstablishment(string id)
    {
      return Ok(service.RemoveEstablishment(id));
    }
    #endregion

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