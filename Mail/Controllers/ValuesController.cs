using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Mail.Controllers
{
  /// <summary>
  /// Controle para verficação de API ativa
  /// </summary>
  [Route("mail/[controller]")]
  public class ValuesController : Controller
  {
    /// <summary>
    /// Método único para demonstração de API REST ativa
    /// </summary>
    /// <returns>String com a versão da API REST</returns>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new string[] { "version", "0.000000000000011" };                  
    }
  }
}
