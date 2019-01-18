using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceSendGrid : Repository<MailLog>, IServiceSendGrid
  {
    private ServiceGeneric<MailLog> mailService;
    public ServiceSendGrid(DataContext context)
      : base(context)
    {
      try
      {
        this.mailService = new ServiceGeneric<MailLog>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public async Task<string> Send(string idMail, string apiKeySendGrid)
    {
      try
      {
        return await Send(mailService.GetAll(p => p._id == idMail).FirstOrDefault(), apiKeySendGrid);
      }
      catch (Exception)
      {
        throw;
      }
    }
    public async Task<string> Send(MailLog mailSend, string apiKeySendGrid)
    {
      try
      {
        var client = new SendGridClient(apiKeySendGrid);
        var msg = new SendGridMessage()
        {
          From = new EmailAddress(mailSend.From.Email, mailSend.From.Name),
          Subject = mailSend.Subject,
          PlainTextContent = "e-Mail formatt HTML",
          HtmlContent = mailSend.Body
        };

        
        foreach (var item in mailSend.To)
          msg.AddTo(new EmailAddress(item.Email, item.Name));

        if (mailSend.CopyTo != null)
        {
          foreach (var item in mailSend.CopyTo)
            msg.AddCc(new EmailAddress(item.Email, item.Name));
        }
        if (mailSend.CopyTo != null)
        {
          foreach (var item in mailSend.CopyBcc)
            msg.AddBcc(new EmailAddress(item.Email, item.Name));
        }
        if (mailSend.Priority == EnumPriorityMail.High)
          msg.Headers.Add("Priority", "Urgent");

        var response = await client.SendEmailAsync(msg);

        if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
        {
          mailSend.StatusMail = EnumStatusMail.Error;
          mailService.Update(mailSend, null);
          throw new ServiceException(_user, new Exception(string.Format("e-mail send error: {0}", response.StatusCode)), _context);
        }

        mailSend.StatusMail = EnumStatusMail.Sended;
        mailSend.KeySendGrid = new List<string>();
        foreach (var item in response.Headers.GetValues("X-Message-Id"))
          mailSend.KeySendGrid.Add(item);

        mailService.Update(mailSend, null);
        return "e-mail send sucess!";
      }
      catch (ServiceException)
      {
        throw;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      this.mailService._user = _user;
    }
  }
}
