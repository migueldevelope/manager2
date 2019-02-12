﻿using Manager.Core.Business;
using Manager.Core.Enumns;
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
    List<ListComments> AddComments(string idonboarding, string iditem, ListComments comments);
    string UpdateComments(string idonboarding, string iditem, ListComments comments);
    string DeleteComments(string idonboarding, string iditem, string idcomments);
    List<ListComments> GetListComments(string idonboarding, string iditem);
    string UpdateCommentsView(string idonboarding, string iditem, EnumUserComment userComment);
    string ScriptComments();
  }
}
