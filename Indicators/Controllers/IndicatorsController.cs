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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("onboardinginday")]
    public async Task<List<ViewListPending>> OnboardingInDay([FromBody]List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.OnboardingInDay(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("onboardingtowin")]
    public async Task<List<ViewListPending>> OnboardingToWin([FromBody]List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.OnboardingToWin(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("onboardinglate")]
    public async Task<List<ViewListPending>> OnboardingLate([FromBody]List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.OnboardingLate(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("checkpointinday")]
    public async Task<List<ViewListPending>> CheckpointInDay([FromBody]List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.CheckpointInDay(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("checkpointtowin")]
    public async Task<List<ViewListPending>> CheckpointToWin([FromBody]List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.CheckpointToWin(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("checkpointlate")]
    public async Task<List<ViewListPending>> CheckpointLate([FromBody]List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.CheckpointLate(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="idmanager"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("getmoninitoringqtd")]
    public async Task<ViewMoninitoringQtd> GetMoninitoringQtd([FromBody]ViewFilterDate date, string idmanager = "")
    {
      return await Task.Run(() => service.GetMoninitoringQtd(date, idmanager));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="idmanager"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("getmoninitoringqtdmanager")]
    public async Task<List<ViewMoninitoringQtdManager>> GetMoninitoringQtdManager([FromBody]ViewFilterDate date, string idmanager = "")
    {
      return await Task.Run(() => service.GetMoninitoringQtdManager(date, idmanager));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="idmanager"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("listtagscloudcompanyperiod")]
    public async Task<List<ViewTagsCloud>> ListTagsCloudCompanyPeriod([FromBody]ViewFilterDate date, string idmanager = "")
    {
      return await Task.Run(() => service.ListTagsCloudCompanyPeriod(date, idmanager));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="idmanager"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("listtagscloudperiod")]
    public async Task<List<ViewTagsCloud>> ListTagsCloudPeriod([FromBody]ViewFilterDate date, string idmanager = "")
    {
      return await Task.Run(() => service.ListTagsCloudPeriod(date, idmanager));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="idmanager"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("listtagscloudcompanyperiodperson")]
    public async Task<List<ViewTagsCloudPerson>> ListTagsCloudCompanyPeriodPerson([FromBody]ViewFilterDate date, string idmanager = "", int count = 10, int page= 1, string filter = "")
    {
      long total = 0;
      var result = await Task.Run(() => service.ListTagsCloudCompanyPeriodPerson(date, idmanager,count, page, ref total, filter));
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="idmanager"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("listtagscloudperiodperson")]
    public async Task<List<ViewTagsCloudPerson>> ListTagsCloudPeriodPerson([FromBody]ViewFilterDate date, string idmanager = "", int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = await Task.Run(() => service.ListTagsCloudPeriodPerson(date, idmanager, count, page, ref total, filter));
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="idmanager"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("getlistplanqtd")]
    public async Task<ViewListPlanQtd> GetListPlanQtd([FromBody]ViewFilterDate date, string idmanager = "")
    {
      return await Task.Run(() => service.GetListPlanQtd(date, idmanager));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getaccountenableds")]
    public async Task<List<ViewAccountEnableds>> GetAccountEnableds()
    {
      return await Task.Run(() => service.GetAccountEnableds());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="idmanager"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getpersonsnotinfo")]
    public async Task<List<ViewPersonsNotInfo>> GetPersonsNotInfo(string idmanager = "", int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = await Task.Run(() => service.GetPersonsNotInfo(count, page, ref total, filter));
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

  }
}