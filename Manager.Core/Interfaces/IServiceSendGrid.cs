using Manager.Core.Base;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceSendGrid
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task<string> Send(string idMail, string apiKeySendGrid);
  }
}
