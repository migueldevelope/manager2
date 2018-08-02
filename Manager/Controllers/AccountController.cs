using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("account")]
  public class AccountController : Controller
  {
    private readonly IServiceAccount service;

    public AccountController(IServiceAccount _service)
    {
      service = _service;
    }

    [HttpPost]
    [Route("new")]
    public ObjectResult Post([FromBody]ViewNewAccount view)
    {
      service.NewAccount(view);
      return Ok("Account sucess!");
    }

    [Authorize]
    [HttpGet]
    [Route("getaccounts")]
    public List<Account> GeAccounts(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GeAccounts(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }


  }
}