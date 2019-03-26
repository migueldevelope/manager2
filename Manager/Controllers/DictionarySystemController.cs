using System.Collections.Generic;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador do dicionário do sistema
  /// </summary>
  [Produces("application/json")]
  [Route("dictionarysystem")]
  public class DictionarySystemController : Controller
  {
    private readonly IServiceDictionarySystem service;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Servio do dicionário do sistema</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public DictionarySystemController(IServiceDictionarySystem _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region DictionarySystem
    /// <summary>
    /// Adicionar um novo dicionario de dados
    /// </summary>
    /// <param name="view">View do dicionario de dados</param>
    /// <returns></returns>
    [HttpPost]
    [Route("new")]
    public string Post([FromBody]ViewCrudDictionarySystem view)
    {
      return service.New(view);
    }
    /// <summary>
    /// Listar os dicionarios do sistema
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do dicionário</param>
    /// <returns>Lista de dicionários</returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<ViewListDictionarySystem> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListDictionarySystem> result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Buscar o dicionario para alteração
    /// </summary>
    /// <param name="id">Identificador do dicionário</param>
    /// <returns>Objeto de manutenção do dicionário</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public ViewCrudDictionarySystem List(string id)
    {
      return service.Get(id);
    }
    /// <summary>
    /// Buscar o dicionário por nome
    /// </summary>
    /// <param name="name">Nome para buscar</param>
    /// <returns>Retorna o dicionário para utilização</returns>
    [Authorize]
    [HttpGet]
    [Route("getname/{name}")]
    public ViewListDictionarySystem GetName(string name)
    {
      return service.GetName(name);
    }
    /// <summary>
    /// Alteração de dicionário do sistema
    /// </summary>
    /// <param name="view">Objeto de manutenção do dicionário</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody]ViewCrudDictionarySystem view)
    {
      return Ok(service.Update(view));
    }
    /// <summary>
    /// Apagar um dicionário de sistema
    /// </summary>
    /// <param name="id">Identificador do dicionário</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public IActionResult Delete(string id)
    {
      return Ok(service.Remove(id));
    }
    #endregion

    #region Old
    [HttpPost]
    [Route("newlist")]
    public string Post([FromBody]List<ViewListDictionarySystem> list)
    {
      return service.New(list);
    }
    #endregion

  }
}