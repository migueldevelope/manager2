using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessNew;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador de Contas de Clientes
  /// </summary>
  [Produces("application/json")]
  [Route("account")]
  public class AccountController : DefaultController
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
    public AccountController(IServiceAccount _service, IServiceLog _serviceLog, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      serviceLog = _serviceLog;
      service.SetUser(contextAccessor);
      serviceLog.SetUser(contextAccessor);
    }
    #endregion

    #region Account
    /// <summary>
    /// Incluir uma nova conta de cliente.
    /// </summary>
    /// <param name="view">Objeto para nova conta</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ViewNewAccount view)
    {
      try
      {
        return await Task.Run(() => Ok(service.NewAccount(view)));
      }
      catch (Exception e)
      {
        return await Task.Run(() => BadRequest(e.Message));
      }
    }

    /// <summary>
    /// Atualiza uma nova de cliente
    /// </summary>
    /// <param name="view">Objeto para conta</param>
    /// /// <param name="id">id para conta</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Put([FromBody]ViewCrudAccount view, string id)
    {
      return await Task.Run(() => Ok(service.UpdateAccount(view, id)));
    }

    /// <summary>
    /// Buscar todas a conta
    /// </summary>
    /// <returns>Lista de contas ativas</returns>
    [Authorize]
    [HttpGet]
    [Route("{id}")]
    public async Task<ViewCrudAccount> Get(string id)
    {
      return await Task.Run(() => service.GetAccount(id));
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
    public async Task<List<ViewListAccount>> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetAccountList(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Trocar de conta de cliente (apenas usuário administrador)
    /// </summary>
    /// <param name="idaccount">Identificador da conta para conectar</param>
    /// <returns>Autorização de conexão da conta</returns>
    [Authorize]
    [HttpPut]
    [Route("alteraccount/{idaccount}")]
    public async Task<ViewPerson> AlterAccount(string idaccount)
    {
      return await Task.Run(() => service.AlterAccount(idaccount));
    }
    /// <summary>
    /// Trocar de pessoa conectada (apenas usuário administrador/suporte)
    /// </summary>
    /// <param name="idperson">Identificador da pessoa para conectar</param>
    /// <returns>Autorização de conexão da pessoa</returns>
    [Authorize]
    [HttpPut]
    [Route("alteraccountperson/{idperson}")]
    public async Task<ViewPerson> AlterAccountPerson(string idperson)
    {
      return await Task.Run(() => service.AlterAccountPerson(idperson));
    }
    /// <summary>
    /// Trocar de pessoa conectada (apenas usuário administrador/suporte)
    /// </summary>
    /// <returns>Autorização de conexão da pessoa</returns>
    [Authorize]
    [HttpPut]
    [Route("synchronize")]
    public async Task<string> Synchronize()
    {
      return await Task.Run(() => service.SynchronizeParameters());
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
    [Route("listlog/{idaccount}")]
    public async Task<List<ViewListLog>> ListLogs(string idaccount, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = serviceLog.ListLogs(idaccount, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    #endregion

  }
}