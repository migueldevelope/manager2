using Manager.Core.Base;
using Manager.Core.Views;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceIndicators
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    Task<List<ViewIndicatorsNotes>> GetNotes(string id);
    Task<List<ViewIndicatorsNotes>> GetNotesPerson(string id);
    Task<bool> VerifyAccount(string id);
    void SetUser(BaseUser baseUser);
    Task SendMessages(string link);
    Task<List<ViewTagsCloud>> ListTagsCloud(string idmanager);
    Task<List<ViewTagsCloud>> ListTagsCloudCompany(string idmanager);
    Task<List<ViewTagsCloud>> ListTagsCloudPerson(string idperson);
    Task<List<ViewTagsCloud>> ListTagsCloudCompanyPerson(string idperson);
    //string[] ExportStatusOnboarding(ref  long total,  string filter, int count, int page);
    Task<List<ViewExportStatusOnboardingGeral>> ExportStatusOnboarding();
    Task<List<ViewExportStatusMonitoringGeral>> ExportStatusMonitoring();
    Task<List<ViewExportStatusOnboarding>> ExportStatusOnboarding(string idperson);
    Task<List<ViewExportStatusMonitoring>> ExportStatusMonitoring(string idperson);
    Task<List<ViewExportStatusCheckpoint>> ExportStatusCheckpoint();
    Task<List<ViewExportStatusPlan>> ExportStatusPlan();
    Task<List<ViewExportStatusCertification>> ExportStatusCertification();
    Task<List<ViewExportStatusCertificationPerson>> ExportStatusCertification(string idperson);

    Task<IEnumerable<ViewChartOnboarding>> ChartOnboarding();

    Task<IEnumerable<ViewChartStatus>> ChartOnboardingRealized();

    Task<IEnumerable<ViewChartMonitoring>> ChartMonitoring();

    Task<IEnumerable<ViewChartCheckpoint>> ChartCheckpoint();

    Task<IEnumerable<ViewChartPlan>> ChartPlan();

    Task<IEnumerable<ViewChartStatus>> ChartMonitoringRealized();

    Task<IEnumerable<ViewChartStatus>> ChartCheckpointRealized();

    Task<IEnumerable<ViewChartStatus>> ChartPlanRealized();
  }
}
