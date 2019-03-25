using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Timers;

namespace Manager.Services.Specific
{
  public class ServiceNotification : Repository<ConfigurationNotifications>, IServiceNotification
  {
    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceGeneric<ConfigurationNotifications> configurationNotificationsService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<OnBoarding> onBoardingService;
    private readonly ServiceGeneric<Account> accountService;
    private readonly ServiceLog logService;
    private readonly ServiceMailModel mailModelService;
    private readonly ServiceGeneric<MailMessage> mailMessageService;
    private readonly ServiceGeneric<MailLog> mailService;
    private readonly ServiceGeneric<Checkpoint> checkpointService;
    private readonly ServiceGeneric<Monitoring> monitoringService;
    private readonly ServiceGeneric<Parameter> parameterService;
    private readonly ServiceLogMessages logMessagesService;


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
        onBoardingService = new ServiceGeneric<OnBoarding>(context);
        checkpointService = new ServiceGeneric<Checkpoint>(context);
        monitoringService = new ServiceGeneric<Monitoring>(context);
        parameterService = new ServiceGeneric<Parameter>(context);
        logMessagesService = new ServiceLogMessages(context);
        serviceAuthentication = new ServiceAuthentication(context);
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
        Send();
        var timer = new Timer
        {
          //24 hours em milliseconds
          Interval = 86400000
        };
        timer.Elapsed += new ElapsedEventHandler(Timer1_Tick);
        timer.Enabled = true;
        timer.Start();
      }
      catch (Exception)
      {

      }
    }

    private void Send()
    {
      try
      {
        var log = new ViewLog
        {
          Description = "Service Notification",
          _idPerson = null,
          Local = "ManagerMessages"
        };

        logService.NewLog(log);
        var accounts = accountService.GetAuthentication(p => p.Status == EnumStatus.Enabled).ToList();

        foreach (var item in accounts)
        {
          var parameter = parameterService.GetAuthentication(p => p.Status == EnumStatus.Enabled & p._idAccount == item._id & p.Name == "servicemailmessage").FirstOrDefault();
          if (parameter != null)
          {
            if (parameter.Content == "true")
            {
              BaseUser baseUser = new BaseUser
              {
                _idAccount = item._idAccount
              };
              SendMessageAccount(baseUser);
            }
          }

        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    private void Timer1_Tick(object Sender, EventArgs e)
    {
      try
      {
        Send();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    private async void SendMessageAccount(BaseUser baseUser)
    {
      try
      {

        SetUser(baseUser);
        //logMessagesService.NewLogMessage("Teste", "Gestor e Colaborador realizaram o Onboarding de ", new Person() { _id = ObjectId.GenerateNewId().ToString(), Name = "Teste" });
        OnboardingSeq1();
        OnboardingSeq2();
        OnboardingSeq3();
        OnboardingSeq4();
        OnboardingSeq5();
        OnboardingSeq6();
        CheckpointSeq1();
        CheckpointSeq2();
        CheckpointSeq3();
        MonitoringSeq1();
        MonitoringSeq2();
        PlanSeq1();
      }
      catch (Exception)
      {
        //
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
      onBoardingService._user = _user;
      checkpointService._user = _user;
      monitoringService._user = _user;
      logMessagesService.SetUser(_user);
      mailModelService.SetUser(baseUser);
    }

    public async void OnboardingSeq1()
    {
      try
      {
        var nowLast = DateTime.Now.AddDays(-1).Date;
        var nowNext = DateTime.Now.AddDays(1).Date;
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.OnBoarding & p.User.DateAdm > nowLast & p.User.DateAdm < nowNext).ToList();
        foreach (var item in persons)
        {
          if (item.Manager != null)
          {
            logMessagesService.NewLogMessage("Novo Colaborador", "Colaborador " + item.User.Name, item);
            if (onBoardingService.GetAll(p => p.Person._id == item._id & p.StatusOnBoarding == EnumStatusOnBoarding.End).Count() == 0)
              MailOnboardingSeq1(item);
          }
        }
      }
      catch (Exception)
      {

      }
    }

    public async void OnboardingSeq2()
    {
      try
      {
        var nowLast = DateTime.Now.AddDays(-26).Date;
        var nowNext = DateTime.Now.AddDays(-24).Date;
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.OnBoarding & p.User.DateAdm > nowLast & p.User.DateAdm < nowNext).ToList();
        foreach (var item in persons)
        {
          if (item.Manager != null)
          {
            if (onBoardingService.GetAll(p => p.Person._id == item._id & p.StatusOnBoarding == EnumStatusOnBoarding.End).Count() == 0)
              MailOnboardingSeq2(item);
          }
        }
      }
      catch (Exception)
      {

      }
    }

    public async void OnboardingSeq3()
    {
      try
      {
        var nowLast = DateTime.Now.AddDays(-31).Date;
        var nowNext = DateTime.Now.AddDays(-29).Date;
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.OnBoarding & p.User.DateAdm > nowLast & p.User.DateAdm < nowNext).ToList();
        foreach (var item in persons)
        {
          if (item.Manager != null)
          {
            if (onBoardingService.GetAll(p => p.Person._id == item._id & p.StatusOnBoarding == EnumStatusOnBoarding.End).Count() == 0)
              MailOnboardingSeq3(item);
          }
        }
      }
      catch (Exception)
      {

      }
    }

    public async void OnboardingSeq4()
    {
      try
      {
        var nowLast = DateTime.Now.AddDays(-36).Date;
        var nowNext = DateTime.Now.AddDays(-34).Date;
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.OnBoarding & p.User.DateAdm > nowLast & p.User.DateAdm < nowNext).ToList();
        foreach (var item in persons)
        {
          if (item.Manager != null)
          {
            if (onBoardingService.GetAll(p => p.Person._id == item._id & p.StatusOnBoarding == EnumStatusOnBoarding.End).Count() == 0)
              MailOnboardingSeq4(item);
          }
        }
      }
      catch (Exception)
      {

      }
    }

    public async void OnboardingSeq5()
    {
      try
      {
        var nowLast = DateTime.Now.AddDays(-41).Date;
        var nowNext = DateTime.Now.AddDays(-39).Date;
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.OnBoarding & p.User.DateAdm > nowLast & p.User.DateAdm < nowNext).ToList();
        foreach (var item in persons)
        {
          if (item.Manager != null)
          {
            if (onBoardingService.GetAll(p => p.Person._id == item._id & p.StatusOnBoarding == EnumStatusOnBoarding.End).Count() == 0)
              MailOnboardingSeq5(item);
          }
        }
      }
      catch (Exception)
      {

      }
    }

    public async void OnboardingSeq6()
    {
      try
      {
        var nowLast = DateTime.Now.AddDays(-71).Date;
        var nowNext = DateTime.Now.AddDays(-69).Date;
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.OnBoarding & p.User.DateAdm > nowLast & p.User.DateAdm < nowNext).ToList();
        foreach (var item in persons)
        {
          if (item.Manager != null)
          {
            if (onBoardingService.GetAll(p => p.Person._id == item._id & p.StatusOnBoarding == EnumStatusOnBoarding.End).Count() == 0)
            {
              item.TypeJourney = EnumTypeJourney.Checkpoint;
              personService.Update(item, null);
            }
          }

        }
      }
      catch (Exception)
      {

      }
    }

    public async void CheckpointSeq1()
    {
      try
      {
        var nowLast = DateTime.Now.AddDays(-56).Date;
        var nowNext = DateTime.Now.AddDays(-54).Date;
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.Checkpoint & p.User.DateAdm > nowLast & p.User.DateAdm < nowNext).ToList();
        foreach (var item in persons)
        {
          if (item.Manager != null)
          {
            if (checkpointService.GetAll(p => p.Person._id == item._id & p.StatusCheckpoint == EnumStatusCheckpoint.End).Count() == 0)
              MailCheckpointSeq1(item);
          }
        }
      }
      catch (Exception)
      {

      }
    }

    public async void MonitoringSeq1()
    {
      try
      {
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.Monitoring).ToList();
        foreach (var item in persons)
        {
          if (item.Manager != null)
          {
            var wait = monitoringService.GetAll(p => p.Person._id == item._id
            & (p.StatusMonitoring != EnumStatusMonitoring.End
            & p.StatusMonitoring != EnumStatusMonitoring.Disapproved)).Count();
            if (wait == 0)
            {
              var maxDate = monitoringService.GetAll(p => p.Person._id == item._id).Max(p => p.DateEndEnd);
              if (maxDate == null)
              {
                maxDate = checkpointService.GetAll(p => p.Person._id == item._id).Max(p => p.DateEnd);
                if (maxDate == null)
                  return;
              }

              var totaldays = (DateTime.Parse(DateTime.Now.Date.ToString()) - DateTime.Parse(maxDate.ToString())).TotalDays;
              if (Math.Round(totaldays) == 60)
              {
                MailMonitoringSeq1(item, Math.Round(totaldays));
                MailMonitoringSeq1_Person(item, Math.Round(totaldays));
              }

            }
          }
        }
      }
      catch (Exception)
      {

      }
    }

    public async void PlanSeq1()
    {
      try
      {
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.Monitoring).ToList();
        foreach (var item in persons)
        {
          if (item.Manager != null)
          {
            var list = monitoringService.GetAll(p => p.Person._id == item._id
            & p.StatusMonitoring == EnumStatusMonitoring.End).ToList();

            foreach (var moni in list)
            {
              foreach (var row in moni.Activities)
              {
                foreach (var plan in row.Plans)
                {
                  var days = Math.Truncate((DateTime.Parse(plan.Deadline.ToString()) - DateTime.Now).TotalDays);
                  var daysExp = Math.Truncate((DateTime.Now - DateTime.Parse(plan.Deadline.ToString())).TotalDays);
                  if ((days <= 10) & (days >= 1))
                  {
                    MailPlanSeq1(item, days);
                    MailPlanSeq1_Person(item, days);
                  }
                  else if (days == 0)
                  {
                    MailPlanSeq2(item);
                    MailPlanSeq2_Person(item);
                  }
                  else if ((daysExp % 5) == 0)
                  {
                    MailPlanSeq3(item, daysExp);
                    MailPlanSeq3_Person(item, daysExp);
                  }
                }

              }

              foreach (var row in moni.SkillsCompany)
              {
                foreach (var plan in row.Plans)
                {
                  var days = Math.Truncate((DateTime.Parse(plan.Deadline.ToString()) - DateTime.Now).TotalDays);
                  var daysExp = Math.Truncate((DateTime.Now - DateTime.Parse(plan.Deadline.ToString())).TotalDays);
                  if ((days <= 10) & (days >= 1))
                  {
                    MailPlanSeq1(item, days);
                    MailPlanSeq1_Person(item, days);
                  }
                  else if (days == 0)
                  {
                    MailPlanSeq2(item);
                    MailPlanSeq2_Person(item);
                  }
                  else if ((daysExp % 5) == 0)
                  {
                    MailPlanSeq3(item, daysExp);
                    MailPlanSeq3_Person(item, daysExp);
                  }
                }

              }

              foreach (var row in moni.Schoolings)
              {
                foreach (var plan in row.Plans)
                {
                  var days = Math.Truncate((DateTime.Parse(plan.Deadline.ToString()) - DateTime.Now).TotalDays);
                  var daysExp = Math.Truncate((DateTime.Now - DateTime.Parse(plan.Deadline.ToString())).TotalDays);
                  if ((days <= 10) & (days >= 1))
                  {
                    MailPlanSeq1(item, days);
                    MailPlanSeq1_Person(item, days);
                  }
                  else if (days == 0)
                  {
                    MailPlanSeq2(item);
                    MailPlanSeq2_Person(item);
                  }
                  else if ((daysExp % 5) == 0)
                  {
                    MailPlanSeq3(item, daysExp);
                    MailPlanSeq3_Person(item, daysExp);
                  }
                }

              }
            }

          }
        }
      }
      catch (Exception)
      {

      }
    }

    public async void MonitoringSeq2()
    {
      try
      {
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.Monitoring).ToList();
        foreach (var item in persons)
        {
          if (item.Manager != null)
          {
            var wait = monitoringService.GetAll(p => p.Person._id == item._id
            & p.StatusMonitoring != EnumStatusMonitoring.End
            & p.StatusMonitoring != EnumStatusMonitoring.Disapproved).Count();
            if (wait == 0)
            {
              var maxDate = monitoringService.GetAll(p => p.Person._id == item._id).Max(p => p.DateEndEnd);
              if (maxDate == null)
              {
                maxDate = checkpointService.GetAll(p => p.Person._id == item._id).Max(p => p.DateEnd);
                if (maxDate == null)
                  return;
              }


              var totaldays = (DateTime.Parse(DateTime.Now.Date.ToString()) - DateTime.Parse(maxDate.ToString())).TotalDays;
              if (Math.Truncate(totaldays) > 69)
              {
                if ((Math.Truncate(totaldays) % 10) == 0)
                {
                  MailMonitoringSeq1(item, Math.Truncate(totaldays));
                  MailMonitoringSeq1_Person(item, Math.Truncate(totaldays));
                }

              }


            }
          }
        }
      }
      catch (Exception)
      {

      }
    }

    public async void CheckpointSeq2()
    {
      try
      {
        var nowLast = DateTime.Now.AddDays(-84).Date;
        var nowNext = DateTime.Now.AddDays(-73).Date;
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.Checkpoint & p.User.DateAdm > nowLast & p.User.DateAdm < nowNext).ToList();
        foreach (var item in persons)
        {
          if (item.Manager != null)
          {
            if (checkpointService.GetAll(p => p.Person._id == item._id & p.StatusCheckpoint == EnumStatusCheckpoint.End).Count() == 0)
            {
              var days = Math.Truncate((DateTime.Parse(item.User.DateAdm.ToString()).AddDays(85) - DateTime.Now).TotalDays);
              MailCheckpointSeq2(item, byte.Parse(((days < 0) ? 0 : days).ToString()));
            }

          }
        }
      }
      catch (Exception)
      {

      }
    }

    public async void CheckpointSeq3()
    {
      try
      {
        var nowLast = DateTime.Now.AddDays(-91).Date;
        var nowNext = DateTime.Now.AddDays(-89).Date;
        var persons = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.Checkpoint & p.User.DateAdm > nowLast & p.User.DateAdm < nowNext).ToList();
        foreach (var item in persons)
        {
          if (item.Manager != null)
          {
            if (checkpointService.GetAll(p => p.Person._id == item._id & p.StatusCheckpoint == EnumStatusCheckpoint.End).Count() == 0)
              MailCheckpointSeq3(item);
          }
        }
      }
      catch (Exception)
      {

      }
    }

    public async void MailPlanSeq1(Person person, double totaldays)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.PlanSeq1(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Days}", Math.Truncate(totaldays).ToString());
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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

    public async void MailPlanSeq1_Person(Person person, double totaldays)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.PlanSeq1_Person(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Days}", Math.Truncate(totaldays).ToString());
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.User.Mail, person.User.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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

    public async void MailPlanSeq2(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.PlanSeq2(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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

    public async void MailPlanSeq2_Person(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.PlanSeq2_Person(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.User.Mail, person.User.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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

    public async void MailPlanSeq3(Person person, double totaldays)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.PlanSeq3(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Days}", Math.Truncate(totaldays).ToString());
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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

    public async void MailPlanSeq3_Person(Person person, double totaldays)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.PlanSeq3_Person(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Days}", Math.Truncate(totaldays).ToString());
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.User.Mail, person.User.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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

    public async void MailMonitoringSeq1(Person person, double totaldays)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.MonitoringSeq1(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Days}", totaldays.ToString());
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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

    public async void MailMonitoringSeq1_Person(Person person, double totaldays)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.MonitoringSeq1_Person(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Days}", totaldays.ToString());
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.User.Mail, person.User.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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

    public async void MailCheckpointSeq1(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.CheckpointSeq1(path);
        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Days}", "30");
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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

    public async void MailCheckpointSeq2(Person person, byte days)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.CheckpointSeq1(path);
        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Days}", int.Parse(days.ToString()).ToString());
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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

    public async void MailCheckpointSeq3(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.CheckpointSeq2(path);
        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Days}", "90");
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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

    public async void MailOnboardingSeq1(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.OnBoardingSeq1(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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
        var model = mailModelService.OnBoardingSeq2(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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
        var model = mailModelService.OnBoardingSeq3(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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
        var model = mailModelService.OnBoardingSeq4(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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
        var model = mailModelService.OnBoardingSeq5(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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
        ViewPerson view = serviceAuthentication.AuthenticationMail(person);
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link);
          //var data = new
          //{
          //  mail = person.User.Mail,
          //  password = person.User.Password
          //};
          //var json = JsonConvert.SerializeObject(data);
          //var content = new StringContent(json);
          //content.Headers.ContentType.MediaType = "application/json";
          //client.DefaultRequestHeaders.Add("ContentType", "application/json");
          //var result = client.PostAsync("manager/authentication/encrypt", content).Result;
          //var resultContent = result.Content.ReadAsStringAsync().Result;
          //var auth = JsonConvert.DeserializeObject<ViewPerson>(resultContent);
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + view.Token);
          var resultMail = client.PostAsync("mail/sendmail/" + idmail, null).Result;
          return view.Token;
        }
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
  }
}
