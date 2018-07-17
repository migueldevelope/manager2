using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceSendGrid
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    Task<string> Send(string idMail, string apiKeySendGrid);
    Task<string> Send(MailLog mailSend, string apiKeySendGrid);
  }
}
