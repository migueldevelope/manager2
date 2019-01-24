using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("dictionarysystem")]
  public class DictionarySystemController : Controller
  {
    private readonly IServiceDictionarySystem service;

    public DictionarySystemController(IServiceDictionarySystem _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [HttpPost]
    [Route("new")]
    public string Post([FromBody]DictionarySystem view)
    {
      return service.New(view);
    }

    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<DictionarySystem> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public DictionarySystem List(string id)
    {
      return service.Get(id);
    }

    [Authorize]
    [HttpGet]
    [Route("getname/{name}")]
    public DictionarySystem GetName(string name)
    {
      return service.GetName(name);
    }

    [Authorize]
    [HttpPut]
    [Route("update")]
    public string Update([FromBody]DictionarySystem view)
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
  }
}