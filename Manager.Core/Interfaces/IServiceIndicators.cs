using Manager.Core.Base;
using Manager.Core.Views;
using Manager.Views.BusinessList;
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
    void SendMessages(string link);
    List<ViewTagsCloud> ListTagsCloud(string idmanager);
    List<ViewTagsCloud> ListTagsCloudCompany(string idmanager);
    List<ViewTagsCloud> ListTagsCloudPerson(string idperson);
    List<ViewTagsCloud> ListTagsCloudCompanyPerson(string idperson);
    //string[] ExportStatusOnboarding(ref  long total,  string filter, int count, int page);

    List<_ViewList> GetFilterPersons(string idmanager);
    IEnumerable<ViewChartOnboarding> ChartOnboarding(List<_ViewList> persons);

    IEnumerable<ViewChartStatus> ChartOnboardingRealized(List<_ViewList> persons);

    IEnumerable<ViewChartMonitoring> ChartMonitoring(List<_ViewList> persons);

    IEnumerable<ViewChartCheckpoint> ChartCheckpoint(List<_ViewList> persons);

    IEnumerable<ViewChartPlan> ChartPlan(List<_ViewList> persons);

    IEnumerable<ViewChartStatus> ChartMonitoringRealized(List<_ViewList> persons);

    IEnumerable<ViewChartStatus> ChartCheckpointRealized(List<_ViewList> persons);

    IEnumerable<ViewChartStatus> ChartPlanRealized(List<_ViewList> persons);
  }
}
