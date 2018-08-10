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
    MailModel OnBoardingApproval(string path);
    MailModel DefaultOnBoardingApproval(string path);
    MailModel OnBoardingPendingManager(string path);
    MailModel DefaultExpectationsPendingManager(string path);
    MailModel OnBoardingPendingEmployee(string path);
    MailModel DefaultExpectationsPendingEmployee(string path);

  }
}
