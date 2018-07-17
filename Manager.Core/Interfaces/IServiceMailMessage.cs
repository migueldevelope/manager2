using Manager.Core.Base;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;

namespace Manager.Core.Interfaces
{
  public interface IServiceMailMessage
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    ViewMailMessage GetMessage(string id);
  }
}
