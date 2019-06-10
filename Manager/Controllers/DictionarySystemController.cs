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
  /// Controlador do dicionário do sistema
  /// </summary>
  [Produces("application/json")]
  [Route("dictionarysystem")]
  public class DictionarySystemController : DefaultController
  {
    private readonly IServiceDictionarySystem service;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Servio do dicionário do sistema</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public DictionarySystemController(IServiceDictionarySystem _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
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
    [Authorize]
    [HttpPost]
    [Route("new")]
    public async Task<string> New([FromBody]ViewCrudDictionarySystem view)
    {
      return await service.New(view);
    }
    /// <summary>
    /// Incluir vários discionários de dados ao mesmo tempo
    /// </summary>
    /// <param name="list">Lista de objetos de discionário para incluir</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("newlist")]
    public async Task<string> NewList([FromBody]List<ViewListDictionarySystem> list)
    {
      return await service.New(list);
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
    public async Task<List<ViewListDictionarySystem>> List( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListDictionarySystem> result = await service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return  result;
    }
    /// <summary>
    /// Buscar o dicionario para alteração
    /// </summary>
    /// <param name="id">Identificador do dicionário</param>
    /// <returns>Objeto de manutenção do dicionário</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudDictionarySystem> Get(string id)
    {
      return await service.Get(id);
    }
    /// <summary>
    /// Buscar o dicionário por nome
    /// </summary>
    /// <param name="name">Nome para buscar</param>
    /// <returns>Retorna o dicionário para utilização</returns>
    [Authorize]
    [HttpGet]
    [Route("getname/{name}")]
    public async Task<ViewListDictionarySystem> GetName(string name)
    {
      return await service.GetName(name);
    }
    /// <summary>
    /// Alteração de dicionário do sistema
    /// </summary>
    /// <param name="view">Objeto de manutenção do dicionário</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update([FromBody]ViewCrudDictionarySystem view)
    {
      return Ok(await  service.Update(view));
    }
    /// <summary>
    /// Apagar um dicionário de sistema
    /// </summary>
    /// <param name="id">Identificador do dicionário</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      return Ok(await  service.Delete(id));
    }
    #endregion

  }
}