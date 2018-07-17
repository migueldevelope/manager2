using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("account")]
  public class AccountController : Controller
  {
    private readonly IServiceAccount service;

    public AccountController(IServiceAccount _service)
    {
      service = _service;
    }

    [HttpPost]
    [Route("new")]
    public ObjectResult Post([FromBody]ViewNewAccount view)
    {
      service.NewAccount(view);
      return Ok("Account sucess!");
    }
  }
}