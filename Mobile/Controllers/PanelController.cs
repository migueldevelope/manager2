using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Mobile.Controllers
{
  /// <summary>
  /// Controlador do Onboarding
  /// </summary>
  [Produces("application/json")]
  [Route("panel")]
  public class PanelController : DefaultController
  {
    private readonly IServiceOnBoarding serviceOnborading;
    private readonly IServiceIndicators serviceIndicators;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_serviceOnborading">Serviço de Onboarding</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public PanelController(IServiceOnBoarding _serviceOnborading, IServiceIndicators _serviceIndicators,IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      serviceOnborading = _serviceOnborading;
      serviceIndicators = _serviceIndicators;
      serviceOnborading.SetUser(contextAccessor);
      serviceIndicators.SetUser(contextAccessor);
    }
    #endregion

    #region Panel

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getnotesperson/{idperson}")]
    public async Task<List<ViewIndicatorsNotes>> GetNotesPerson(string idperson)
    {
      return await Task.Run(() => serviceIndicators.GetNotesPerson(idperson));
    }

    #endregion
  }
}