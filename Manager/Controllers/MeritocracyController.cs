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
  [Route("meritocracy")]
  public class MeritocracyController : Controller
  {
    private readonly IServiceMeritocracy service;

    #region Constructor
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">Serviço da empresa</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public MeritocracyController(IServiceMeritocracy _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Meritocracy
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
    public async Task<List<ViewListMeritocracy>> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListMeritocracy> result = service.List(ref total, count, page, filter);
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
    public async Task<IActionResult> Post([FromBody]ViewCrudMeritocracy view)
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
    public async Task<ViewCrudMeritocracy> Get(string id)
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
    public async Task<IActionResult> Update([FromBody]ViewCrudMeritocracy view)
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
    public async Task<IActionResult> Delete(string id)
    {
      return Ok(service.Delete(id));
    }
    #endregion

    #region MeritocracyScore
    /// <summary>
    /// Listar estabelecimentos
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do estabelecimento</param>
    /// <returns>Lista de estabelecimentos</returns>
    [Authorize]
    [HttpGet]
    [Route("listmeritocracyccore")]
    public async Task<List<ViewListMeritocracyScore>> ListMeritocracyScore(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListMeritocracyScore> result = service.ListMeritocracyScore(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Novo estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("newmeritocracyccore")]
    public async Task<IActionResult> PostMeritocracyScore([FromBody]ViewCrudMeritocracyScore view)
    {
      return Ok(service.NewMeritocracyScore(view));
    }
    /// <summary>
    /// Buscar estabelecimento para manutenção
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Objeto de manutenção do estabelecimento</returns>
    [Authorize]
    [HttpGet]
    [Route("getmeritocracyccore/{id}")]
    public async Task<ViewCrudMeritocracyScore> ListMeritocracyScore(string id)
    {
      return service.GetMeritocracyScore(id);
    }
    /// <summary>
    /// Alterar o estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updatemeritocracyccore")]
    public async Task<IActionResult> UpdateMeritocracyScore([FromBody]ViewCrudMeritocracyScore view)
    {
      return Ok(service.UpdateMeritocracyScore(view));
    }
    /// <summary>
    /// Deletar um estabelecimento
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletemeritocracyccore/{id}")]
    public async Task<IActionResult> DeleteMeritocracyScore(string id)
    {
      return Ok(service.RemoveMeritocracyScore(id));
    }
    #endregion

    #region SalaryScaleScore
    /// <summary>
    /// Listar estabelecimentos
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do estabelecimento</param>
    /// <returns>Lista de estabelecimentos</returns>
    [Authorize]
    [HttpGet]
    [Route("listmeritocracyccore")]
    public async Task<List<ViewCrudSalaryScaleScore>> ListSalaryScaleScore(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewCrudSalaryScaleScore> result = service.ListSalaryScaleScore(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Novo estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("newmeritocracyccore")]
    public async Task<IActionResult> PostSalaryScaleScore([FromBody]ViewCrudSalaryScaleScore view)
    {
      return Ok(service.NewSalaryScaleScore(view));
    }
    /// <summary>
    /// Buscar estabelecimento para manutenção
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Objeto de manutenção do estabelecimento</returns>
    [Authorize]
    [HttpGet]
    [Route("getmeritocracyccore/{id}")]
    public async Task<ViewCrudSalaryScaleScore> ListSalaryScaleScore(string id)
    {
      return service.GetSalaryScaleScore(id);
    }
    /// <summary>
    /// Alterar o estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updatemeritocracyccore")]
    public async Task<IActionResult> UpdateSalaryScaleScore([FromBody]ViewCrudSalaryScaleScore view)
    {
      return Ok(service.UpdateSalaryScaleScore(view));
    }
    /// <summary>
    /// Deletar um estabelecimento
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletemeritocracyccore/{id}")]
    public async Task<IActionResult> DeleteSalaryScaleScore(string id)
    {
      return Ok(service.DeleteSalaryScaleScore(id));
    }
    #endregion
  }
}