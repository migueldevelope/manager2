using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Core.Business;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Linq;
using Manager.Core.Base;

namespace Manager.Services.Specific
{
  public class ServiceMailMessage : Repository<MailMessage>, IServiceMailMessage
  {
    private readonly ServiceGeneric<MailMessage> mailMessageService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceMailMessage(DataContext context)
      : base(context)
    {
      try
      {
        mailMessageService = new ServiceGeneric<MailMessage>(context);
      }
      catch (ServiceException)
      {
        throw;
      }
    }

    public ViewMailMessage GetMessage(string id)
    {
      try
      {
        return (from message in mailMessageService.GetAuthentication(p => p._id == id)
                select message).ToList().
                Select(message => new ViewMailMessage
                {
                  Url = message.Url,
                  Body = message.Body,
                  Type = message.Type,
                  Token = message.Token,
                  Name = message.Name
                }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      mailMessageService._user = _user;
    }
  }
}
