using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceAutoManager
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    List<ViewAutoManagerPerson> List(string idManager, string filter);
    List<ViewAutoManagerPerson> ListOpen(string idManager);
    List<ViewAutoManagerPerson> ListEnd(string idManager, string filter);
    void SetManagerPerson(ViewManager view, string idPerson, string path);
    string SendMail(string link, Person person, string idmail);
    void Disapproved(ViewWorkflow view, string idPerson, string idManager);
    void Approved(ViewWorkflow view, string idPerson, string idManager);
    void Canceled(string idPerson, string idManager);
    List<ViewAutoManager> GetApproved(string idManager);
    void DeleteManager(string idPerson);
  }
}
