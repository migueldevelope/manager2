﻿using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceOnBoarding
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    List<ViewListOnBoarding> List(string idmanager, ref long total, string filter, int count, int page);
    List<ViewListOnBoarding> List_V2(List<ViewListIdIndicators> persons, ref long total, string filter, int count, int page);
    ViewListOnBoarding PersonWait(string idperson);
    ViewListOnBoarding New(string idperson, string plataform);
    ViewCrudOnboarding Get(string id);
    string Delete(string idperson, string plataform);
    string DeleteComments(string idonboarding, string iditem, string idcomments, string plataform);
    string UpdateCommentsView(string idonboarding, string iditem, EnumUserComment userComment);
    List<ViewListOnBoarding> ListEnded(string idmanager, string plataform, ref long total, string filter, int count, int page);
    ViewCrudOnboarding GetOnBoardings(string id);
    List<ViewListOnBoarding> ListPersonEnd(string idmanager, string plataform, ref long total, string filter, int count, int page);
    string Update(ViewCrudOnboarding onboarding, string plataform);
    List<ViewListOnBoarding> ListExcluded(ref long total, string filter, int count, int page);
    List<ViewCrudComment> AddComments(string idonboarding, string iditem, ViewCrudComment comments, string plataform);
    List<ViewCrudComment> UpdateComments(string idonboarding, string iditem, ViewCrudComment comments);
    List<ViewCrudComment> ListComments(string idonboarding, string iditem);
    List<ViewListOnBoarding> ListOnBoardingsWait(string idmanager, ref long total, string filter, int count, int page);
    List<ViewExportStatusOnboardingGeral> ExportStatusOnboarding(List<ViewListIdIndicators> persons);
    List<ViewExportStatusOnboarding> ExportStatusOnboarding(string idperson);
    List<ViewExportOnboardingComments> ExportOnboardingComments(List<ViewListIdIndicators> persons);
    void MailTest();
    List<OnBoarding> Load();
    string AddCommentsSpeech(string idonboarding, string iditem, string link, EnumUserComment user, string totalimte, string plataform);
    
    #region Mobile
    ViewListOnBoardingMobile GetMobile(string id);
    string UpdateCommentsEndMobile(string idonboarding, EnumUserComment userComment, ViewCrudCommentEnd comments);
    string UpdateStatusOnBoarding(string idonboarding, EnumStatusOnBoarding status, string plataform);
    void UpdateCommentsSpeech(string idonboarding, string iditem, EnumUserComment user, string path, string link);
    void RegisterOnMessageHandlerAndReceiveMesssages();
    #endregion
  }
}
