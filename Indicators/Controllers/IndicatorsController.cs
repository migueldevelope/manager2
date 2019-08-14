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
    public async Task<List<ViewListIdIndicators>> GetFilterPersons(string idmanager = "")
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
    public async Task<IEnumerable<ViewChartOnboarding>> ChartOnboarding([FromBody] List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.ChartOnboarding(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("chartrecommendation")]
    public async Task<IEnumerable<ViewChartRecommendation>> ChartRecommendation([FromBody] List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.ChartRecommendation(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("chartcertificationstatus")]
    public async Task<IEnumerable<ViewChartCeritificationStatus>> ChartCertificationStatus([FromBody] List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.ChartCertificationStatus(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("chartmonitoring")]
    public async Task<IEnumerable<ViewChartMonitoring>> ChartMonitoring([FromBody] List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.ChartMonitoring(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("chartcheckpoint")]
    public async Task<IEnumerable<ViewChartCheckpoint>> ChartCheckpoint([FromBody] List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.ChartCheckpoint(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("chartplan")]
    public async Task<IEnumerable<ViewChartPlan>> ChartPlan([FromBody] List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.ChartPlan(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("chartmonitoringrealized")]
    public async Task<IEnumerable<ViewChartStatus>> ChartMonitoringRealized([FromBody] List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.ChartMonitoringRealized(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("chartcheckpointrealized")]
    public async Task<IEnumerable<ViewChartStatus>> ChartCheckpointRealized([FromBody] List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.ChartCheckpointRealized(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("chartplanrealized")]
    public async Task<IEnumerable<ViewChartStatus>> ChartPlanRealized([FromBody] List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.ChartPlanRealized(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("chartonboardingrealized")]
    public async Task<IEnumerable<ViewChartStatus>> ChartOnboardingRealized([FromBody] List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.ChartOnboardingRealized(persons));
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