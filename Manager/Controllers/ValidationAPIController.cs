using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  
  /// <summary>
  /// 
  /// </summary>
  [Route("validationapi")]
  public class ValidationAPIController : Controller
  {
    private IServiceAuthentication serviceAuthentication;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_serviceAuthentication"></param>
    public ValidationAPIController(IServiceAuthentication _serviceAuthentication)
    {
      serviceAuthentication = _serviceAuthentication;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    /// application/x-www-form-urlencoded
    [Produces("application/x-www-form-urlencoded")]
    [HttpPost]
    [Route("test")]
    public string Test([FromBody]dynamic view)
    {
      return view;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    /// application/x-www-form-urlencoded
    [HttpPost]
    [Route("maristas")]
    public string Maristas([FromBody]ViewAuthentication view)
    {
      serviceAuthentication.GetMaristasAsyncTest(view.Mail, view.Password);
      return "ok";
    }

  }
}