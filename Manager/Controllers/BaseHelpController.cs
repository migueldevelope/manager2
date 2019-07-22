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
  /// Controlador da base de conhecimento
  /// </summary>
  [Produces("application/json")]
  [Route("basehelp")]
  public class BaseHelpController : DefaultController
  {
    private readonly IServiceBaseHelp service;

    #region Constructor
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">Serviço da base de conhecimento</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public BaseHelpController(IServiceBaseHelp _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region BaseHelp
    /// <summary>
    /// Listar as base de conhecimentos
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome da base de conhecimento</param>
    /// <returns>Lista de base de conhecimentos cadastradas</returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public async Task<List<ViewListBaseHelp>> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListBaseHelp> result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Cadastrar uma nova base de conhecimento
    /// </summary>
    /// <param name="view">Objeto de cadastro da base de conhecimento</param>
    /// <returns></returns>
    [HttpPost]
    [Route("new")]
    public async Task<IActionResult> Post([FromBody]ViewCrudBaseHelp view)
    {
      return await Task.Run(() => Ok(service.New(view)));
    }
    /// <summary>
    /// Retorar a base de conhecimento para manutenção
    /// </summary>
    /// <param name="id">Identificador da base de conhecimento</param>
    /// <returns>Objeto de manutenção da base de conhecimento</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudBaseHelp> Get(string id)
    {
      return await Task.Run(() => service.Get(id));
    }


    /// <summary>
    /// Alterar a base de conhecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção da base de conhecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update([FromBody]ViewCrudBaseHelp view)
    {
      return await Task.Run(() => Ok(service.Update(view)));
    }

    /// <summary>
    /// Alterar a base de conhecimento
    /// </summary>
    /// <param name="id">Objeto de manutenção da base de conhecimento</param>
    /// <param name="link">Objeto de manutenção da base de conhecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updatelink/{link}/{id}")]
    public async Task<IActionResult> UpdateLink(string link, string id)
    {
      return await Task.Run(() => Ok(service.UpdateLink(link, id)));
    }

    /// <summary>
    /// Alterar a base de conhecimento
    /// </summary>
    /// <param name="id">identificador</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("count/{id}")]
    public async Task<IActionResult> Count(string id)
    {
      return await Task.Run(() => Ok(service.Count(id)));
    }

    /// <summary>
    /// Excluir uma base de conhecimento
    /// </summary>
    /// <param name="id">Identificação da base de conhecimento</param>
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