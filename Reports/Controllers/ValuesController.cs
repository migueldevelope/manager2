using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Reports.Controllers
{
  /// <summary>
  /// 
  /// </summary>
  [Route("api/[controller]")]
  public class ValuesController : Controller
  {
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new string[] { "Version", "0.000000001" };
    }

  }
}
