using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessList;
using Manager.Views.BusinessNew;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Tools;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador de Contas de Clientes
  /// </summary>
  [Produces("application/json")]
  [Route("account")]
  public class AccountController : Controller
  {
    private readonly IServiceAccount service;
    private readonly IServiceLog serviceLog;

    #region Constructor
    /// <summary>
    /// Construtor do controlador de contas do cliente
    /// </summary>
    /// <param name="_service">Serviço de contas do cliente</param>
    /// <param name="_serviceLog">Serviço de Logs</param>
    public AccountController(IServiceAccount _service, IServiceLog _serviceLog)
    {
      service = _service;
      serviceLog = _serviceLog;
    }
    #endregion

    #region Account
    /// <summary>
    /// Incluir uma nova conta de cliente
    /// </summary>
    /// <param name="view">Objeto para nova conta</param>
    /// <returns></returns>
    [HttpPost]
    [Route("new")]
    public IActionResult Post([FromBody]ViewNewAccount view)
    {
      return Ok(service.NewAccount(view));
    }
    /// <summary>
    /// Buscar todas as contas ativas
    /// </summary>
    /// <param name="count">Quantidade de registros por página</param>
    /// <param name="page">Número da página</param>
    /// <param name="filter">Filtro para nome da conta</param>
    /// <returns>Lista de contas ativas</returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<ViewListAccount> GetAll(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetAll(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    #endregion

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