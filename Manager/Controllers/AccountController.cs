using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Tools;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("account")]
  public class AccountController : Controller
  {
    private readonly IServiceAccount service;
    private readonly IServiceLog serviceLog;

    public AccountController(IServiceAccount _service, IServiceLog _serviceLog)
    {
      service = _service;
      serviceLog = _serviceLog;
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

    [Authorize]
    [HttpPut]
    [Route("alteraccount/{idaccount}")]
    public ViewPerson AlterAccount(string idaccount)
    {
      var conn = ConnectionNoSqlService.GetConnetionServer();
      return service.AlterAccount(idaccount, conn.TokenServer);
    }

    [Authorize]
    [HttpPut]
    [Route("alteraccountperson/{idperson}")]
    public ViewPerson AlterAccountPerson(string idperson)
    {
      var conn = ConnectionNoSqlService.GetConnetionServer();
      return service.AlterAccountPerson(idperson, conn.TokenServer);
    }

    [Authorize]
    [HttpGet]
    [Route("getlogs/{idaccount}")]
    public List<Log> GetLogs(string idaccount, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = serviceLog.GetLogs(idaccount, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;

    }

  }
}