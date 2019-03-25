using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceOnBoarding
  {

    string RemoveOnBoarding(string idperson);
    string ScriptComments();
    string DeleteComments(string idonboarding, string iditem, string idcomments);
    void SetUser(IHttpContextAccessor contextAccessor);

    List<OnBoarding> ListOnBoardingsWait(string idmanager, ref long total, string filter, int count, int page);
    List<OnBoarding> ListOnBoardingsEnd(string idmanager, ref long total, string filter, int count, int page);
    OnBoarding GetOnBoardings(string id);
    OnBoarding PersonOnBoardingsWait(string idmanager);
    List<OnBoarding> PersonOnBoardingsEnd(string idmanager, ref long total, string filter, int count, int page);
    OnBoarding NewOnBoarding(OnBoarding onboarding, string idperson);
    string UpdateOnBoarding(OnBoarding onboarding, string idperson);
    List<OnBoarding> GetListExclud(ref long total, string filter, int count, int page);
    List<ListComments> AddComments(string idonboarding, string iditem, ListComments comments);
    string UpdateComments(string idonboarding, string iditem, ListComments comments);
    List<ListComments> GetListComments(string idonboarding, string iditem);
    string UpdateCommentsView(string idonboarding, string iditem, EnumUserComment userComment);

    //#region Old
    //List<OnBoarding> ListOnBoardingsWaitOld(string idmanager, ref long total, string filter, int count, int page);
    //List<OnBoarding> ListOnBoardingsEndOld(string idmanager, ref long total, string filter, int count, int page);
    //OnBoarding GetOnBoardingsOld(string id);
    //OnBoarding PersonOnBoardingsWaitOld(string idmanager);
    //List<OnBoarding> PersonOnBoardingsEndOld(string idmanager, ref long total, string filter, int count, int page);
    //OnBoarding NewOnBoardingOld(OnBoarding onboarding, string idperson);
    //string UpdateOnBoardingOld(OnBoarding onboarding, string idperson);
    //List<OnBoarding> GetListExcludOld(ref long total, string filter, int count, int page);
    //List<ListComments> AddCommentsOld(string idonboarding, string iditem, ListComments comments);
    //string UpdateCommentsOld(string idonboarding, string iditem, ListComments comments);
    //List<ListComments> GetListCommentsOld(string idonboarding, string iditem);
    //string UpdateCommentsViewOld(string idonboarding, string iditem, EnumUserComment userComment);
    //#endregion


  }
}
