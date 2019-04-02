﻿using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceMailModel
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);

    List<ViewListMailModel> List(ref long total, int count = 10, int page = 1, string filter = "");
    string New(ViewCrudMailModel view);
    ViewCrudMailModel Get(string id);
    string Update(ViewCrudMailModel view);

    #region Old
    //MailModel DefaultAutoManagerOld(string path);
    //MailModel DefaultExpectationsPendingManagerOld(string path);
    //MailModel DefaultExpectationsPendingEmployeeOld(string path);

    string NewOld(MailModel view);
    string UpdateOld(MailModel view);
    string RemoveOld(string id);
    MailModel GetOld(string id);
    List<MailModel> ListOld(ref long total, int count = 10, int page = 1, string filter = "");
    #endregion

  }
}
