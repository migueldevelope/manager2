using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Manager.Services.Specific
{
  public class ServiceNotification : Repository<ConfigurationNotifications>, IServiceNotification
  {
    private readonly ServiceGeneric<ConfigurationNotifications> configurationNotificationsService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<Account> accountService;
    private readonly ServiceLog logService;
    private readonly ServiceMailModel mailModelService;
    private readonly ServiceGeneric<MailMessage> mailMessageService;
    private readonly ServiceGeneric<MailLog> mailService;
    private readonly string path;
    public BaseUser user { get => _user; set => user = _user; }

    public ServiceNotification(DataContext context, string _path)
      : base(context)
    {
      try
      {
        path = _path;
        configurationNotificationsService = new ServiceGeneric<ConfigurationNotifications>(context);
        personService = new ServiceGeneric<Person>(context);
        logService = new ServiceLog(_context);
        mailModelService = new ServiceMailModel(context);
        mailMessageService = new ServiceGeneric<MailMessage>(context);
        mailService = new ServiceGeneric<MailLog>(context);
        accountService = new ServiceGeneric<Account>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SendMessage()
    {
      try
      {
        var accounts = accountService.GetAuthentication(p => p.Status == EnumStatus.Disabled).ToList();
        foreach (var item in accounts)
        {
          BaseUser baseUser = new BaseUser();
          baseUser._idAccount = item._idAccount;
          SendMessageAccount(baseUser);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async void SendMessageAccount(BaseUser baseUser)
    {
      try
      {
        SetUser(baseUser);
        OnboardingSeq1();
        OnboardingSeq2();
        OnboardingSeq3();
        OnboardingSeq4();
        OnboardingSeq5();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetUser(BaseUser baseUser)
    {
      _user = baseUser;
      personService._user = _user;
      configurationNotificationsService._user = _user;
      logService._user = _user;
      mailModelService._user = _user;
      mailMessageService._user = _user;
      mailService._user = _user;
    }

    public async void OnboardingSeq1()
    {
      try
      {
        var now = DateTime.Now.AddDays(1).AddTicks(-1);
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.OnBoarding & p.DateAdm == now).ToList();
        foreach(var item in persons)
        {
          MailOnboardingSeq1(item);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async void OnboardingSeq2()
    {
      try
      {
        var now = DateTime.Now.AddDays(26).AddTicks(-1);
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.OnBoarding & p.DateAdm == now).ToList();
        foreach (var item in persons)
        {
          MailOnboardingSeq2(item);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async void OnboardingSeq3()
    {
      try
      {
        var now = DateTime.Now.AddDays(31).AddTicks(-1);
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.OnBoarding & p.DateAdm == now).ToList();
        foreach (var item in persons)
        {
          MailOnboardingSeq3(item);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async void OnboardingSeq4()
    {
      try
      {
        var now = DateTime.Now.AddDays(36).AddTicks(-1);
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.OnBoarding & p.DateAdm == now).ToList();
        foreach (var item in persons)
        {
          MailOnboardingSeq4(item);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async void OnboardingSeq5()
    {
      try
      {
        var now = DateTime.Now.AddDays(41).AddTicks(-1);
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.OnBoarding & p.DateAdm == now).ToList();
        foreach (var item in persons)
        {
          MailOnboardingSeq5(item);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async void MailOnboardingSeq1(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.OnBoardingSeq1(path);
        var url = "";
        var body = model.Message.Replace("{Person}", person.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name);
        var message = new MailMessage
        {
          Type = EnumTypeMailMessage.Put,
          Name = model.Name,
          Url = url,
          Body = body
        };
        var idMessage = mailMessageService.Insert(message)._id;
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Analisa.Solutions"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.Name,
          Body = body,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = model.Subject
        };
        var mailObj = mailService.Insert(sendMail);
        var token = SendMail(path, person, mailObj._id.ToString());
        var messageEnd = mailMessageService.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        mailMessageService.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public async void MailOnboardingSeq2(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.OnBoardingSeq1(path);
        var url = "";
        var body = model.Message.Replace("{Person}", person.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name);
        var message = new MailMessage
        {
          Type = EnumTypeMailMessage.Put,
          Name = model.Name,
          Url = url,
          Body = body
        };
        var idMessage = mailMessageService.Insert(message)._id;
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Analisa.Solutions"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.Name,
          Body = body,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = model.Subject
        };
        var mailObj = mailService.Insert(sendMail);
        var token = SendMail(path, person, mailObj._id.ToString());
        var messageEnd = mailMessageService.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        mailMessageService.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public async void MailOnboardingSeq3(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.OnBoardingSeq1(path);
        var url = "";
        var body = model.Message.Replace("{Person}", person.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name);
        var message = new MailMessage
        {
          Type = EnumTypeMailMessage.Put,
          Name = model.Name,
          Url = url,
          Body = body
        };
        var idMessage = mailMessageService.Insert(message)._id;
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Analisa.Solutions"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.Name,
          Body = body,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = model.Subject
        };
        var mailObj = mailService.Insert(sendMail);
        var token = SendMail(path, person, mailObj._id.ToString());
        var messageEnd = mailMessageService.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        mailMessageService.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public async void MailOnboardingSeq4(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.OnBoardingSeq1(path);
        var url = "";
        var body = model.Message.Replace("{Person}", person.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name);
        var message = new MailMessage
        {
          Type = EnumTypeMailMessage.Put,
          Name = model.Name,
          Url = url,
          Body = body
        };
        var idMessage = mailMessageService.Insert(message)._id;
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Analisa.Solutions"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.Name,
          Body = body,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = model.Subject
        };
        var mailObj = mailService.Insert(sendMail);
        var token = SendMail(path, person, mailObj._id.ToString());
        var messageEnd = mailMessageService.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        mailMessageService.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public async void MailOnboardingSeq5(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.OnBoardingSeq1(path);
        var url = "";
        var body = model.Message.Replace("{Person}", person.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name);
        var message = new MailMessage
        {
          Type = EnumTypeMailMessage.Put,
          Name = model.Name,
          Url = url,
          Body = body
        };
        var idMessage = mailMessageService.Insert(message)._id;
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Analisa.Solutions"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.Name,
          Body = body,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = model.Subject
        };
        var mailObj = mailService.Insert(sendMail);
        var token = SendMail(path, person, mailObj._id.ToString());
        var messageEnd = mailMessageService.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        mailMessageService.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string SendMail(string link, Person person, string idmail)
    {
      try
      {
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link);
          var data = new
          {
            mail = person.Mail,
            password = person.Password
          };
          var json = JsonConvert.SerializeObject(data);
          var content = new StringContent(json);
          content.Headers.ContentType.MediaType = "application/json";
          client.DefaultRequestHeaders.Add("ContentType", "application/json");
          var result = client.PostAsync("manager/authentication/encrypt", content).Result;
          var resultContent = result.Content.ReadAsStringAsync().Result;
          var auth = JsonConvert.DeserializeObject<ViewPerson>(resultContent);
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth.Token);
          var resultMail = client.PostAsync("mail/sendmail/" + idmail, null).Result;
          return auth.Token;
        }
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
  }
}
