using Manager.Core.Base;
using Manager.Core.Business;
using Microsoft.AspNetCore.Http;

namespace Manager.Core.Interfaces
{
  public interface IServiceMailModel
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    MailModel AutoManager(string path);
    MailModel DefaultAutoManager(string path);
    MailModel DevelopmentPlan(string path);
    MailModel DefaultDevelopmentPlan(string path);
    MailModel AgreementExpectationsApproval(string path);
    MailModel DefaultAgreementExpectationsApproval(string path);
    MailModel AgreementExpectationsPendingManager(string path);
    MailModel DefaultExpectationsPendingManager(string path);
    MailModel AgreementExpectationsPendingEmployee(string path);
    MailModel DefaultExpectationsPendingEmployee(string path);

  }
}
