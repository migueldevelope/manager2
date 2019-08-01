using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Indicators.Controllers
{
  /// <summary>
  /// 
  /// </summary>
  [Produces("application/json")]
  [Route("")]
  public class IndicatorsController : DefaultController
  {
    private readonly IServiceIndicators service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_service"></param>
    /// <param name="contextAccessor"></param>
    public IndicatorsController(IServiceIndicators _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getnotes/{idperson}")]
    public async Task<List<ViewIndicatorsNotes>> GetNotes(string idperson)
    {
      return await Task.Run(() => service.GetNotes(idperson));
    }

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
      return await Task.Run(() => service.GetNotesPerson(idperson));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmanager"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listtagscloud/{idmanager}")]
    public async Task<List<ViewTagsCloud>> ListTagsCloud(string idmanager)
    {
      return await Task.Run(() => service.ListTagsCloud(idmanager));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listtagscloudperson/{idperson}")]
    public async Task<List<ViewTagsCloud>> ListTagsCloudPerson(string idperson)
    {
      return await Task.Run(() => service.ListTagsCloudPerson(idperson));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmanager"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getfilterpersons")]
    public async Task<List<_ViewList>> GetFilterPersons(string idmanager = "")
    {
      return await Task.Run(() => service.GetFilterPersons(idmanager));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("chartonboarding")]
    public async Task<IEnumerable<ViewChartOnboarding>> ChartOnboarding([FromBody] List<_ViewList> persons)
    {
      return await Task.Run(() => service.ChartOnboarding(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("chartmonitoring")]
    public async Task<IEnumerable<ViewChartMonitoring>> ChartMonitoring()
    {
      return await Task.Run(() => service.ChartMonitoring());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("chartcheckpoint")]
    public async Task<IEnumerable<ViewChartCheckpoint>> ChartCheckpoint()
    {
      return await Task.Run(() => service.ChartCheckpoint());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("chartplan")]
    public async Task<IEnumerable<ViewChartPlan>> ChartPlan()
    {
      return await Task.Run(() => service.ChartPlan());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("chartmonitoringrealized")]
    public async Task<IEnumerable<ViewChartStatus>> ChartMonitoringRealized()
    {
      return await Task.Run(() => service.ChartMonitoringRealized());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("chartcheckpointrealized")]
    public async Task<IEnumerable<ViewChartStatus>> ChartCheckpointRealized()
    {
      return await Task.Run(() => service.ChartCheckpointRealized());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("chartplanrealized")]
    public async Task<IEnumerable<ViewChartStatus>> ChartPlanRealized()
    {
      return await Task.Run(() => service.ChartPlanRealized());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("chartonboardingrealized")]
    public async Task<IEnumerable<ViewChartStatus>> ChartOnboardingRealized()
    {
      return await Task.Run(() => service.ChartOnboardingRealized());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmanager"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listtagscloudcompany/{idmanager}")]
    public async Task<List<ViewTagsCloud>> ListTagsCloudCompany(string idmanager)
    {
      return await Task.Run(() => service.ListTagsCloudCompany(idmanager));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listtagscloudcompanyperson/{idperson}")]
    public async Task<List<ViewTagsCloud>> ListTagsCloudCompanyPerson(string idperson)
    {
      return await Task.Run(() => service.ListTagsCloudCompanyPerson(idperson));
    }

  }
}