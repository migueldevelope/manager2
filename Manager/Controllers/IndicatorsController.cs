using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("indicators")]
  public class IndicatorsController : DefaultController
  {
    private readonly IServiceIndicators service;

    public IndicatorsController(IServiceIndicators _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [Authorize]
    [HttpGet]
    [Route("getnotes/{idperson}")]
    public async Task<List<ViewIndicatorsNotes>> GetNotes(string idperson)
    {
      return await service.GetNotes(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("getnotesperson/{idperson}")]
    public async Task<List<ViewIndicatorsNotes>> GetNotesPerson(string idperson)
    {
      return await service.GetNotesPerson(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("listtagscloud/{idmanager}")]
    public async Task<List<ViewTagsCloud>> ListTagsCloud(string idmanager)
    {
      return await service.ListTagsCloud(idmanager);
    }

    [Authorize]
    [HttpGet]
    [Route("listtagscloudperson/{idperson}")]
    public async Task<List<ViewTagsCloud>> ListTagsCloudPerson(string idperson)
    {
      return await service.ListTagsCloudPerson(idperson);
    }

    //[Authorize]
    //[HttpGet]
    //[Route("exportonboarding")]
    //public async Task<  string[] ExportStatusOnboarding(int count = 999999, int page = 1, string filter = "")
    //{
    //  long total = 0;
    //  var result = service.ExportStatusOnboarding(filter, count, page);
    //  Response.Headers.Add("x-total-count", total.ToString());
    //  return await  result;
    //}

    [Authorize]
    [HttpGet]
    [Route("exportonboarding")]
    public async Task<List<ViewExportStatusOnboardingGeral>> ExportStatusOnboarding()
    {
      return await service.ExportStatusOnboarding();
    }

    [Authorize]
    [HttpGet]
    [Route("exportstatuscertification")]
    public async Task<List<ViewExportStatusCertification>> ExportStatusCertification()
    {
      return await service.ExportStatusCertification();
    }

    [Authorize]
    [HttpGet]
    [Route("exportstatuscertification/{idperson}")]
    public async Task<List<ViewExportStatusCertificationPerson>> ExportStatusCertification(string idperson)
    {
      return await service.ExportStatusCertification(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("exportmonitoring")]
    public async Task<List<ViewExportStatusMonitoringGeral>> ExportStatusMonitoring()
    {
      return await service.ExportStatusMonitoring();
    }

    [HttpGet]
    [Route("exportonboarding/{idperson}")]
    public async Task<List<ViewExportStatusOnboarding>> ExportStatusOnboarding(string idperson)
    {
      return await service.ExportStatusOnboarding(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("exportmonitoring/{idperson}")]
    public async Task<List<ViewExportStatusMonitoring>> ExportStatusMonitoring(string idperson)
    {
      return await service.ExportStatusMonitoring(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("exportcheckpoint")]
    public async Task<List<ViewExportStatusCheckpoint>> ExportStatusCheckpoint()
    {
      return await service.ExportStatusCheckpoint();
    }

    [Authorize]
    [HttpGet]
    [Route("exportplan")]
    public async Task<List<ViewExportStatusPlan>> ExportStatusPlan()
    {
      return await service.ExportStatusPlan();
    }

    [Authorize]
    [HttpGet]
    [Route("chartonboarding")]
    public async Task<IEnumerable<ViewChartOnboarding>> ChartOnboarding()
    {
      return await service.ChartOnboarding();
    }

    [Authorize]
    [HttpGet]
    [Route("chartmonitoring")]
    public async Task<IEnumerable<ViewChartMonitoring>> ChartMonitoring()
    {
      return await service.ChartMonitoring();
    }

    [Authorize]
    [HttpGet]
    [Route("chartcheckpoint")]
    public async Task<IEnumerable<ViewChartCheckpoint>> ChartCheckpoint()
    {
      return await service.ChartCheckpoint();
    }

    [Authorize]
    [HttpGet]
    [Route("chartplan")]
    public async Task<IEnumerable<ViewChartPlan>> ChartPlan()
    {
      return await service.ChartPlan();
    }

    [Authorize]
    [HttpGet]
    [Route("chartmonitoringrealized")]
    public async Task<IEnumerable<ViewChartStatus>> ChartMonitoringRealized()
    {
      return await service.ChartMonitoringRealized();
    }

    [Authorize]
    [HttpGet]
    [Route("chartcheckpointrealized")]
    public async Task<IEnumerable<ViewChartStatus>> ChartCheckpointRealized()
    {
      return await service.ChartCheckpointRealized();
    }

    [Authorize]
    [HttpGet]
    [Route("chartplanrealized")]
    public async Task<IEnumerable<ViewChartStatus>> ChartPlanRealized()
    {
      return await service.ChartPlanRealized();
    }

    [Authorize]
    [HttpGet]
    [Route("chartonboardingrealized")]
    public async Task<IEnumerable<ViewChartStatus>> ChartOnboardingRealized()
    {
      return await service.ChartOnboardingRealized();
    }

    [Authorize]
    [HttpGet]
    [Route("listtagscloudcompany/{idmanager}")]
    public async Task<List<ViewTagsCloud>> ListTagsCloudCompany(string idmanager)
    {
      return await service.ListTagsCloudCompany(idmanager);
    }


    [Authorize]
    [HttpGet]
    [Route("listtagscloudcompanyperson/{idperson}")]
    public async Task<List<ViewTagsCloud>> ListTagsCloudCompanyPerson(string idperson)
    {
      return await service.ListTagsCloudCompanyPerson(idperson);
    }

  }
}