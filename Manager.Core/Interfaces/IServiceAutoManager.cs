using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Views;
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
    Task<List<ViewAutoManagerPerson>> List(string idManager,  ref long total, int count = 10, int page = 1, string filter = "");
    Task<List<ViewAutoManagerPerson>> ListOpen(string idManager,  ref long total, int count = 10, int page = 1, string filter = "");
    Task<List<ViewAutoManagerPerson>> ListEnd(string idManager, string filter);
    Task SetManagerPerson(ViewManager view, string idPerson, string path);
    Task<string> SendMail(string link, Person person, string idmail);
    Task<string> Disapproved(ViewWorkflow view, string idPerson, string idManager);
    Task<string> Approved(ViewWorkflow view, string idPerson, string idManager);
    Task Canceled(string idPerson, string idManager);
    Task<List<ViewAutoManager>> ListApproved(string idManager);
    Task DeleteManager(string idPerson);
  }
}
