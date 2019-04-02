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
  /// Controlador de Parâmetros
  /// </summary>
  [Produces("application/json")]
  [Route("parameters")]
  public class ParametersController : Controller
  {
    private readonly IServiceParameters service;

    #region Constructor
    /// <summary>
    /// Contrutor de parâmetros
    /// </summary>
    /// <param name="_service">Serviço de parâmetros</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public ParametersController(IServiceParameters _service, IHttpContextAccessor contextAccessor)
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
    public IActionResult New([FromBody]ViewCrudParameter view)
    {
      return Ok(service.New(view));
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
    public List<ViewListParameter> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListParameter> result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Buscar um parâmetro para alteração
    /// </summary>
    /// <param name="id">Identificador do parâmetro</param>
    /// <returns>Objeto de manutenão do parâmetro</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public ViewCrudParameter Get(string id)
    {
      return service.Get(id);
    }
    /// <summary>
    /// Buscar um parâmetro pela chave interna
    /// </summary>
    /// <param name="key">Chave para pesquisar</param>
    /// <returns>Objeto de parâmetro para manutenção</returns>
    [Authorize]
    [HttpGet]
    [Route("getkey/{key}")]
    public ViewCrudParameter GetName(string key)
    {
      return service.GetKey(key);
    }
    /// <summary>
    /// Alterar um parâmetro
    /// </summary>
    /// <param name="view">Objeto de manutenção do parâmetro</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody]ViewCrudParameter view)
    {
      return Ok(service.Update(view));
    }
    /// <summary>
    /// Excluir um parâmetro
    /// </summary>
    /// <param name="id">Identificador do parâmetro</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public IActionResult Delete(string id)
    {
      return Ok(service.Delete(id));
    }
    #endregion

    #region Old
    [HttpPost]
    [Route("old/new")]
    public string PostOld([FromBody]Parameter view)
    {
      return service.NewOld(view);
    }

    [Authorize]
    [HttpGet]
    [Route("old/list")]
    public List<Parameter> ListOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOld(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/get/{id}")]
    public Parameter ListOld(string id)
    {
      return service.GetOld(id);
    }

    [Authorize]
    [HttpGet]
    [Route("old/getname/{name}")]
    public Parameter GetNameOld(string name)
    {
      return service.GetNameOld(name);
    }

    [Authorize]
    [HttpPut]
    [Route("old/update")]
    public string UpdateOld([FromBody]Parameter view)
    {
      return service.UpdateOld(view);
    }
    #endregion

  }
}