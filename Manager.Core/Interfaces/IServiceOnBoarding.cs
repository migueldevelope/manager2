using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceOnBoarding
  {
    #region OnBoardgin
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    List<ViewListOnBoarding> ListOnBoarding(string idmanager, ref long total, string filter, int count, int page);
    ViewListOnBoarding PersonOnBoardingWait(string idperson);
    ViewListOnBoarding NewOnBoarding(string idperson);
    ViewCrudOnboarding GetOnBoarding(string id);
    string RemoveOnBoarding(string idperson);
    string DeleteComments(string idonboarding, string iditem, string idcomments);
    string UpdateCommentsView(string idonboarding, string iditem, EnumUserComment userComment);
    List<ViewListOnBoarding> ListOnBoardingsEnd(string idmanager, ref long total, string filter, int count, int page);
    ViewCrudOnboarding GetOnBoardings(string id);
    List<ViewListOnBoarding> PersonOnBoardingsEnd(string idmanager, ref long total, string filter, int count, int page);
    string UpdateOnBoarding(ViewCrudOnboarding onboarding, string idperson);
    List<ViewListOnBoarding> GetListExclud(ref long total, string filter, int count, int page);
    List<ViewCrudComment> AddComments(string idonboarding, string iditem, ViewCrudComment comments);
    string UpdateComments(string idonboarding, string iditem, ViewCrudComment comments);
    List<ViewCrudComment> GetListComments(string idonboarding, string iditem);
    #endregion

    #region Old
    List<OnBoarding> ListOnBoardingsWaitOld(string idmanager, ref long total, string filter, int count, int page);
    OnBoarding PersonOnBoardingsWaitOld(string idmanager);
    OnBoarding NewOnBoardingOld(OnBoarding onboarding, string idperson);
    List<OnBoarding> ListOnBoardingsEndOld(string idmanager, ref long total, string filter, int count, int page);
    OnBoarding GetOnBoardingsOld(string id);
    List<OnBoarding> PersonOnBoardingsEndOld(string idmanager, ref long total, string filter, int count, int page);
    string UpdateOnBoardingOld(OnBoarding onboarding, string idperson);
    List<OnBoarding> GetListExcludOld(ref long total, string filter, int count, int page);
    List<ListComments> AddCommentsOld(string idonboarding, string iditem, ListComments comments);
    string UpdateCommentsOld(string idonboarding, string iditem, ListComments comments);
    List<ListComments> GetListCommentsOld(string idonboarding, string iditem);
    #endregion

  }
}
