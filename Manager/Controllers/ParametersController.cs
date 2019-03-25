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

    [HttpPost]
    [Route("new")]
    public string Post([FromBody]ViewCrudParameter view)
    {
      return service.New(view);
    }

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

    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public ViewCrudParameter List(string id)
    {
      return service.Get(id);
    }

    [Authorize]
    [HttpGet]
    [Route("getname/{name}")]
    public ViewCrudParameter GetName(string name)
    {
      return service.GetName(name);
    }

    [Authorize]
    [HttpPut]
    [Route("update")]
    public string Update([FromBody]ViewCrudParameter view)
    {
      return service.Update(view);
    }


    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public string Delete(string id)
    {
      return service.Remove(id);
    }

    #region Old
    [HttpPost]
    [Route("new")]
    public string PostOld([FromBody]Parameter view)
    {
      return service.NewOld(view);
    }

    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<Parameter> ListOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOld(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public Parameter ListOld(string id)
    {
      return service.GetOld(id);
    }

    [Authorize]
    [HttpGet]
    [Route("getname/{name}")]
    public Parameter GetNameOld(string name)
    {
      return service.GetNameOld(name);
    }

    [Authorize]
    [HttpPut]
    [Route("update")]
    public string UpdateOld([FromBody]Parameter view)
    {
      return service.UpdateOld(view);
    }
    #endregion

  }
}