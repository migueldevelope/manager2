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
  /// Controlador da empresa
  /// </summary>
  [Produces("application/json")]
  [Route("termsofservice")]
  public class TermsOfServiceController : Controller
  {
    private readonly IServiceTermsOfService service;

    #region Constructor
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">Serviço da empresa</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public TermsOfServiceController(IServiceTermsOfService _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region TermsOfService
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
    public List<ViewListTermsOfService> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListTermsOfService> result = service.List(ref total, count, page, filter);
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
    public IActionResult Post([FromBody]ViewCrudTermsOfService view)
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
    public ViewCrudTermsOfService Get(string id)
    {
      return service.Get(id);
    }

    /// <summary>
    /// Retorar a empresa para manutenção
    /// </summary>
    /// <param name="id">Identificador da empresa</param>
    /// <returns>Objeto de manutenção da empresa</returns>
    [Authorize]
    [HttpGet]
    [Route("getterm/{id}")]
    public ViewListTermsOfService GetTerm()
    {
      return service.GetTerm();
    }

    /// <summary>
    /// Alterar a empresa
    /// </summary>
    /// <param name="view">Objeto de manutenção da empresa</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody]ViewCrudTermsOfService view)
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
    public IActionResult Delete(string id)
    {
      return Ok(service.Delete(id));
    }
    #endregion


  }
}