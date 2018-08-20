using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceOnBoarding
  {
    List<OnBoarding> ListOnBoardingsWait(string idmanager, ref long total, string filter, int count, int page);
    List<OnBoarding> ListOnBoardingsEnd(string idmanager, ref long total, string filter, int count, int page);
    OnBoarding GetOnBoardings(string id);
    OnBoarding PersonOnBoardingsWait(string idmanager);
    OnBoarding PersonOnBoardingsEnd(string idmanager);
    OnBoarding NewOnBoarding(OnBoarding onboarding, string idperson);
    string UpdateOnBoarding(OnBoarding onboarding, string idperson);
    string RemoveOnBoarding(string idperson);
    List<OnBoarding> GetListExclud(string idperson);
    void SetUser(IHttpContextAccessor contextAccessor);
  }
}
