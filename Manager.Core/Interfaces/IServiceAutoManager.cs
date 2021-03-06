﻿using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Views;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceAutoManager
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    List<ViewAutoManagerPerson> List(string idManager, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewAutoManagerPerson> ListOpen(string idManager, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewAutoManagerPerson> ListEnd(string idManager, string filter);
    void SetManagerPerson(ViewManager view, string idPerson);
    string Disapproved(ViewWorkflow view, string idPerson, string idManager);
    string Approved(ViewWorkflow view, string idPerson, string idManager);
    void Canceled(string idPerson, string idManager);
    List<ViewAutoManager> ListApproved(string idManager);
    void DeleteManager(string idPerson);
  }
}
