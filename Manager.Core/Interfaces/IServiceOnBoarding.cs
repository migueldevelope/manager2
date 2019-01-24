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
    List<OnBoarding> PersonOnBoardingsEnd(string idmanager, ref long total, string filter, int count, int page);
    OnBoarding NewOnBoarding(OnBoarding onboarding, string idperson);
    string UpdateOnBoarding(OnBoarding onboarding, string idperson);
    string RemoveOnBoarding(string idperson);
    List<OnBoarding> GetListExclud(ref long total, string filter, int count, int page);
    void SetUser(IHttpContextAccessor contextAccessor);
    string AddComments(string idonboarding, string iditem, List comments);
    string UpdateComments(string idonboarding, string iditem, List comments);
    string DeleteComments(string idonboarding, string iditem, string idcomments);
    List<List> GetListComments(string idonboarding, string iditem);
  }
}
