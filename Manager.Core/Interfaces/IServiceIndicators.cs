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
    List<ViewTagsCloud> ListTagsCloud(string idmanager, string idperson);
    List<ViewTagsCloud> ListTagsCloudCompany(string idmanager, string idperson);
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
    List<ViewTagsCloud> ListTagsCloudCompanyPeriod(ViewFilterManagerAndDate filters, string idmanager);
    List<ViewTagsCloud> ListTagsCloudPeriod(ViewFilterManagerAndDate filters, string idmanager);
    ViewListPlanQtd GetListPlanQtd(ViewFilterDate date, string idManager);
    ViewListPlanQtd GetListPlanQtdMap(ViewFilterDate date, string idManager);
    List<ViewAccountEnableds> GetAccountEnableds();
    List<ViewTagsCloudPerson> ListTagsCloudCompanyPeriodPerson(ViewFilterManagerAndDate filters, string idmanager, int count, int page, ref long total, string filter);
    List<ViewTagsCloudPerson> ListTagsCloudPeriodPerson(ViewFilterManagerAndDate filters, string idmanager, int count, int page, ref long total, string filter);
    ViewListMonitoringQtdManagerGeral GetMoninitoringQtdManager(ViewFilterDate date, string idManager, int count, int page, ref long total, string filter);
    List<ViewPersonsNotInfo> GetPersonsNotInfo(int count, int page, ref long total, string filter);
    IEnumerable<ViewChartCeritification> ChartCertification(ViewFilterDate date);
    IEnumerable<ViewChartCertificationCount> ChartCertificationCount(ViewFilterDate date);
    ViewListPlanQtdGerals GetPlanQtd(ViewFilterDate date, int count, int page, ref long total, string filter);
    List<ViewListSucessFactors1> ListSucessFactors1();
    List<ViewListSucessFactors2> ListSucessFactors2();
    List<ViewListSucessFactors3> ListSucessFactors3();
    IEnumerable<ViewChartRecommendation> ChartRecommendationPersons(ViewFilterIdAndDate filters, int count, int page, ref long total, string filter);
    IEnumerable<ViewChartOnboarding> ChartOnboardingMap(List<ViewListIdIndicators> persons);
    ViewListMonitoringQtdManagerGeral GetMoninitoringQtdManagerMap(ViewFilterDate date, string idManager, int count, int page, ref long total, string filter);
    ViewListPlanQtdGerals GetPlanQtdMap(ViewFilterDate date, int count, int page, ref long total, string filter);
    List<ViewListPending> OnboardingInDayMap(List<_ViewList> managers);
    List<ViewListPending> OnboardingToWinMap(List<_ViewList> managers);
    List<ViewListPending> OnboardingLateMap(List<_ViewList> managers);
    List<ViewListPending> CheckpointInDayMap(List<_ViewList> managers);
    List<ViewListPending> CheckpointToWinMap(List<_ViewList> managers);
    List<ViewListPending> CheckpointLateMap(List<_ViewList> managers);
    IEnumerable<ViewChartRecommendation> ChartRecommendationPersonsMap(ViewFilterManagerAndDate filters, int count, int page, ref long total, string filter);
    void UpdateStatusPlanMonitoring();
    List<ViewListScheduleManager> ListScheduleManager(ViewFilterDate date);
    ViewDashboard GetDashboard();
  }
}
