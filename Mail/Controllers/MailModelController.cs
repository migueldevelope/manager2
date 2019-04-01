using System.Collections.Generic;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mail.Controllers
{
  /// <summary>
  /// Controlador do modelo de e-mail
  /// </summary>
  [Produces("application/json")]
  [Route("mailmodel")]
  public class MailModelController : Controller
  {
    private readonly IServiceMailModel service;

    #region Construtor
    /// <summary>
    /// Construtor do controle
    /// </summary>
    /// <param name="_service">Serviço de modelo de e-mail</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public MailModelController(IServiceMailModel _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region MailModel
    /// <summary>
    /// Listar os modelos de e-mail cadastrados
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do e-mail</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<ViewListMailModel> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Adicionar um novo modelo de e-mail
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("new")]
    public IActionResult New([FromBody]ViewCrudMailModel view)
    {
      return Ok(service.New(view));
    }
    /// <summary>
    /// Buscar modelo de e-mail para manutenção
    /// </summary>
    /// <param name="id">Identificador do modelo de e-mail</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("get")]
    public ViewCrudMailModel Get(string id)
    {
      return service.Get(id);
    }
    /// <summary>
    /// Alteração de modelo de e-mail
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody]ViewCrudMailModel view)
    {
      return Ok(service.Update(view));
    }
    #endregion

    #region Old
    [HttpPost]
    [Route("old/new")]
    public string Post([FromBody]MailModel view)
    {
      return service.NewOld(view);
    }
    [Authorize]
    [HttpGet]
    [Route("old/list")]
    public List<MailModel> ListOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOld(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    [Authorize]
    [HttpGet]
    [Route("old/get")]
    public MailModel List(string id)
    {
      return service.GetOld(id);
    }
    [Authorize]
    [HttpPut]
    [Route("old/update")]
    public string Update([FromBody]MailModel view)
    {
      return service.UpdateOld(view);
    }
    [Authorize]
    [HttpDelete]
    [Route("old/delete/{id}")]
    public string Delete(string id)
    {
      return service.RemoveOld(id);
    }
    #endregion

  }
}