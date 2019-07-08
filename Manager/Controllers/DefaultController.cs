using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// controller padrão
  /// </summary>
  [Produces("application/json")]
  public class DefaultController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="contextAccessor"></param>
    public DefaultController(IHttpContextAccessor contextAccessor)
    {
      if (contextAccessor.HttpContext.Request.Path.Value.Length > 20)
      {
        if (contextAccessor.HttpContext.Request.Path.Value.Substring(0, 20) == "/user/forgotpassword")
        {
          return;
        }
      }

      string idperson = "";

      foreach (Claim ci in contextAccessor.HttpContext.User.Claims)
      {
        if (ci.Type == ClaimTypes.Actor)
          idperson = ci.Value;
      }

      if (idperson == "")
      {
        throw new HttpRequestException("token_invalid");
      }
    }
  }
}