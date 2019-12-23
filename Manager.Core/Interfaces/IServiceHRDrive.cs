using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceHRDrive
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    string Delete(string id);
    string Add(AttachmentDrive view);
    ViewCrudHRDrive Get(string id);
    List<ViewListHRDrive> List(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
