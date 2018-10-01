using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
    [Produces("application/json")]
    [Route("company")]
    public class CompanyController : Controller
    {
    private readonly IServiceCompany service;

    public CompanyController(IServiceCompany _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [HttpPost]
    [Route("new")]
    public string Post([FromBody]Company view)
    {
      return service.New(view);
    }

    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<Company> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("get")]
    public Company List(string id)
    {
      return service.Get(id);
    }

    [Authorize]
    [HttpPut]
    [Route("update")]
    public string Update([FromBody]Company view)
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

    [HttpPost]
    [Route("newestablishment")]
    public string PostEstablishment([FromBody]Establishment view)
    {
      return service.NewEstablishment(view);
    }

    [Authorize]
    [HttpGet]
    [Route("listestablishment/{idcompany}")]
    public List<Establishment> ListEstablishment(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEstablishment(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("getestablishment")]
    public Establishment ListEstablishment(string id)
    {
      return service.GetEstablishment(id);
    }

    [Authorize]
    [HttpPut]
    [Route("updateestablishment")]
    public string UpdateEstablishment([FromBody]Establishment view)
    {
      return service.UpdateEstablishment(view);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteestablishment/{id}")]
    public string DeleteEstablishment(string id)
    {
      return service.RemoveEstablishment(id);
    }
  }
}