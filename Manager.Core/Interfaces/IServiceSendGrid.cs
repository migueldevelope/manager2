using Manager.Core.Base;
using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceSendGrid
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task<string> Send(string idMail, string apiKeySendGrid);
    Task<string> Send(MailLog mailSend, string apiKeySendGrid);
  }
}
