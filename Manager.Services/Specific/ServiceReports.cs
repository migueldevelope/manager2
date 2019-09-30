using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Azure.ServiceBus;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
#pragma warning disable 4014
  public class ServiceReports : Repository<Monitoring>, IServiceReports
  {
    private readonly ServiceGeneric<Monitoring> serviceMonitoring;
    private readonly ServiceGeneric<OnBoarding> serviceOnboarding;
    private readonly ServiceGeneric<Occupation> serviceOccupation;
    private readonly ServiceGeneric<Area> serviceArea;
    private readonly ServiceGeneric<Checkpoint> serviceCheckpoint;
    private readonly ServiceGeneric<Workflow> serviceWorkflow;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Plan> servicePlan;
    private readonly ServiceGeneric<SalaryScale> serviceSalaryScale;
    private readonly ServiceLog serviceLog;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceGeneric<Account> serviceAccount;
    private readonly ServiceGeneric<Certification> serviceCertification;
    private readonly ServiceGeneric<CertificationPerson> serviceCertificationPerson;
    private readonly ServiceGeneric<Recommendation> serviceRecommendation;
    private readonly ServiceGeneric<RecommendationPerson> serviceRecommendationPerson;
    private readonly ServiceGeneric<Parameter> serviceParameter;
    private readonly ServiceGeneric<Reports> serviceReport;
    private readonly ServiceGeneric<Event> serviceEvent;
    private readonly IServicePerson serviceIPerson;

    public string path;
    private readonly IQueueClient queueClient;
    private readonly IQueueClient queueClientReturn;

    #region construtctor
    public ServiceReports(DataContext context, DataContext contextLog, string pathToken, IServicePerson _serviceIPerson, string serviceBusConnectionString)
      : base(context)
    {
      try
      {
        serviceMonitoring = new ServiceGeneric<Monitoring>(context);
        serviceOnboarding = new ServiceGeneric<OnBoarding>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        servicePlan = new ServiceGeneric<Plan>(context);
        serviceCheckpoint = new ServiceGeneric<Checkpoint>(context);
        serviceLog = new ServiceLog(context, contextLog);
        serviceMailModel = new ServiceMailModel(context);
        serviceMail = new ServiceGeneric<MailLog>(contextLog);
        serviceWorkflow = new ServiceGeneric<Workflow>(context);
        serviceAccount = new ServiceGeneric<Account>(context);
        serviceCertification = new ServiceGeneric<Certification>(context);
        serviceCertificationPerson = new ServiceGeneric<CertificationPerson>(context);
        serviceRecommendation = new ServiceGeneric<Recommendation>(context);
        serviceRecommendationPerson = new ServiceGeneric<RecommendationPerson>(context);
        serviceParameter = new ServiceGeneric<Parameter>(context);
        serviceSalaryScale = new ServiceGeneric<SalaryScale>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        serviceArea = new ServiceGeneric<Area>(context);
        serviceReport = new ServiceGeneric<Reports>(context);
        serviceEvent = new ServiceGeneric<Event>(context);
        serviceIPerson = _serviceIPerson;
        queueClient = new QueueClient(serviceBusConnectionString, "reports");
        queueClientReturn = new QueueClient(serviceBusConnectionString, "reportsreturn");
        path = pathToken;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      servicePerson._user = _user;
      serviceOnboarding._user = _user;
      serviceMonitoring._user = _user;
      servicePlan._user = _user;
      serviceLog._user = _user;
      serviceMailModel._user = _user;
      serviceWorkflow._user = _user;
      serviceMail._user = _user;
      serviceReport._user = _user;
      serviceCertification._user = _user;
      serviceCheckpoint._user = _user;
      serviceRecommendation._user = _user;
      serviceRecommendationPerson._user = _user;
      serviceParameter._user = _user;
      serviceSalaryScale._user = _user;
      serviceOccupation._user = _user;
      serviceArea._user = _user;
      serviceEvent._user = _user;
      serviceCertificationPerson._user = _user;
      serviceIPerson.SetUser(_user);
    }

    public void SetUser(BaseUser baseUser)
    {
      _user = baseUser;
      servicePerson._user = _user;
      serviceOnboarding._user = _user;
      serviceMonitoring._user = _user;
      servicePlan._user = _user;
      serviceLog._user = _user;
      serviceMailModel._user = _user;
      serviceWorkflow._user = _user;
      serviceMail._user = _user;
      serviceReport._user = _user;
      serviceCertification._user = _user;
      serviceCheckpoint._user = _user;
      serviceRecommendation._user = _user;
      serviceRecommendationPerson._user = _user;
      serviceParameter._user = _user;
      serviceSalaryScale._user = _user;
      serviceOccupation._user = _user;
      serviceArea._user = _user;
      serviceEvent._user = _user;
      serviceCertificationPerson._user = _user;
      serviceIPerson.SetUser(_user);
    }
    #endregion


    #region reports
    public string ListPersons()
    {
      try
      {
        var data = servicePerson.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result
          .Select(p => new _ViewListBase() { _id = p._id, Name = p.User.Name }).ToList();

        var view = new ViewReport()
        {
          Data = data,
          Name = "listpersons",
          _idReport = NewReport("listpersons"),
          _idAccount = _user._idAccount
        };
        SendMessageAsync(view);
        var report = new ViewCrudReport();

        while (report.StatusReport == EnumStatusReport.Open)
        {
          var rest = serviceReport.GetNewVersion(p => p._id == view._idReport).Result;
          report.StatusReport = rest.StatusReport;
          report.Link = rest.Link;
          //Thread.Sleep(1000);
        }

        return report.Link;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ListTraining(string idevent)
    {
      try
      {
        var events = serviceEvent.GetNewVersion(p => p._id == idevent).Result;

        List<ViewListTraining> data = new List<ViewListTraining>();
        List<string> dates = new List<string>();
        string instructors = "";

        if (events.Instructors != null)
        {
          foreach (var item in events.Instructors)
          {
            instructors += "\n" + item.Name;
          }
        }

        if (events.Days != null)
        {
          foreach (var item in events.Days)
          {
            dates.Add(item.Begin.ToString("dd/MM/yyyy") + "  " +
              item.Begin.ToString("HH:mm") + " - " + item.End.ToString("HH:mm"));
          }
        }

        if (events.Participants != null)
        {
          foreach (var item in events.Participants)
          {
            var viewEvent = new ViewListTraining()
            {
              NameEvent = events.Name,
              NameCourse = events.Course?.Name,
              Content = events.Content,
              DateBegin = events.Begin,
              DateEnd = events.End,
              NameEntity = events.Entity?.Name,
              NameParticipant = item.Name,
              Instructor = instructors,
            };
            if (dates.Count >= 1)
              viewEvent.Day1 = dates[0].ToString();
            if (dates.Count >= 2)
              viewEvent.Day2 = dates[1].ToString();
            if (dates.Count >= 3)
              viewEvent.Day3 = dates[2].ToString();
            if (dates.Count >= 4)
              viewEvent.Day4 = dates[3].ToString();
            if (dates.Count >= 5)
              viewEvent.Day5 = dates[4].ToString();
            if (dates.Count >= 6)
              viewEvent.Day6 = dates[5].ToString();
            if (dates.Count >= 7)
              viewEvent.Day7 = dates[6].ToString();
            if (dates.Count >= 8)
              viewEvent.Day8 = dates[7].ToString();
            if (dates.Count >= 9)
              viewEvent.Day9 = dates[8].ToString();
            if (dates.Count >= 10)
              viewEvent.Day10 = dates[9].ToString();
            if (dates.Count >= 11)
              viewEvent.Day11 = dates[10].ToString();
            if (dates.Count >= 12)
              viewEvent.Day12 = dates[11].ToString();
            if (dates.Count >= 13)
              viewEvent.Day13 = dates[12].ToString();
            if (dates.Count >= 14)
              viewEvent.Day14 = dates[13].ToString();
            if (dates.Count >= 15)
              viewEvent.Day15 = dates[14].ToString();

            data.Add(viewEvent);
          };
        }

        var view = new ViewReport()
        {
          Data = data,
          Name = "listtraining",
          _idReport = NewReport("listtraining"),
          _idAccount = _user._idAccount
        };
        SendMessageAsync(view);
        var report = new ViewCrudReport();

        while (report.StatusReport == EnumStatusReport.Open)
        {
          var rest = serviceReport.GetNewVersion(p => p._id == view._idReport).Result;
          report.StatusReport = rest.StatusReport;
          report.Link = rest.Link;
          //Thread.Sleep(1000);
        }

        return report.Link;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region ... private ...

    private string NewReport(string name)
    {
      try
      {
        var report = new Reports()
        {
          StatusReport = EnumStatusReport.Open,
          Date = DateTime.Now,
          Name = name
        };

        return serviceReport.InsertNewVersion(report).Result._id;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void SendMessageAsync(ViewReport view)
    {
      try
      {
        var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(view)));
        queueClient.SendAsync(message);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void RegisterOnMessageHandlerAndReceiveMesssages()
    {
      try
      {
        var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
        {
          MaxConcurrentCalls = 1,
          AutoComplete = false
        };

        queueClientReturn.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task ProcessMessagesAsync(Message message, CancellationToken token)
    {
      try
      {
        var view = JsonConvert.DeserializeObject<ViewCrudReport>(Encoding.UTF8.GetString(message.Body));
        SetUser(new BaseUser()
        {
          _idAccount = view._idAccount
        });

        if (view.StatusReport != EnumStatusReport.Open)
        {
          Reports report = serviceReport.GetFreeNewVersion(p => p._id == view._id).Result;
          report.StatusReport = view.StatusReport;
          report.Link = view.Link;
          serviceReport.UpdateAccount(report, null).Wait();

          await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
      }
      catch (Exception e)
      {
        var error = e.Message;
        queueClient.CompleteAsync(message.SystemProperties.LockToken);
      }

    }

    private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
    {
      var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
      return Task.CompletedTask;
    }

    #endregion
  }

}

#pragma warning restore 4014

