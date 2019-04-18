using Manager.Core.Interfaces;
using Manager.Views.Audit;
using Manager.Views.BusinessCrud;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegrationServer.Controllers
{
  /// <summary>
  /// Controlador de Configuração da Integração
  /// </summary>
  [Produces("application/json")]
  [Route("audit")]
  public class AuditController : Controller
  {
    private readonly IServiceAudit service;

    #region Constructor
    /// <summary>
    /// Inicializador do controlador de configuração da integração
    /// </summary>
    /// <param name="_service">Serviço de Integração</param>
    /// <param name="contextAccessor">Token de autenticação</param>
    public AuditController(IServiceAudit _service, IHttpContextAccessor contextAccessor)
    {
      try
      {
        service = _service;
        service.SetUser(contextAccessor);
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #region User
    /// <summary>
    /// Retornar uma lista crud dos usuários da conta
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("user")]
    public List<ViewAuditUser> ListUser()
    {
      var result = service.ListUser();
      // Gravar arquivo CSV
      string csv = Tools.CSVService.ToCsv(result, "", "");
      System.IO.File.WriteAllText("export/user.csv", csv);
      //
      return result;
    }
    #endregion

    #region Person
    /// <summary>
    /// Retornar uma lista crud das pessoas da conta
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("person")]
    public List<ViewAuditPerson> ListPerson()
    {
      var result = service.ListPerson();
      // Gravar arquivo CSV
      string csv = Tools.CSVService.ToCsv(result, "", "");
      System.IO.File.WriteAllText("export/person.csv", csv);
      //
      return result;
    }
    #endregion

  }
}
