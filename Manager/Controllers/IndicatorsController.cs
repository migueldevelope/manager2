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
    public class IndicatorsController : Controller
    {
    private readonly IServiceIndicators service;

    public IndicatorsController(IServiceIndicators _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [Authorize]
    [HttpGet]
    [Route("getnotes/{idperson}")]
    public List<ViewIndicatorsNotes> GetNotes(string idperson)
    {
      return service.GetNotes(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("getnotesperson/{idperson}")]
    public List<ViewIndicatorsNotes> GetNotesPerson(string idperson)
    {
      return service.GetNotesPerson(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("listtagscloud/{idmanager}")]
    public List<ViewTagsCloud> ListTagsCloud(string idmanager)
    {
      return service.ListTagsCloud(idmanager);
    }

    [Authorize]
    [HttpGet]
    [Route("listtagscloudperson/{idperson}")]
    public List<ViewTagsCloud> ListTagsCloudPerson(string idperson)
    {
      return service.ListTagsCloudPerson(idperson);
    }

    //[Authorize]
    //[HttpGet]
    //[Route("exportonboarding")]
    //public string[] ExportStatusOnboarding(int count = 999999, int page = 1, string filter = "")
    //{
    //  long total = 0;
    //  var result = service.ExportStatusOnboarding(ref total, filter, count, page);
    //  Response.Headers.Add("x-total-count", total.ToString());
    //  return result;
    //}

    [Authorize]
    [HttpGet]
    [Route("exportonboarding")]
    public List<ViewExportStatusOnboardingGeral> ExportStatusOnboarding()
    {
     return service.ExportStatusOnboarding();
    }

    [Authorize]
    [HttpGet]
    [Route("exportstatuscertification")]
    public List<ViewExportStatusCertification> ExportStatusCertification()
    {
      return service.ExportStatusCertification();
    }

    [Authorize]
    [HttpGet]
    [Route("exportstatuscertification/{idperson}")]
    public List<ViewExportStatusCertificationPerson> ExportStatusCertification(string idperson)
    {
      return service.ExportStatusCertification(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("exportmonitoring")]
    public List<ViewExportStatusMonitoringGeral> ExportStatusMonitoring()
    {
      return service.ExportStatusMonitoring();
    }

    [HttpGet]
    [Route("exportonboarding/{idperson}")]
    public List<ViewExportStatusOnboarding> ExportStatusOnboarding(string idperson)
    {
      return service.ExportStatusOnboarding(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("exportmonitoring/{idperson}")]
    public List<ViewExportStatusMonitoring> ExportStatusMonitoring(string idperson)
    {
      return service.ExportStatusMonitoring(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("exportcheckpoint")]
    public List<ViewExportStatusCheckpoint> ExportStatusCheckpoint()
    {
      return service.ExportStatusCheckpoint();
    }

    [Authorize]
    [HttpGet]
    [Route("exportplan")]
    public List<ViewExportStatusPlan> ExportStatusPlan()
    {
      return service.ExportStatusPlan();
    }

    [Authorize]
    [HttpGet]
    [Route("chartonboarding")]
    public IEnumerable<ViewChartOnboarding> ChartOnboarding()
    {
      return service.ChartOnboarding();
    }

    [Authorize]
    [HttpGet]
    [Route("chartmonitoring")]
    public IEnumerable<ViewChartMonitoring> ChartMonitoring()
    {
      return service.ChartMonitoring();
    }

    [Authorize]
    [HttpGet]
    [Route("chartcheckpoint")]
    public IEnumerable<ViewChartCheckpoint> ChartCheckpoint()
    {
      return service.ChartCheckpoint();
    }

    [Authorize]
    [HttpGet]
    [Route("chartplan")]
    public IEnumerable<ViewChartPlan> ChartPlan()
    {
      return service.ChartPlan();
    }

    [Authorize]
    [HttpGet]
    [Route("chartmonitoringrealized")]
    public IEnumerable<ViewChartStatus> ChartMonitoringRealized()
    {
      return service.ChartMonitoringRealized();
    }

    [Authorize]
    [HttpGet]
    [Route("chartcheckpointrealized")]
    public IEnumerable<ViewChartStatus> ChartCheckpointRealized()
    {
      return service.ChartCheckpointRealized();
    }

    [Authorize]
    [HttpGet]
    [Route("chartplanrealized")]
    public IEnumerable<ViewChartStatus> ChartPlanRealized()
    {
      return service.ChartPlanRealized();
    }

    [Authorize]
    [HttpGet]
    [Route("chartonboardingrealized")]
    public IEnumerable<ViewChartStatus> ChartOnboardingRealized()
    {
      return service.ChartOnboardingRealized();
    }

    [Authorize]
    [HttpGet]
    [Route("listtagscloudcompany/{idmanager}")]
    public List<ViewTagsCloud> ListTagsCloudCompany(string idmanager)
    {
      return service.ListTagsCloudCompany(idmanager);
    }


    [Authorize]
    [HttpGet]
    [Route("listtagscloudcompanyperson/{idperson}")]
    public List<ViewTagsCloud> ListTagsCloudCompanyPerson(string idperson)
    {
      return service.ListTagsCloudCompanyPerson(idperson);
    }

  }
}