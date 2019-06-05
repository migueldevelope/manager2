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
  [Produces("application/json")]
  public class DefaultController : Controller
  {
    public DefaultController(IHttpContextAccessor contextAccessor)
    {
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