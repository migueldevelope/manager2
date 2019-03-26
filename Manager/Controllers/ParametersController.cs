using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
  /// Controlador de parametros
  /// </summary>
  [Produces("application/json")]
  [Route("parameters")]
  public class ParametersController : Controller
  {
    private readonly IServiceParameters service;

    #region Constructor
    public ParametersController(IServiceParameters _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    /// <summary>
    /// Inclusão de novo parametro 
    /// </summary>
    /// <param name="view">Objeto de CRUD</param>
    /// <returns></returns>
    [HttpPost]
    [Route("new")]
    public string Post([FromBody]ViewCrudParameter view)
    {
      return service.New(view);
    }

    /// <summary>
    /// Lista parametros
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<ViewListParameter> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Busca informações para editar parametro
    /// </summary>
    /// <param name="id">Identificador</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public ViewCrudParameter List(string id)
    {
      return service.Get(id);
    }

    /// <summary>
    /// Buscar informações para editar parametro
    /// </summary>
    /// <param name="name">Descriação do parametro</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getname/{name}")]
    public ViewCrudParameter GetName(string name)
    {
      return service.GetName(name);
    }

    /// <summary>
    /// Atualiza informações de parametro
    /// </summary>
    /// <param name="view">Objeto de CRUD</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public string Update([FromBody]ViewCrudParameter view)
    {
      return service.Update(view);
    }


    /// <summary>
    /// Exclui um parametro
    /// </summary>
    /// <param name="id">Identificador</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public string Delete(string id)
    {
      return service.Remove(id);
    }

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