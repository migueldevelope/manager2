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

    IEnumerable<ViewChartOnboarding> ChartOnboarding(List<ViewListIdIndicators> persons);

    IEnumerable<ViewChartStatus> ChartOnboardingRealized(List<ViewListIdIndicators> persons);

    IEnumerable<ViewChartMonitoring> ChartMonitoring(List<ViewListIdIndicators> persons);

    IEnumerable<ViewChartCheckpoint> ChartCheckpoint(List<ViewListIdIndicators> persons);

    IEnumerable<ViewChartPlan> ChartPlan(List<ViewListIdIndicators> persons);

    IEnumerable<ViewChartStatus> ChartMonitoringRealized(List<ViewListIdIndicators> persons);

    IEnumerable<ViewChartStatus> ChartCheckpointRealized(List<ViewListIdIndicators> persons);

    IEnumerable<ViewChartStatus> ChartPlanRealized(List<ViewListIdIndicators> persons);
    IEnumerable<ViewChartRecommendation> ChartRecommendation(List<ViewListIdIndicators> persons);
    IEnumerable<ViewChartCeritificationStatus> ChartCertificationStatus(List<ViewListIdIndicators> persons);

    List<ViewListPending> OnboardingInDay(List<ViewListIdIndicators> persons);
    List<ViewListPending> OnboardingToWin(List<ViewListIdIndicators> persons);
    List<ViewListPending> OnboardingLate(List<ViewListIdIndicators> persons);
    List<ViewListPending> CheckpointInDay(List<ViewListIdIndicators> persons);
    List<ViewListPending> CheckpointToWin(List<ViewListIdIndicators> persons);
    List<ViewListPending> CheckpointLate(List<ViewListIdIndicators> persons);
    ViewMoninitoringQtd GetMoninitoringQtd(ViewFilterDate date, string idManager);
    List<ViewTagsCloud> ListTagsCloudCompanyPeriod(ViewFilterDate date, string idmanager);
    List<ViewTagsCloud> ListTagsCloudPeriod(ViewFilterDate date, string idmanager);
    ViewListPlanQtd GetListPlanQtd(ViewFilterDate date, string idManager);
    List<ViewAccountEnableds> GetAccountEnableds();
    List<ViewTagsCloudPerson> ListTagsCloudCompanyPeriodPerson(ViewFilterDate date, string idmanager, int count, int page, ref long total, string filter);
    List<ViewTagsCloudPerson> ListTagsCloudPeriodPerson(ViewFilterDate date, string idmanager, int count, int page, ref long total, string filter);
    List<ViewMoninitoringQtdManager> GetMoninitoringQtdManager(ViewFilterDate date, string idManager);
    List<ViewPersonsNotInfo> GetPersonsNotInfo(int count, int page, ref long total, string filter);
  }
}
