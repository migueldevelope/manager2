using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tools;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("user")]
  public class UserController : Controller
  {
    private readonly IServiceUser service;

    public UserController(IHttpContextAccessor contextAccessor, IServiceUser _service)
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
    public string Put([FromBody] User user)
    {
      service.UpdateUserView(user);
      return "ok";
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

    [HttpPut]
    [Route("scriptperson/{idaccount}")]
    public string ScriptPerson(string idaccount)
    {
      return service.ScriptPerson(idaccount);
    }


  }
}