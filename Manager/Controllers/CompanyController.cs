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
  /// Controlador da empresa
  /// </summary>
  [Produces("application/json")]
  [Route("manager/company")]
  public class CompanyController : DefaultController
  {
    private readonly IServiceCompany service;

    #region Constructor
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">Serviço da empresa</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public CompanyController(IServiceCompany _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
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
    public async Task<List<ViewListCompany>> List( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListCompany> result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Cadastrar uma nova empresa
    /// </summary>
    /// <param name="view">Objeto de cadastro da empresa</param>
    /// <returns></returns>
    [HttpPost]
    [Route("new")]
    public async Task<IActionResult> Post([FromBody]ViewCrudCompany view)
    {
      return await Task.Run(() =>Ok( service.New(view)));
    }
    /// <summary>
    /// Retorar a empresa para manutenção
    /// </summary>
    /// <param name="id">Identificador da empresa</param>
    /// <returns>Objeto de manutenção da empresa</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudCompany> Get(string id)
    {
      return await Task.Run(() =>service.Get(id));
    }
    /// <summary>
    /// Alterar a empresa
    /// </summary>
    /// <param name="view">Objeto de manutenção da empresa</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update([FromBody]ViewCrudCompany view)
    {
      return await Task.Run(() =>Ok( service.Update(view)));
    }
    /// <summary>
    /// Excluir uma empresa
    /// </summary>
    /// <param name="id">Identificação da empresa</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      return await Task.Run(() =>Ok( service.Delete(id)));
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
    public async Task<List<ViewListEstablishment>> ListEstablishment( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListEstablishment> result = service.ListEstablishment(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
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
    public async Task<List<ViewListEstablishment>> ListEstablishment(string idcompany,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListEstablishment> result = service.ListEstablishment(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Novo estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("newestablishment")]
    public async Task<IActionResult> PostEstablishment([FromBody]ViewCrudEstablishment view)
    {
      return await Task.Run(() =>Ok( service.NewEstablishment(view)));
    }
    /// <summary>
    /// Buscar estabelecimento para manutenção
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Objeto de manutenção do estabelecimento</returns>
    [Authorize]
    [HttpGet]
    [Route("getestablishment/{id}")]
    public async Task<ViewCrudEstablishment> ListEstablishment(string id)
    {
      return await Task.Run(() =>service.GetEstablishment(id));
    }
    /// <summary>
    /// Alterar o estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updateestablishment")]
    public async Task<IActionResult> UpdateEstablishment([FromBody]ViewCrudEstablishment view)
    {
      return await Task.Run(() =>Ok( service.UpdateEstablishment(view)));
    }
    /// <summary>
    /// Deletar um estabelecimento
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteestablishment/{id}")]
    public async Task<IActionResult> DeleteEstablishment(string id)
    {
      return await Task.Run(() =>Ok( service.RemoveEstablishment(id)));
    }
    #endregion

  }
}