using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceOnBoarding
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    List<ViewListOnBoarding> ListOnBoarding(string idmanager, ref long total, string filter, int count, int page);
    ViewListOnBoarding PersonOnBoardingWait(string idperson);
    ViewListOnBoarding NewOnBoarding(string idperson);

    #region Old
    string RemoveOnBoarding(string idperson);
    string DeleteComments(string idonboarding, string iditem, string idcomments);
    List<OnBoarding> ListOnBoardingsWaitOld(string idmanager, ref long total, string filter, int count, int page);
    List<OnBoarding> ListOnBoardingsEnd(string idmanager, ref long total, string filter, int count, int page);
    OnBoarding GetOnBoardings(string id);
    OnBoarding PersonOnBoardingsWaitOld(string idmanager);
    List<OnBoarding> PersonOnBoardingsEnd(string idmanager, ref long total, string filter, int count, int page);
    OnBoarding NewOnBoardingOld(OnBoarding onboarding, string idperson);
    string UpdateOnBoarding(OnBoarding onboarding, string idperson);
    List<OnBoarding> GetListExclud(ref long total, string filter, int count, int page);
    List<ListComments> AddComments(string idonboarding, string iditem, ListComments comments);
    string UpdateComments(string idonboarding, string iditem, ListComments comments);
    List<ListComments> GetListComments(string idonboarding, string iditem);
    string UpdateCommentsView(string idonboarding, string iditem, EnumUserComment userComment);
    #endregion

  }
}
