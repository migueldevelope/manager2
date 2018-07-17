using Manager.Core.Base;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;

namespace Manager.Core.Interfaces
{
  public interface IServiceLog
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void NewLog(ViewLog view);
  }
}
