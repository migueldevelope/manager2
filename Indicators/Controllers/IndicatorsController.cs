using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tools;
using Tools.Data;

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
    private readonly IServiceManager serviceManager;
    private readonly IHttpContextAccessor _contextAccessor;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_service"></param>
    /// <param name="_serviceManager"></param>
    /// <param name="contextAccessor"></param>
    public IndicatorsController(IServiceIndicators _service, IServiceManager _serviceManager,
    //public IndicatorsController(IServiceIndicators _service, 
      IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      _contextAccessor = contextAccessor;
      service = _service;
      serviceManager = _serviceManager;
      service.SetUser(contextAccessor);
      serviceManager.SetUser(contextAccessor);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmanager"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("gethierarchy/{idmanager}")]
    public async Task<List<ViewListStructManager>> GetHierarchy(string idmanager)
    {
      return await Task.Run(() => serviceManager.GetHierarchy(idmanager));
      //return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("exportlogs")]
    public async Task<List<ViewExportLogs>> ExportLogs()
    {
      return await Task.Run(() => service.ExportLogs());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("accessaccount")]
    public async Task<ViewAccessAccount> AccessAccount()
    {
      return await Task.Run(() => service.AccessAccount());
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getdashboard")]
    public async Task<ViewDashboard> GetDashboard()
    {
      return await Task.Run(() => service.GetDashboard());
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
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listtagscloud/{idmanager}")]
    public async Task<List<ViewTagsCloud>> ListTagsCloud(string idmanager, string idperson = "")
    {
      return await Task.Run(() => service.ListTagsCloud(idmanager, idperson));
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
    [Route("map/chartonboarding")]
    public async Task<IEnumerable<ViewChartOnboarding>> ChartOnboardingMap([FromBody] List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.ChartOnboardingMap(persons));
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
    /// <param name="filters"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("chartrecommendationpersons")]
    public async Task<IEnumerable<ViewChartRecommendation>> ChartRecommendationPersons([FromBody] ViewFilterIdAndDate filters, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ChartRecommendationPersons(filters, count, page, ref total, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getoffBoarding")]
    public async Task<IEnumerable<ViewGetOffBoarding>> GetOffBoarding(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetOffBoarding(count, page, ref total, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("map/chartrecommendationpersons")]
    public async Task<IEnumerable<ViewChartRecommendation>> ChartRecommendationPersonsMap([FromBody] ViewFilterManagerAndDate filters, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ChartRecommendationPersonsMap(filters, count, page, ref total, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
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
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listtagscloudcompany/{idmanager}")]
    public async Task<List<ViewTagsCloud>> ListTagsCloudCompany(string idmanager, string idperson = "")
    {
      return await Task.Run(() => service.ListTagsCloudCompany(idmanager, idperson));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmanager"></param>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listtagscloudfull")]
    public async Task<List<ViewTagsCloudFull>> ListTagsCloudFull(string idmanager = "", string idperson = "")
    {
      return await Task.Run(() => service.ListTagsCloudFull(idmanager, idperson));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmanager"></param>
    /// <param name="idperson"></param>
    /// <param name="filters"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("listtagscloudfullrh")]
    public async Task<List<ViewTagsCloudFull>> ListTagsCloudFullRH([FromBody] ViewFilterManagerAndDate filters, string idmanager = "", string idperson = "")
    {
      return await Task.Run(() => service.ListTagsCloudFullRH(filters, idmanager, idperson));
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
    /// <param name="managers"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("map/onboardinginday")]
    public async Task<List<ViewListPending>> OnboardingInDayMap([FromBody]List<_ViewList> managers)
    {
      return await Task.Run(() => service.OnboardingInDayMap(managers));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="managers"></param>
    /// <param name="contextAccessor"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("map/onboardinginday/test")]
    public async Task<List<ViewListPending>> OnboardingInDayMapTest([FromBody]List<_ViewList> managers)
    {
      Config conn = XmlConnection.ReadVariablesSystem();
      var context = new DataContext(conn.Server, conn.DataBase);

      var serviceMaturity = new ServiceMaturity(context);
      serviceMaturity.SetUser(_contextAccessor);
      var serviceQue = new ServiceControlQueue(conn.ServiceBusConnectionString, serviceMaturity);
      var servicePerson = new ServicePerson(context, context, serviceQue, conn.SignalRService);
      servicePerson.SetUser(_contextAccessor);
      var serviceTest = new ServiceIndicators(context, context, conn.TokenServer, servicePerson);
      serviceTest.SetUser(_contextAccessor);
      return await Task.Run(() => serviceTest.OnboardingInDayMap(managers));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="managers"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("map/onboardingtowin")]
    public async Task<List<ViewListPending>> OnboardingToWinMap([FromBody]List<_ViewList> managers)
    {
      return await Task.Run(() => service.OnboardingToWinMap(managers));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="managers"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("map/onboardinglate")]
    public async Task<List<ViewListPending>> OnboardingLateMap([FromBody]List<_ViewList> managers)
    {
      return await Task.Run(() => service.OnboardingLateMap(managers));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="managers"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("map/checkpointinday")]
    public async Task<List<ViewListPending>> CheckpointInDayMap([FromBody]List<_ViewList> managers)
    {
      return await Task.Run(() => service.CheckpointInDayMap(managers));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="managers"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("map/checkpointtowin")]
    public async Task<List<ViewListPending>> CheckpointToWinMap([FromBody]List<_ViewList> managers)
    {
      return await Task.Run(() => service.CheckpointToWinMap(managers));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="managers"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("map/checkpointlate")]
    public async Task<List<ViewListPending>> CheckpointLateMap([FromBody]List<_ViewList> managers)
    {
      return await Task.Run(() => service.CheckpointLateMap(managers));
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
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("getmoninitoringqtdmanager")]
    public async Task<ViewListMonitoringQtdManagerGeral> GetMoninitoringQtdManager([FromBody]ViewFilterDate date, string idmanager = "", int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = await Task.Run(() => service.GetMoninitoringQtdManager(date, idmanager, count, page, ref total, filter));
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
    [Route("map/getmoninitoringqtdmanager")]
    public async Task<ViewListMonitoringQtdManagerGeral> GetMoninitoringQtdManagerMap([FromBody]ViewFilterDate date, string idmanager = "", int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = await Task.Run(() => service.GetMoninitoringQtdManagerMap(date, idmanager, count, page, ref total, filter));
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="idmanager"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("listtagscloudcompanyperiod")]
    public async Task<List<ViewTagsCloud>> ListTagsCloudCompanyPeriod([FromBody]ViewFilterManagerAndDate filters, string idmanager = "")
    {
      return await Task.Run(() => service.ListTagsCloudCompanyPeriod(filters, idmanager));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="idmanager"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("listtagscloudperiod")]
    public async Task<List<ViewTagsCloud>> ListTagsCloudPeriod([FromBody]ViewFilterManagerAndDate filters, string idmanager = "")
    {
      return await Task.Run(() => service.ListTagsCloudPeriod(filters, idmanager));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="idmanager"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("listtagscloudcompanyperiodperson")]
    public async Task<List<ViewTagsCloudPerson>> ListTagsCloudCompanyPeriodPerson([FromBody]ViewFilterManagerAndDate filters, string idmanager = "", int count = 9999999, int page = 1, string filter = "")
    {
      long total = 0;
      var result = await Task.Run(() => service.ListTagsCloudCompanyPeriodPerson(filters, idmanager, count, page, ref total, filter));
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="idmanager"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("listtagscloudperiodperson")]
    public async Task<List<ViewTagsCloudPerson>> ListTagsCloudPeriodPerson([FromBody]ViewFilterManagerAndDate filters, string idmanager = "", int count = 9999999, int page = 1, string filter = "")
    {
      long total = 0;
      var result = await Task.Run(() => service.ListTagsCloudPeriodPerson(filters, idmanager, count, page, ref total, filter));
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
    /// <param name="date"></param>
    /// <param name="idmanager"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("map/getlistplanqtd")]
    public async Task<ViewListPlanQtd> GetListPlanQtdMap([FromBody]ViewFilterDate date, string idmanager = "")
    {
      return await Task.Run(() => service.GetListPlanQtdMap(date, idmanager));
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("chartcertification")]
    public async Task<IEnumerable<ViewChartCeritification>> ChartCertification([FromBody] ViewFilterDate date)
    {
      return await Task.Run(() => service.ChartCertification(date));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("chartcertificationcount")]
    public async Task<IEnumerable<ViewChartCertificationCount>> ChartCertificationCount([FromBody] ViewFilterDate date)
    {
      return await Task.Run(() => service.ChartCertificationCount(date));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("getplanqtd")]
    public async Task<ViewListPlanQtdGerals> GetPlanQtd([FromBody]ViewFilterDate date, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = await Task.Run(() => service.GetPlanQtd(date, count, page, ref total, filter));
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("map/getplanqtd")]
    public async Task<ViewListPlanQtdGerals> GetPlanQtdMap([FromBody]ViewFilterDate date, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = await Task.Run(() => service.GetPlanQtdMap(date, count, page, ref total, filter));
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listsucessfactors1")]
    public async Task<List<ViewListSucessFactors1>> ListSucessFactors1()
    {
      return await Task.Run(() => service.ListSucessFactors1());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listsucessfactors2")]
    public async Task<List<ViewListSucessFactors2>> ListSucessFactors2()
    {
      return await Task.Run(() => service.ListSucessFactors2());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listsucessfactors3")]
    public async Task<List<ViewListSucessFactors3>> ListSucessFactors3()
    {
      return await Task.Run(() => service.ListSucessFactors3());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("listschedulemanager")]
    public async Task<List<ViewListScheduleManager>> ListScheduleManager([FromBody]ViewFilterDate date)
    {
      return await Task.Run(() => service.ListScheduleManager(date));
    }

  }
}