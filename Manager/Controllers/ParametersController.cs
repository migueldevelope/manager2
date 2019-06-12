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
  /// Controlador de Parâmetros
  /// </summary>
  [Produces("application/json")]
  [Route("parameters")]
  public class ParametersController : DefaultController
  {
    private readonly IServiceParameters service;

    #region Constructor
    /// <summary>
    /// Contrutor de parâmetros
    /// </summary>
    /// <param name="_service">Serviço de parâmetros</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public ParametersController(IServiceParameters _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Parameters
    /// <summary>
    /// Incluir um novo parâmetro
    /// </summary>
    /// <param name="view">Objeto de manutenção de parâmetros</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("new")]
    public async Task<IActionResult> New([FromBody]ViewCrudParameter view)
    {
      return await Task.Run(() =>Ok( service.New(view)));
    }
    /// <summary>
    /// Lista de parâmetros
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do parâmetro</param>
    /// <returns>Lista de parâmetros</returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public async Task<List<ViewListParameter>> List( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListParameter> result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Buscar um parâmetro para alteração
    /// </summary>
    /// <param name="id">Identificador do parâmetro</param>
    /// <returns>Objeto de manutenão do parâmetro</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudParameter> Get(string id)
    {
      return await Task.Run(() =>service.Get(id));
    }
    /// <summary>
    /// Buscar um parâmetro pela chave interna
    /// </summary>
    /// <param name="key">Chave para pesquisar</param>
    /// <returns>Objeto de parâmetro para manutenção</returns>
    [Authorize]
    [HttpGet]
    [Route("getkey/{key}")]
    public async Task<ViewCrudParameter> GetName(string key)
    {
      return await Task.Run(() =>service.GetKey(key));
    }
    /// <summary>
    /// Alterar um parâmetro
    /// </summary>
    /// <param name="view">Objeto de manutenção do parâmetro</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update([FromBody]ViewCrudParameter view)
    {
      return await Task.Run(() =>Ok( service.Update(view)));
    }
    /// <summary>
    /// Excluir um parâmetro
    /// </summary>
    /// <param name="id">Identificador do parâmetro</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      return await Task.Run(() =>Ok( service.Delete(id)));
    }
    #endregion

  }
}