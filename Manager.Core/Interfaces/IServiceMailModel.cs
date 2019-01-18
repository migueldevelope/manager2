using Manager.Core.Base;
using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceMailModel
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
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

    string New(MailModel view);
    string Update(MailModel view);
    string Remove(string id);
    MailModel Get(string id);
    List<MailModel> List(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
