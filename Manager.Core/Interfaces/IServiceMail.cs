using Manager.Core.Base;
using Microsoft.AspNetCore.Http;

namespace Manager.Core.Interfaces
{
  public interface IServiceMail
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    string SendMail(string link, string mail, string password, string idmail);
  }
}
