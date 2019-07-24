using System.Collections.Generic;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// Controle para verficação de API ativa
  /// </summary>
  [Route("api/[controller]")]
  public class ValuesController : Controller
  {
    /// <summary>
    /// Método único para demonstração de API REST ativa
    /// </summary>
    /// <returns>String com a versão da API REST</returns>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new string[] { "version", "0.000000035" };
    }

    /// <summary>
    /// teste excel
    /// </summary>
    /// <returns></returns>
    [Route("excel")]
    [HttpGet]
    public string Excel()
    {
      var i = new ServiceExcel();
      return i.ImportSalaryScale();
    }
  }
}
