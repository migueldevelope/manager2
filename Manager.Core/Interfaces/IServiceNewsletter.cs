using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceNewsletter
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    string Delete(string id);
    string New(ViewCrudNewsletter view);
    string Update(ViewCrudNewsletter view);
    ViewCrudNewsletter Get(string id);
    List<ViewListNewsletter> List(ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListNewsletter> ListNewsletter(EnumPortal portal, ref long total, int count = 10, int page = 1, string filter = "");


    string DeleteNewsletterRead(string id);
    string NewNewsletterRead(ViewCrudNewsletterRead view);
    string UpdateNewsletterRead(ViewCrudNewsletterRead view);
    ViewCrudNewsletterRead GetNewsletterRead(string id);
    List<ViewListNewsletterRead> ListNewsletterRead(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
