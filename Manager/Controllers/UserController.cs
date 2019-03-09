using System;
using System.Collections.Generic;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tools;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador de Usuários 
  /// </summary>
  [Produces("application/json")]
  [Route("user")]
  public class UserController : Controller
  {
    private readonly IServiceUser service;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Serviço de usuário</param>
    /// <param name="contextAccessor">Autorização</param>
    public UserController(IServiceUser _service, IHttpContextAccessor contextAccessor)
    {
      try
      {
        service = _service;
        service.SetUser(contextAccessor);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region User
    /// <summary>
    /// Listar os usuarios de uma empresa
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listusers/{idcompany}")]
    public List<User> ListUsers(string idcompany, string filter = "")
    {
      return service.GetUsers(idcompany, filter);
    }


    [Authorize]
    [HttpGet]
    [Route("list/{type}")]
    public List<User> List(EnumTypeUser type, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetUsersCrud(type, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    #endregion


    [Authorize]
    [HttpGet]
    [Route("photo/{idUser}")]
    public string GetPhoto(string idUser)
    {
      return service.GetPhoto(idUser);
    }

    [Authorize]
    [HttpPut]
    [Route("alterpass/{idUser}")]
    public string AlterPass([FromBody]ViewAlterPass view, string idUser)
    {
      return service.AlterPassword(view, idUser);
    }


    [HttpPut]
    [Route("forgotpassword/{foreign}/alter")]
    public string ForgotPassword([FromBody]ViewAlterPass view, string foreign)
    {
      return service.AlterPasswordForgot(view, foreign);
    }

    [HttpPut]
    [Route("forgotpassword/{mail}")]
    public string ForgotPassword([FromBody]ViewForgotPassword view, string mail)
    {
      var conn = ConnectionNoSqlService.GetConnetionServer();
      var sendGridKey = conn.SendGridKey;
      return service.ForgotPassword(mail, view, sendGridKey).Result;
    }




    [Authorize]
    [HttpGet]
    [Route("{iduser}/edit")]
    public User GetEdit(string iduser)
    {
      return service.GetUserCrud(iduser); ;
    }

    [Authorize]
    [HttpPost]
    [Route("new")]
    public User Post([FromBody] User user)
    {
      return service.NewUserView(user);
    }

    [Authorize]
    [HttpPut]
    [Route("update")]
    public User Put([FromBody] User user)
    {
      return service.UpdateUserView(user);
    }

    [Authorize]
    [HttpGet]
    [Route("listoccupation")]
    public List<Occupation> ListOccupation(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOccupation(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listperson/{iduser}")]
    public List<Person> ListPerson(string iduser, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPerson(iduser, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listcompany")]
    public List<Company> ListCompany(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCompany(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listmanager")]
    public List<User> ListManager(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListManager(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    #region Scripts (apagar?)
    [HttpPut]
    [Route("scriptperson")]
    public string ScriptPerson()
    {
      return service.ScriptPerson();
    }

    [HttpPut]
    [Route("scriptonboarding")]
    public string ScriptOnBoarding()
    {
      return service.ScriptOnBoarding();
    }

    [HttpPut]
    [Route("scriptcheckpoint")]
    public string ScriptCheckpoint()
    {
      return service.ScriptCheckpoint();
    }

    [HttpPut]
    [Route("scriptmonitoring")]
    public string ScriptMonitoring()
    {
      return service.ScriptMonitoring();
    }

    [HttpPut]
    [Route("scriptlog")]
    public string ScriptLog()
    {
      return service.ScriptLog();
    }
    #endregion

  }
}