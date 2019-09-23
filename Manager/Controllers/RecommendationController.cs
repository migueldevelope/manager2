using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador da reconhecimento
  /// </summary>
  [Produces("application/json")]
  [Route("recommendation")]
  public class RecommendationController : DefaultController
  {
    private readonly IServiceRecommendation service;

    #region Constructor
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">Serviço da reconhecimento</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public RecommendationController(IServiceRecommendation _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Recommendation
    /// <summary>
    /// Listar as reconhecimentos
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome da reconhecimento</param>
    /// <returns>Lista de reconhecimentos cadastradas</returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public async Task<List<ViewListRecommendation>> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListRecommendation> result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Cadastrar uma nova reconhecimento
    /// </summary>
    /// <param name="view">Objeto de cadastro da reconhecimento</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("new")]
    public async Task<IActionResult> Post([FromBody]ViewCrudRecommendation view)
    {
      return await Task.Run(() => Ok(service.New(view)));
    }

    /// <summary>
    /// exporta reconhecimentos
    /// </summary>
    /// <param name="view">Objeto de cadastro da reconhecimento</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("exportrecommendation")]
    public async Task<List<ViewExportRecommendation>> ExportRecommendation([FromBody]ViewFilterIdAndDate view)
    {
      return await Task.Run(() => service.ExportRecommendation(view));
    }

    /// <summary>
    /// Retorar a reconhecimento para manutenção
    /// </summary>
    /// <param name="id">Identificador da reconhecimento</param>
    /// <returns>Objeto de manutenção da reconhecimento</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudRecommendation> Get(string id)
    {
      return await Task.Run(() => service.Get(id));
    }
    /// <summary>
    /// Alterar a reconhecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção da reconhecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update([FromBody]ViewCrudRecommendation view)
    {
      return await Task.Run(() => Ok(service.Update(view)));
    }
    /// <summary>
    /// Excluir uma reconhecimento
    /// </summary>
    /// <param name="id">Identificação da reconhecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      return await Task.Run(() => Ok(service.Delete(id)));
    }
    #endregion

    #region RecommendationPerson

    /// <summary>
    /// Listar as reconhecimentos
    /// </summary>
    /// <param name="idperson">id pessoa</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome da reconhecimento</param>
    /// <returns>Lista de reconhecimentos cadastradas</returns>
    [Authorize]
    [HttpGet]
    [Route("listrecommendationpersonid/{idperson}")]
    public async Task<List<ViewListRecommendationPersonId>> List(string idperson, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListRecommendationPersonId> result = service.ListRecommendationPersonId(idperson, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Listar reconhecimento de pessoas
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do reconhecimento de pessoa</param>
    /// <returns>Lista de reconhecimento de pessoas</returns>
    [Authorize]
    [HttpGet]
    [Route("listrecommendationperson")]
    public async Task<List<ViewListRecommendationPerson>> ListRecommendationPerson(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListRecommendationPerson> result = service.ListRecommendationPerson(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Listar reconhecimento de pessoa de uma reconhecimento
    /// </summary>
    /// <param name="idrecommendation">Identificador da reconhecimento</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do reconhecimento de pessoa</param>
    /// <returns>Lista de reconhecimento de pessoas da reconhecimento</returns>
    [Authorize]
    [HttpGet]
    [Route("listrecommendationperson/{idrecommendation}")]
    public async Task<List<ViewListRecommendationPerson>> ListRecommendationPerson(string idrecommendation, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListRecommendationPerson> result = service.ListRecommendationPerson(idrecommendation, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Novo reconhecimento de pessoa
    /// </summary>
    /// <param name="view">Objeto de manutenção do reconhecimento de pessoa</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("newrecommendationperson")]
    public async Task<IActionResult> PostRecommendationPerson([FromBody]ViewCrudRecommendationPerson view)
    {
      return await Task.Run(() => Ok(service.NewRecommendationPerson(view)));
    }

    /// <summary>
    /// Listar pessoa 
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do reconhecimento de pessoa</param>
    /// <returns>Lista de reconhecimento de pessoas da reconhecimento</returns>
    [Authorize]
    [HttpGet]
    [Route("listperson")]
    public async Task<List<ViewListPersonBase>> ListPerson(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListPersonBase> result = service.ListPerson(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    #endregion

  }
}