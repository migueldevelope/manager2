using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador da empresa
  /// </summary>
  [Produces("application/json")]
  [Route("feelingday")]
  public class FeelingDayController : DefaultController
  {
    private readonly IServiceFeelingDay service;

    #region Constructor
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">Serviço da empresa</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public FeelingDayController(IServiceFeelingDay _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region FeelingDay
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
    public async Task<List<ViewListFeelingDay>> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListFeelingDay> result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Cadastrar uma nova empresa
    /// </summary>
    /// <param name="feeling">Objeto de cadastro da empresa</param>
    /// <returns></returns>
    [HttpPost]
    [Route("new/{feeling}")]
    public async Task<IActionResult> Post(EnumFeeling feeling)
    {
      return await Task.Run(() => Ok(service.New(feeling)));
    }

    /// <summary>
    /// Retorar a empresa para manutenção
    /// </summary>
    /// <param name="id">Identificador da empresa</param>
    /// <returns>Objeto de manutenção da empresa</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudFeelingDay> Get(string id)
    {
      return await Task.Run(() => service.Get(id));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmanager"></param>
    /// <param name="days"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getquantity")]
    public async Task<List<ViewFeelingQtd>> GetQuantity(string idmanager = "", long days = 7)
    {
      return await Task.Run(() => service.GetQuantity(idmanager, days));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmanager"></param>
    /// <param name="days"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getmanager/{idmanager}")]
    public async Task<List<ViewFeelingManager>> GetManager(string idmanager, long days = 7)
    {
      return await Task.Run(() => service.GetManager(idmanager, days));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getfeelingday")]
    public async Task<ViewCrudFeelingDay> GetFeeelingDay()
    {
      return await Task.Run(() => service.GetFeeelingDay());
    }

    /// <summary>
    /// Alterar a empresa
    /// </summary>
    /// <param name="view">Objeto de manutenção da empresa</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update([FromBody]ViewCrudFeelingDay view)
    {
      return await Task.Run(() => Ok(service.Update(view)));
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
      return await Task.Run(() => Ok(service.Delete(id)));
    }
    #endregion


  }
}