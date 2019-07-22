using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceBaseHelp
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    void SetAttachment(string idBaseHelp, string url, string fileName, string attachmentid);
    string Delete(string id);
    string New(ViewCrudBaseHelp view);
    string Update(ViewCrudBaseHelp view);
    ViewCrudBaseHelp Get(string id);
    void Count(string id);
    ViewCrudBaseHelp GetByText(string text);
    List<ViewListBaseHelp> List(ref long total, int count = 10, int page = 1, string filter = "");
    void RegisterOnMessageHandlerAndReceiveMesssages();
  }
}
