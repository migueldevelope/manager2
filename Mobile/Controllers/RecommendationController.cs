using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mobile.Controllers
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
    /// Read
    /// </summary>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("readrecommendationperson/{idperson}")]
    public async Task<IActionResult> ReadRecommendationPerson(string idperson)
    {
      return await Task.Run(() => Ok(service.ReadRecommendationPerson(idperson)));
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