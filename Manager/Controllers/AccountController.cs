using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessList;
using Manager.Views.BusinessNew;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    /// <param name="contextAccessor">Autorização</param>
    public AccountController(IServiceAccount _service, IServiceLog _serviceLog, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      serviceLog = _serviceLog;
      service.SetUser(contextAccessor);
      serviceLog.SetUser(contextAccessor);
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
    /// <summary>
    /// Trocar de conta de cliente (apenas usuário administrador)
    /// </summary>
    /// <param name="idaccount">Identificador da conta para conectar</param>
    /// <returns>Autorização de conexão da conta</returns>
    [Authorize]
    [HttpPut]
    [Route("alteraccount/{idaccount}")]
    public ViewPerson AlterAccount(string idaccount)
    {
      return service.AlterAccount(idaccount);
    }
    /// <summary>
    /// Trocar de pessoa conectada (apenas usuário administrador/suporte)
    /// </summary>
    /// <param name="idperson">Identificador da pessoa para conectar</param>
    /// <returns>Autorização de conexão da pessoa</returns>
    [Authorize]
    [HttpPut]
    [Route("alteraccountperson/{idperson}")]
    public ViewPerson AlterAccountPerson(string idperson)
    {
      return service.AlterAccountPerson(idperson);
    }
    #endregion

    #region Logs
    /// <summary>
    /// Listar os logs para consulta
    /// </summary>
    /// <param name="idaccount">Identificador da conta para consultar o Log</param>
    /// <param name="count">Quantidade de registros por página</param>
    /// <param name="page">Número da Página</param>
    /// <param name="filter">Filtro de pesquisa</param>
    /// <returns>Lista de informações do Log</returns>
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
    #endregion

  }
}