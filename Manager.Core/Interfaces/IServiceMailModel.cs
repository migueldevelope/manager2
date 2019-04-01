using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceMailModel
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);

    List<ViewListMailModel> List(ref long total, int count = 10, int page = 1, string filter = "");
    string New(ViewCrudMailModel view);
    ViewCrudMailModel Get(string id);
    string Update(ViewCrudMailModel view);

    MailModel AutoManager(string path);
    MailModel Certification(string path);
    MailModel CertificationApproval(string path);
    MailModel CertificationDisapproval(string path);

    MailModel CheckpointApproval(string path);
    MailModel CheckpointResult(string path);
    MailModel CheckpointResultDisapproved(string path);
    MailModel CheckpointResultPerson(string path);
    MailModel CheckpointSeq1(string path);
    MailModel CheckpointSeq2(string path);
    MailModel MonitoringApproval(string path);
    MailModel MonitoringApprovalManager(string path);
    MailModel MonitoringDisApproval(string path);
    MailModel MonitoringSeq1(string path);
    MailModel MonitoringSeq1Person(string path);
    MailModel OnBoardingApproval(string path);
    MailModel OnBoardingApprovalManager(string path);
    MailModel OnBoardingApprovalOccupation(string path);
    MailModel OnBoardingApprovalManagerOccupation(string path);
    MailModel OnBoardingDisapproval(string path);
    MailModel OnBoardingPendingManager(string path);
    MailModel OnBoardingSeq1(string path);
    MailModel OnBoardingSeq2(string path);
    MailModel OnBoardingSeq3(string path);
    MailModel OnBoardingSeq4(string path);
    MailModel OnBoardingSeq5(string path);
    MailModel PlanApproval(string path);
    MailModel Plan(string path);
    MailModel PlanSeq1(string path);
    MailModel PlanSeq1Person(string path);
    MailModel PlanSeq2(string path);
    MailModel PlanSeq2Person(string path);
    MailModel PlanSeq3(string path);
    MailModel PlanSeq3Person(string path);

    #region Old
    //MailModel DefaultAutoManagerOld(string path);
    MailModel DefaultExpectationsPendingManagerOld(string path);
    MailModel DefaultExpectationsPendingEmployeeOld(string path);

    string NewOld(MailModel view);
    string UpdateOld(MailModel view);
    string RemoveOld(string id);
    MailModel GetOld(string id);
    List<MailModel> ListOld(ref long total, int count = 10, int page = 1, string filter = "");
    #endregion

  }
}
