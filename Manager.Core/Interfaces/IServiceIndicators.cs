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
    List<ViewIndicatorsNotes> GetNotes(string id);
    List<ViewIndicatorsNotes> GetNotesPerson(string id);
    bool VerifyAccount(string id);
    void SetUser(BaseUser baseUser);
    Task SendMessages(string link);
    List<ViewTagsCloud> ListTagsCloud(string idmanager);
    List<ViewTagsCloud> ListTagsCloudCompany(string idmanager);
    List<ViewTagsCloud> ListTagsCloudPerson(string idperson);
    List<ViewTagsCloud> ListTagsCloudCompanyPerson(string idperson);
    //string[] ExportStatusOnboarding(ref long total, string filter, int count, int page);
    List<ViewExportStatusOnboardingGeral> ExportStatusOnboarding();
    List<ViewExportStatusMonitoringGeral> ExportStatusMonitoring();
    List<ViewExportStatusOnboarding> ExportStatusOnboarding(string idperson);
    List<ViewExportStatusMonitoring> ExportStatusMonitoring(string idperson);
    List<ViewExportStatusCheckpoint> ExportStatusCheckpoint();
    List<ViewExportStatusPlan> ExportStatusPlan();
    List<ViewExportStatusCertification> ExportStatusCertification();
    List<ViewExportStatusCertificationPerson> ExportStatusCertification(string idperson);

    IEnumerable<ViewChartOnboarding> ChartOnboarding();

    IEnumerable<ViewChartStatus> ChartOnboardingRealized();

    IEnumerable<ViewChartMonitoring> ChartMonitoring();

    IEnumerable<ViewChartCheckpoint> ChartCheckpoint();

    IEnumerable<ViewChartPlan> ChartPlan();

    IEnumerable<ViewChartStatus> ChartMonitoringRealized();

    IEnumerable<ViewChartStatus> ChartCheckpointRealized();

    IEnumerable<ViewChartStatus> ChartPlanRealized();
  }
}
