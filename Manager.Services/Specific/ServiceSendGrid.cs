using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
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
    private ServiceGeneric<MailLog> serviceMail;

    #region Constructor
    public ServiceSendGrid(DataContext context) : base(context)
    {
      try
      {
        serviceMail = new ServiceGeneric<MailLog>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceMail._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      serviceMail._user = user;
    }
    #endregion

    #region Send Grid
    public async Task<string> Send(string idMail, string apiKeySendGrid)
    {
      try
      {
        return await Send(serviceMail.GetAll(p => p._id == idMail).FirstOrDefault(), apiKeySendGrid);
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
          serviceMail.Update(mailSend, null);
          throw new Exception(string.Format("e-mail send error: {0}", response.StatusCode));
        }

        mailSend.StatusMail = EnumStatusMail.Sended;
        mailSend.KeySendGrid = new List<string>();
        foreach (var item in response.Headers.GetValues("X-Message-Id"))
          mailSend.KeySendGrid.Add(item);

        serviceMail.Update(mailSend, null);
        return "e-mail send sucess!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

  }
}
