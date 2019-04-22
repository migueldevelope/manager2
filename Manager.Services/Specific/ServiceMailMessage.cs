using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Core.Business;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using System;
using Manager.Core.Base;

namespace Manager.Services.Specific
{
  public class ServiceMailMessage : Repository<MailMessage>, IServiceMailMessage
  {
    private readonly ServiceGeneric<MailMessage> serviceMailMessage;

    #region Constructor
    public ServiceMailMessage(DataContext contextLog) : base(contextLog)
    {
      try
      {
        serviceMailMessage = new ServiceGeneric<MailMessage>(contextLog);
      }
      catch (ServiceException)
      {
        throw;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceMailMessage._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceMailMessage._user = user;
    }
    #endregion

    #region MailMessage
    public ViewMailMessage GetMessage(string id)
    {
      try
      {
        MailMessage message = serviceMailMessage.GetNewVersion(p => p._id == id).Result;
        return message == null ? null :
          new ViewMailMessage
          {
            Url = message.Url,
            Body = message.Body,
            Type = message.Type,
            Token = message.Token,
            Name = message.Name
          };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

  }
}
