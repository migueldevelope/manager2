using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Services.WorkModel;
using Manager.Views.BusinessCrud;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace Manager.Services.Specific
{
  public class ServiceNotification : Repository<ConfigurationNotification>, IServiceNotification
  {
    private readonly ServiceGeneric<Account> serviceAccount;
    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceGeneric<ConfigurationNotification> serviceConfigurationNotification;
    private readonly ServiceGeneric<Checkpoint> serviceCheckpoint;
    private readonly ServiceLog serviceLog;
    private readonly ServiceLogMessages serviceLogMessages;
    private readonly ServiceGeneric<MailLog> serviceMailLog;
    private readonly ServiceGeneric<MailMessage> serviceMailMessage;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<Monitoring> serviceMonitoring;
    private readonly ServiceGeneric<OnBoarding> serviceOnboarding;
    private readonly ServiceGeneric<Parameter> serviceParameter;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Plan> servicePlan;
    private readonly string path;

    #region Constructor
    public ServiceNotification(DataContext context, DataContext contextLog, string _path, IServiceControlQueue serviceControlQueue) : base(context)
    {
      serviceAccount = new ServiceGeneric<Account>(context);
      serviceAuthentication = new ServiceAuthentication(context, contextLog, serviceControlQueue, _path);
      serviceCheckpoint = new ServiceGeneric<Checkpoint>(context);
      serviceConfigurationNotification = new ServiceGeneric<ConfigurationNotification>(context);
      serviceLog = new ServiceLog(context, contextLog);
      serviceLogMessages = new ServiceLogMessages(context);
      serviceMailLog = new ServiceGeneric<MailLog>(contextLog);
      serviceMailMessage = new ServiceGeneric<MailMessage>(contextLog);
      serviceMailModel = new ServiceMailModel(context);
      serviceMonitoring = new ServiceGeneric<Monitoring>(context);
      serviceParameter = new ServiceGeneric<Parameter>(context);
      servicePerson = new ServiceGeneric<Person>(context);
      servicePlan = new ServiceGeneric<Plan>(context);
      serviceOnboarding = new ServiceGeneric<OnBoarding>(context);
      path = _path;
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceAccount._user = _user;
      serviceCheckpoint._user = _user;
      serviceConfigurationNotification._user = _user;
      serviceLog.SetUser(_user);
      serviceLogMessages.SetUser(_user);
      serviceMailLog._user = _user;
      serviceMailMessage._user = _user;
      serviceMailModel.SetUser(_user);
      serviceMonitoring._user = _user;
      serviceParameter._user = _user;
      servicePerson._user = _user;
      servicePlan._user = _user;
      serviceOnboarding._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceAccount._user = user;
      serviceCheckpoint._user = user;
      serviceConfigurationNotification._user = user;
      serviceLog.SetUser(user);
      serviceLogMessages.SetUser(user);
      serviceMailLog._user = user;
      serviceMailMessage._user = user;
      serviceMailModel.SetUser(user);
      serviceMonitoring._user = user;
      serviceParameter._user = user;
      servicePerson._user = user;
      servicePlan._user = user;
      serviceOnboarding._user = user;
    }
    #endregion

    #region Start Message
    public void SendMessage()
    {
      try
      {
        // Aqui você ativa o teste para o Ricardo da resolution com true e false vai para a gestão.
        Send(false);
        var timer = new Timer
        {
          //24 hours em milliseconds
          Interval = 86400000
        };
        timer.Elapsed += new ElapsedEventHandler(Timer1_Tick);
        timer.Enabled = true;
        timer.Start();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void Timer1_Tick(object Sender, EventArgs e)
    {
      try
      {
        // Aqui você ativa o teste para o Ricardo da resolution com true e false vai para a gestão.
        Send(false);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    #endregion

    #region Send Messages
    private void Send(bool sendTest)
    {
      try
      {
        List<Account> accounts = serviceAccount.GetAllFreeNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        foreach (Account account in accounts)
        {
          SetUser(new BaseUser() { _idAccount = account._id });

          DateTime dateStart = DateTime.Now.Date;
          DateTime dateEnd = dateStart.AddDays(1);
          Log logExits = serviceLog.GetNewVersion(p => p.Local == "ManagerMessages" && p.DataLog >= dateStart && p.DataLog < dateEnd).Result;

          if (logExits == null)
          {
            Parameter parameter = serviceParameter.GetNewVersion(p => p.Key == "servicemailmessage" && p.Content == "1").Result;
            ViewLog log = new ViewLog
            {
              Description = "Service Notification",
              _idPerson = null,
              Local = "ManagerMessages"
            };
            serviceLog.NewLogService(log);
            //SendMessageSuccessFactory(sendTest);
            CheckpointManagerDeadlineV2(sendTest);
            if (parameter != null)
            {
              SendMessageAccount(sendTest);
            }
          }
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void SendMessageAccount(bool sendTest)
    {
      try
      {
        MailModel modelOnboardingAdmission = serviceMailModel.OnboardingAdmission(path);

        MailModel modelOnboardingEmployeeDeadline = serviceMailModel.OnboardingEmployeeDeadline(path);
        MailModel modelOnboardingManagerDeadline = serviceMailModel.OnboardingManagerDeadline(path);
        MailModel modelMonitoringManagerDeadline = serviceMailModel.MonitoringManagerDeadline(path);
        MailModel modelMonitoringDeadline = serviceMailModel.MonitoringDeadline(path);
        MailModel modelPlanManagerDeadline = serviceMailModel.PlanManagerDeadline(path);
        Log logOnboardingAdmission = null;
        Log logOnboardingManagerDeadline = null;
        Log logMonitoringManagerDeadline = null;
        Log logPlanManagerDeadline = null;

        //OnboardingAdmission
        if (modelOnboardingAdmission.TypeFrequence == EnumTypeFrequence.Daily)
        {
          logOnboardingAdmission = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesOnboardingAdmission").Result.Where(p => p.DataLog.Value.Day == DateTime.Now.Day).FirstOrDefault();
        }
        else if (modelOnboardingAdmission.TypeFrequence == EnumTypeFrequence.Weekly)
        {
          if (DateTime.Now.DayOfWeek == modelOnboardingAdmission.Weekly)
            logOnboardingAdmission = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesOnboardingAdmission").Result.Where(p => p.DataLog.Value.DayOfWeek == DateTime.Now.DayOfWeek
            && p.DataLog.Value.Month == DateTime.Now.Month).FirstOrDefault();
          else
            logOnboardingAdmission = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesOnboardingAdmission").Result.FirstOrDefault();
        }
        else if (modelOnboardingAdmission.TypeFrequence == EnumTypeFrequence.Weekly)
        {
          if (DateTime.Now.Day == modelOnboardingAdmission.Day)
            logOnboardingAdmission = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesOnboardingAdmission").Result.Where(p => p.DataLog.Value.Month == DateTime.Now.Month).FirstOrDefault();
          else
            logOnboardingAdmission = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesOnboardingAdmission").Result.FirstOrDefault();
        }

        if ((logOnboardingAdmission == null) && (modelOnboardingAdmission.StatusMail == EnumStatus.Enabled))
        {
          Parameter parameter = serviceParameter.GetNewVersion(p => p.Key == "servicemailmessage" && p.Content == "1").Result;
          ViewLog log = new ViewLog
          {
            Description = "Service Notification",
            _idPerson = null,
            Local = "ManagerMessagesOnboardingAdmission"
          };
          serviceLog.NewLogService(log);
          OnboardingAdmission(sendTest, modelOnboardingAdmission);
        }


        //OnboardingManagerDeadline
        if (modelOnboardingManagerDeadline.TypeFrequence == EnumTypeFrequence.Daily)
        {
          logOnboardingManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesOnboardingManagerDeadline").Result.Where(p => p.DataLog.Value.Day == DateTime.Now.Day).FirstOrDefault();
        }
        else if (modelOnboardingManagerDeadline.TypeFrequence == EnumTypeFrequence.Weekly)
        {
          if (DateTime.Now.DayOfWeek == modelOnboardingManagerDeadline.Weekly)
            logOnboardingManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesOnboardingManagerDeadline").Result.Where(p => p.DataLog.Value.DayOfWeek == DateTime.Now.DayOfWeek
            && p.DataLog.Value.Month == DateTime.Now.Month).FirstOrDefault();
          else
            logOnboardingManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesOnboardingManagerDeadline").Result.FirstOrDefault();
        }
        else if (modelOnboardingManagerDeadline.TypeFrequence == EnumTypeFrequence.Weekly)
        {
          if (DateTime.Now.Day == modelOnboardingManagerDeadline.Day)
            logOnboardingManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesOnboardingManagerDeadline").Result.Where(p => p.DataLog.Value.Month == DateTime.Now.Month).FirstOrDefault();
          else
            logOnboardingManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesOnboardingManagerDeadline").Result.FirstOrDefault();
        }

        if ((logOnboardingManagerDeadline == null) && (modelOnboardingManagerDeadline.StatusMail == EnumStatus.Enabled))
        {
          Parameter parameter = serviceParameter.GetNewVersion(p => p.Key == "servicemailmessage" && p.Content == "1").Result;
          ViewLog log = new ViewLog
          {
            Description = "Service Notification",
            _idPerson = null,
            Local = "ManagerMessagesOnboardingManagerDeadline"
          };
          serviceLog.NewLogService(log);
          //OnboardingManagerDeadline(sendTest, modelOnboardingAdmission);
          OnboardingManagerDeadlineV2(sendTest, modelOnboardingManagerDeadline, modelOnboardingEmployeeDeadline);
        }


        //MonitoringManagerDeadline
        if (modelMonitoringManagerDeadline.TypeFrequence == EnumTypeFrequence.Daily)
        {
          logMonitoringManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesMonitoringManagerDeadline").Result.Where(p => p.DataLog.Value.Day == DateTime.Now.Day).FirstOrDefault();
        }
        else if (modelMonitoringManagerDeadline.TypeFrequence == EnumTypeFrequence.Weekly)
        {
          if (DateTime.Now.DayOfWeek == modelMonitoringManagerDeadline.Weekly)
            logMonitoringManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesMonitoringManagerDeadline").Result.Where(p => p.DataLog.Value.DayOfWeek == DateTime.Now.DayOfWeek
            && p.DataLog.Value.Month == DateTime.Now.Month).FirstOrDefault();
          else
            logMonitoringManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesMonitoringManagerDeadline").Result.FirstOrDefault();
        }
        else if (modelMonitoringManagerDeadline.TypeFrequence == EnumTypeFrequence.Weekly)
        {
          if (DateTime.Now.Day == modelMonitoringManagerDeadline.Day)
            logMonitoringManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesMonitoringManagerDeadline").Result.Where(p => p.DataLog.Value.Month == DateTime.Now.Month).FirstOrDefault();
          else
            logMonitoringManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesMonitoringManagerDeadline").Result.FirstOrDefault();
        }

        if ((logMonitoringManagerDeadline == null) && (modelMonitoringManagerDeadline.StatusMail == EnumStatus.Enabled))
        {
          Parameter parameter = serviceParameter.GetNewVersion(p => p.Key == "servicemailmessage" && p.Content == "1").Result;
          ViewLog log = new ViewLog
          {
            Description = "Service Notification",
            _idPerson = null,
            Local = "ManagerMessagesMonitoringManagerDeadline"
          };
          serviceLog.NewLogService(log);
          MonitoringManagerDeadline(sendTest, modelMonitoringManagerDeadline, modelMonitoringDeadline);
        }


        //PlanManagerDeadline
        if (modelPlanManagerDeadline.TypeFrequence == EnumTypeFrequence.Daily)
        {
          logPlanManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesPlanManagerDeadline").Result.Where(p => p.DataLog.Value.Day == DateTime.Now.Day).FirstOrDefault();
        }
        else if (modelPlanManagerDeadline.TypeFrequence == EnumTypeFrequence.Weekly)
        {
          if (DateTime.Now.DayOfWeek == modelPlanManagerDeadline.Weekly)
            logPlanManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesPlanManagerDeadline").Result.Where(p => p.DataLog.Value.DayOfWeek == DateTime.Now.DayOfWeek
            && p.DataLog.Value.Month == DateTime.Now.Month).FirstOrDefault();
          else
            logPlanManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesPlanManagerDeadline").Result.FirstOrDefault();
        }
        else if (modelPlanManagerDeadline.TypeFrequence == EnumTypeFrequence.Weekly)
        {
          if (DateTime.Now.Day == modelPlanManagerDeadline.Day)
            logPlanManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesPlanManagerDeadline").Result.Where(p => p.DataLog.Value.Month == DateTime.Now.Month).FirstOrDefault();
          else
            logPlanManagerDeadline = serviceLog.GetAllNewVersion(p => p.Local == "ManagerMessagesPlanManagerDeadline").Result.FirstOrDefault();
        }

        if ((logPlanManagerDeadline == null) && (modelPlanManagerDeadline.StatusMail == EnumStatus.Enabled))
        {
          Parameter parameter = serviceParameter.GetNewVersion(p => p.Key == "servicemailmessage" && p.Content == "1").Result;
          ViewLog log = new ViewLog
          {
            Description = "Service Notification",
            _idPerson = null,
            Local = "ManagerMessagesPlanManagerDeadline"
          };
          serviceLog.NewLogService(log);
          PlanManagerDeadline(sendTest, modelPlanManagerDeadline);
        }

      }
      catch (Exception)
      {

      }
    }

    private void SendMessageSuccessFactory(bool sendTest)
    {
      try
      {
        BirthCompany(sendTest);
      }
      catch (Exception)
      {

      }
    }

    #endregion

    #region Checkpoint

    private void CheckpointManagerDeadlineV2(bool sendTest)
    {
      try
      {
        string daysCheckpointParameter = serviceParameter.GetNewVersion(p => p.Key == "DeadlineAdm").Result?.Content;
        int daysCheckpoint = -90;
        if (!string.IsNullOrEmpty(daysCheckpointParameter))
          daysCheckpoint = int.Parse(daysCheckpointParameter) * -1;

        List<ManagerWorkNotification> listManager = new List<ManagerWorkNotification>();
        // Checkpoint vencidos (sem data de admissão)
        //List<Person> persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.TypeJourney == EnumTypeJourney.Checkpoint && p.User.DateAdm == null && p.Manager != null).Result;
        //foreach (Person item in persons)
        //{
        //  if (serviceCheckpoint.CountNewVersion(p => p.Person._id == item._id && p.StatusCheckpoint == EnumStatusCheckpoint.End).Result == 0)
        //    listManager.Add(new ManagerWorkNotification()
        //    {
        //      Manager = item.Manager,
        //      Person = item,
        //      Type = ManagerListType.Defeated
        //    });
        //}
        // Checkpoint vencidos
        DateTime nowLimit = DateTime.Now.AddDays(daysCheckpoint - 1).Date;
        DateTime nowLimitLast = DateTime.Now.AddDays(daysCheckpoint - 10).Date;

        List<Person> persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.TypeJourney == EnumTypeJourney.Checkpoint && p.User.DateAdm <= nowLimit
        && p.User.DateAdm > nowLimitLast
        && p.Manager != null).Result;

        foreach (Person item in persons)
        {
          if (serviceCheckpoint.CountNewVersion(p => p.Person._id == item._id && p.StatusCheckpoint == EnumStatusCheckpoint.End).Result == 0)
          {
            listManager.Add(new ManagerWorkNotification()
            {
              Manager = item.Manager,
              Person = item,
              Type = ManagerListType.Defeated
            });

          }
        }
        // Checkpoint troca status
        nowLimit = DateTime.Now.AddDays(daysCheckpoint - 10).Date;
        persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.TypeJourney == EnumTypeJourney.Checkpoint && p.User.DateAdm <= nowLimit && p.Manager != null).Result;
        foreach (Person item in persons)
        {
          StatusCheckpoint(item);
        }

        // Checkpoint vencendo hoje
        nowLimit = DateTime.Now.AddDays(daysCheckpoint).Date;
        persons = servicePerson.GetAllNewVersion(p => p.TypeJourney == EnumTypeJourney.Checkpoint && p.User.DateAdm == nowLimit && p.Manager != null).Result;
        foreach (Person item in persons)
        {
          if (serviceCheckpoint.CountNewVersion(p => p.Person._id == item._id && p.StatusCheckpoint == EnumStatusCheckpoint.End).Result == 0)
            listManager.Add(new ManagerWorkNotification()
            {
              Manager = item.Manager,
              Person = item,
              Type = ManagerListType.DefeatedNow
            });
        }

        // Checkpoint vencendo até 10 dias
        var nowFirst = DateTime.Now.AddDays(daysCheckpoint + 10).Date;
        var nowLast = DateTime.Now.AddDays(daysCheckpoint + 10).Date;
        persons = servicePerson.GetAllNewVersion(p => p.TypeJourney == EnumTypeJourney.Checkpoint && p.User.DateAdm >= nowFirst && p.User.DateAdm <= nowLast && p.Manager != null).Result;
        foreach (Person item in persons)
        {
          if (serviceCheckpoint.CountNewVersion(p => p.Person._id == item._id && p.StatusCheckpoint == EnumStatusCheckpoint.End).Result == 0)
            listManager.Add(new ManagerWorkNotification()
            {
              Manager = item.Manager,
              Person = item,
              Type = ManagerListType.LastSevenDays
            });
        }

        //// Checkpoint vencendo até 7 dias
        //var nowFirst = DateTime.Now.AddDays(daysCheckpoint + 7).Date;
        //var nowLast = DateTime.Now.AddDays(daysCheckpoint + 1).Date;
        //persons = servicePerson.GetAllNewVersion(p => p.TypeJourney == EnumTypeJourney.Checkpoint && p.User.DateAdm >= nowFirst && p.User.DateAdm <= nowLast && p.Manager != null).Result;
        //foreach (Person item in persons)
        //{
        //  if (serviceCheckpoint.CountNewVersion(p => p.Person._id == item._id && p.StatusCheckpoint == EnumStatusCheckpoint.End).Result == 0)
        //    listManager.Add(new ManagerWorkNotification()
        //    {
        //      Manager = item.Manager,
        //      Person = item,
        //      Type = ManagerListType.LastSevenDays
        //    });
        //}
        //// Checkpoint vencendo em 15 dias
        //nowLimit = DateTime.Now.AddDays(daysCheckpoint + 15).Date;
        //persons = servicePerson.GetAllNewVersion(p => p.TypeJourney == EnumTypeJourney.Checkpoint && p.User.DateAdm == nowLimit && p.Manager != null).Result;
        //foreach (Person item in persons)
        //{
        //  if (serviceCheckpoint.CountNewVersion(p => p.Person._id == item._id && p.StatusCheckpoint == EnumStatusCheckpoint.End).Result == 0)
        //    listManager.Add(new ManagerWorkNotification()
        //    {
        //      Manager = item.Manager,
        //      Person = item,
        //      Type = ManagerListType.FifteenDays
        //    });
        //}
        //// Checkpoint vencendo em 30 dias
        //nowLimit = DateTime.Now.AddDays(daysCheckpoint + 30).Date;
        //persons = servicePerson.GetAllNewVersion(p => p.TypeJourney == EnumTypeJourney.Checkpoint && p.User.DateAdm == nowLimit && p.Manager != null).Result;
        //foreach (Person item in persons)
        //{
        //  if (serviceCheckpoint.CountNewVersion(p => p.Person._id == item._id && p.StatusCheckpoint == EnumStatusCheckpoint.End).Result == 0)
        //    listManager.Add(new ManagerWorkNotification()
        //    {
        //      Manager = item.Manager,
        //      Person = item,
        //      Type = ManagerListType.ThirtyDays
        //    });
        //}
        if (listManager.Count > 0)
        {
          // Emitir e-mails
          List<ManagerNotification> listManagerNotification = new List<ManagerNotification>();
          listManager = listManager.OrderBy(o => o.Type).OrderBy(o => o.Manager.Name).ToList();
          ManagerNotification managerNotification = new ManagerNotification()
          {
            Manager = listManager[0].Manager,
            Defeated = new List<Person>(),
            DefeatedNow = new List<Person>(),
            LastSevenDays = new List<Person>(),
            FifteenDays = new List<Person>(),
            ThirtyDays = new List<Person>(),
          };
          foreach (var item in listManager)
          {
            if (managerNotification.Manager.Name != item.Manager.Name)
            {
              managerNotification.Defeated = managerNotification.Defeated.OrderBy(o => o.User.Name).ToList();
              managerNotification.DefeatedNow = managerNotification.DefeatedNow.OrderBy(o => o.User.Name).ToList();
              managerNotification.LastSevenDays = managerNotification.LastSevenDays.OrderBy(o => o.User.DateAdm).OrderBy(o => o.User.Name).ToList();
              managerNotification.FifteenDays = managerNotification.FifteenDays.OrderBy(o => o.User.Name).ToList();
              managerNotification.ThirtyDays = managerNotification.ThirtyDays.OrderBy(o => o.User.Name).ToList();
              listManagerNotification.Add(managerNotification);
              managerNotification = new ManagerNotification()
              {
                Manager = item.Manager,
                Defeated = new List<Person>(),
                DefeatedNow = new List<Person>(),
                LastSevenDays = new List<Person>(),
                FifteenDays = new List<Person>(),
                ThirtyDays = new List<Person>(),
              };
            }
            switch (item.Type)
            {
              case ManagerListType.Defeated:
                managerNotification.Defeated.Add(item.Person);
                break;
              case ManagerListType.DefeatedNow:
                managerNotification.DefeatedNow.Add(item.Person);
                break;
              case ManagerListType.LastSevenDays:
                managerNotification.LastSevenDays.Add(item.Person);
                break;
              case ManagerListType.FifteenDays:
                managerNotification.FifteenDays.Add(item.Person);
                break;
              case ManagerListType.ThirtyDays:
                managerNotification.ThirtyDays.Add(item.Person);
                break;
              default:
                break;
            }
          }
          managerNotification.Defeated = managerNotification.Defeated.OrderBy(o => o.User.Name).ToList();
          managerNotification.DefeatedNow = managerNotification.DefeatedNow.OrderBy(o => o.User.Name).ToList();
          managerNotification.LastSevenDays = managerNotification.LastSevenDays.OrderBy(o => o.User.DateAdm).OrderBy(o => o.User.Name).ToList();
          managerNotification.FifteenDays = managerNotification.FifteenDays.OrderBy(o => o.User.Name).ToList();
          managerNotification.ThirtyDays = managerNotification.ThirtyDays.OrderBy(o => o.User.Name).ToList();
          listManagerNotification.Add(managerNotification);
          MailCheckpointManagerDeadline(listManagerNotification, sendTest);
        }
      }
      catch (Exception)
      {

      }
    }
    private void MailCheckpointManagerDeadline(List<ManagerNotification> listManager, bool sendTest)
    {
      try
      {
        //searsh model mail database
        MailModel model = serviceMailModel.CheckpointManagerDeadline(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        Person personManager = servicePerson.GetNewVersion(p => p._id == listManager[0].Manager._id).Result;

        foreach (var item in listManager)
        {
          string body = model.Message.Replace("{Link}", model.Link)
                                      .Replace("{Manager}", item.Manager.Name);

          string list = string.Empty;
          foreach (var person in item.Defeated)
            if (person.User.DateAdm == null)
              list = string.Concat(list, string.Format("{0} - {1}<br>", "Sem Data", person.User.Name));
            else
              list = string.Concat(list, string.Format("{0} - {1}<br>", ((DateTime)person.User.DateAdm).ToString("dd/MM/yyyy"), person.User.Name));

          if (!string.IsNullOrEmpty(list))
          {
            list = string.Concat("Colaboradores que estão com o <strong>prazo expirado</strong>:<br>", list, "<br>");
          }
          body = body.Replace("{LIST1}", list);

          list = string.Empty;
          foreach (var person in item.DefeatedNow)
            list = string.Concat(list, string.Format("{0}<br>", person.User.Name));
          if (!string.IsNullOrEmpty(list))
          {
            list = string.Concat("Colaboradores onde a jornada <strong>vence hoje</strong>:<br>", list, "<br>");
          }
          body = body.Replace("{LIST2}", list);

          list = string.Empty;
          foreach (var person in item.LastSevenDays)
            list = string.Concat(list, string.Format("{0}<br>", person.User.Name));
          if (!string.IsNullOrEmpty(list))
          {
            list = string.Concat("Colaboradores onde a jornada <strong>vence em 10 dias</strong>:<br>", list, "<br>");
          }
          body = body.Replace("{LIST3}", list);

          list = string.Empty;
          foreach (var person in item.FifteenDays)
            list = string.Concat(list, string.Format("{0}<br>", person.User.Name));
          if (!string.IsNullOrEmpty(list))
          {
            list = string.Concat("Colaboradores onde a jornada <strong>vence em 15 dias</strong>:<br>", list, "<br>");
          }
          body = body.Replace("{LIST4}", list);

          list = string.Empty;
          foreach (var person in item.ThirtyDays)
            list = string.Concat(list, string.Format("{0}<br>", person.User.Name));
          if (!string.IsNullOrEmpty(list))
          {
            list = string.Concat("Colaboradores onde a jornada <strong>vence em 30 dias</strong>:<br>", list, "<br>");
          }
          body = body.Replace("{LIST5}", list);

          MailLog sendMail = new MailLog
          {
            From = new MailLogAddress("suporte@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
            To = sendTest ?
              new List<MailLogAddress>()
                {
                  new MailLogAddress("ricardo@resolution.com.br", "Ricardo teste mensageria")
                } :
              new List<MailLogAddress>()
                {
                  new MailLogAddress(item.Manager.Mail, item.Manager.Name)
                },
            Priority = EnumPriorityMail.Low,
            _idPerson = item.Manager._id,
            NamePerson = item.Manager.Name,
            Body = body,
            StatusMail = EnumStatusMail.Sended,
            Included = DateTime.Now,
            Subject = model.Subject
          };
          if (personManager != null)
          {
            sendMail = serviceMailLog.InsertNewVersion(sendMail).Result;
            string token = SendMailApi(path, personManager, sendMail._id);
          }
          else
          {
            sendMail.StatusMail = EnumStatusMail.Error;
            sendMail.MessageError = "Manager null in checkpoint messages.";
            sendMail = serviceMailLog.InsertNewVersion(sendMail).Result;
          }
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void StatusCheckpoint(Person person)
    {
      try
      {
        var checkpoint = serviceCheckpoint.GetNewVersion(p => p.Person._id == person._id).Result;
        if (checkpoint != null)
        {
          checkpoint.StatusCheckpoint = EnumStatusCheckpoint.End;
          var c = serviceCheckpoint.Update(checkpoint, null);
        }
        person.TypeJourney = EnumTypeJourney.Monitoring;
        var x = servicePerson.Update(person, null);

      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion


    #region Monitoring
    private void MonitoringManagerDeadline(bool sendTest, MailModel model, MailModel modelPerson)
    {
      try
      {
        //{DAYS}
        var parameter = serviceParameter.GetNewVersion(p => p.Key == "DeadlineMonitoring").Result;

        int daysMonitoring = -90;
        if (parameter.Content != "")
          daysMonitoring = int.Parse(parameter.Content) * -1;

        List<ManagerWorkNotification> listManager = new List<ManagerWorkNotification>();
        // Monitoring
        DateTime nowLimit = DateTime.Now.AddDays(daysMonitoring - 1).Date;
        List<Person> persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.TypeJourney == EnumTypeJourney.Monitoring && p.Manager != null).Result;
        foreach (Person item in persons)
        {
          if ((serviceMonitoring.CountNewVersion(p => p.Person._id == item._id && p.StatusMonitoring == EnumStatusMonitoring.End && p.DateEndEnd >= nowLimit).Result == 0)
            && ((serviceMonitoring.CountNewVersion(p => p.Person._id == item._id && (p.StatusMonitoring == EnumStatusMonitoring.InProgressPerson || p.StatusMonitoring == EnumStatusMonitoring.Wait)).Result == 0)))
            //&& (serviceOnboarding.CountNewVersion(p => p.Person._id == item._id && p.StatusOnBoarding == EnumStatusOnBoarding.End && p.DateEndEnd >= nowLimit).Result == 0)
            //&& (serviceCheckpoint.CountNewVersion(p => p.Person._id == item._id && p.StatusCheckpoint == EnumStatusCheckpoint.End && p.DateEnd >= nowLimit).Result == 0))
            listManager.Add(new ManagerWorkNotification()
            {
              Manager = item.Manager,
              Person = item,
              Type = ManagerListType.Defeated
            });
          else if (serviceMonitoring.CountNewVersion(p => p.Person._id == item._id && (p.StatusMonitoring == EnumStatusMonitoring.InProgressPerson || p.StatusMonitoring == EnumStatusMonitoring.Wait)).Result > 0)
            MailMonitoringEmployeeDeadline(item, sendTest);
        }
        if (listManager.Count > 0)
        {
          // Emitir e-mails
          List<ManagerNotification> listManagerNotification = new List<ManagerNotification>();
          listManager = listManager.OrderBy(o => o.Type).OrderBy(o => o.Manager.Name).ToList();
          ManagerNotification managerNotification = new ManagerNotification()
          {
            Manager = listManager[0].Manager,
            Defeated = new List<Person>(),
            DefeatedNow = new List<Person>(),
            LastSevenDays = new List<Person>(),
            FifteenDays = new List<Person>(),
            ThirtyDays = new List<Person>(),
          };
          foreach (var item in listManager)
          {
            if (managerNotification.Manager.Name != item.Manager.Name)
            {
              managerNotification.Defeated = managerNotification.Defeated.OrderBy(o => o.User.Name).ToList();
              listManagerNotification.Add(managerNotification);
              managerNotification = new ManagerNotification()
              {
                Manager = item.Manager,
                Defeated = new List<Person>(),
                DefeatedNow = new List<Person>(),
                LastSevenDays = new List<Person>(),
                FifteenDays = new List<Person>(),
                ThirtyDays = new List<Person>(),
              };
            }
            switch (item.Type)
            {
              case ManagerListType.Defeated:
                managerNotification.Defeated.Add(item.Person);
                break;
              case ManagerListType.DefeatedNow:
                break;
              case ManagerListType.LastSevenDays:
                break;
              case ManagerListType.FifteenDays:
                break;
              case ManagerListType.ThirtyDays:
                break;
              default:
                break;
            }
          }
          managerNotification.Defeated = managerNotification.Defeated.OrderBy(o => o.User.Name).ToList();
          listManagerNotification.Add(managerNotification);
          // Enviar para o gestor
          MailMonitoringManagerDeadline(listManagerNotification, sendTest);

          // Enviar para o colaborador
          List<Person> listPerson = new List<Person>();
          foreach (var item in listManager)
          {
            MailMonitoringDeadlinePerson(item.Person, sendTest, modelPerson);
          }


          //if (listPerson.Count > 0)
          //  MailMonitoringDeadline(listPerson, sendTest, model);
        }
      }
      catch (Exception)
      {

      }
    }
    private void MailMonitoringManagerDeadline(List<ManagerNotification> listManager, bool sendTest)
    {
      try
      {
        Person personManager = servicePerson.GetNewVersion(p => p._id == listManager[0].Manager._id).Result;
        var parameter = serviceParameter.GetNewVersion(p => p.Key == "DeadlineMonitoring").Result;

        //searsh model mail database
        MailModel model = serviceMailModel.MonitoringManagerDeadline(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;
        foreach (var item in listManager)
        {
          string body = model.Message.Replace("{Link}", model.Link)
                                      .Replace("{Manager}", item.Manager.Name).Replace("{DAYS}", parameter.Content);

          string list = string.Empty;
          foreach (var person in item.Defeated)
            list = string.Concat(list, string.Format("{0}<br>", person.User.Name));

          if (!string.IsNullOrEmpty(list))
          {
            list = string.Concat("Colaboradores sem <strong>feedback hà mais de " + parameter.Content + " dias</strong>:<br>", list, "<br>");
          }
          body = body.Replace("{LIST1}", list);

          MailLog sendMail = new MailLog
          {
            From = new MailLogAddress("suporte@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
            To = sendTest ?
              new List<MailLogAddress>()
              {
                new MailLogAddress("ricardo@resolution.com.br", "Ricardo teste mensageria")
              } :
              new List<MailLogAddress>()
              {
                new MailLogAddress(item.Manager.Mail, item.Manager.Name)
              },
            Priority = EnumPriorityMail.Low,
            _idPerson = item.Manager._id,
            NamePerson = item.Manager.Name,
            Body = body,
            StatusMail = EnumStatusMail.Sended,
            Included = DateTime.Now,
            Subject = model.Subject
          };
          if (personManager != null)
          {
            sendMail = serviceMailLog.InsertNewVersion(sendMail).Result;
            string token = SendMailApi(path, personManager, sendMail._id);
          }
          else
          {
            sendMail.StatusMail = EnumStatusMail.Error;
            sendMail.MessageError = "Manager null in monitoring manager messages.";
            sendMail = serviceMailLog.InsertNewVersion(sendMail).Result;
          }
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    private void MailMonitoringEmployeeDeadline(Person item, bool sendTest)
    {
      try
      {
        var parameter = serviceParameter.GetNewVersion(p => p.Key == "DeadlineMonitoring").Result;

        //searsh model mail database
        MailModel model = serviceMailModel.MonitoringManagerDeadline(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        string body = model.Message.Replace("{Link}", model.Link).Replace("{Person}", item.User.Name)
                                    .Replace("{Manager}", item.Manager.Name).Replace("{DAYS}", parameter.Content);

       
        MailLog sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
          To = sendTest ?
            new List<MailLogAddress>()
            {
                new MailLogAddress("ricardo@resolution.com.br", "Ricardo teste mensageria")
            } :
            new List<MailLogAddress>()
            {
                new MailLogAddress(item.User.Mail, item.User.Name)
            },
          Priority = EnumPriorityMail.Low,
          _idPerson = item._id,
          NamePerson = item.User.Name,
          Body = body,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = model.Subject
        };
        sendMail = serviceMailLog.InsertNewVersion(sendMail).Result;
        string token = SendMailApi(path, item, sendMail._id);

      
      }
      catch (Exception e)
      {
        throw e;
      }
}

private void MailMonitoringDeadline(List<Person> listPerson, bool sendTest, MailModel model)
{
  try
  {
    //{DAYS}
    var parameter = serviceParameter.GetNewVersion(p => p.Key == "DeadlineMonitoring").Result;

    if (model.StatusMail == EnumStatus.Disabled)
      return;

    foreach (var item in listPerson)
    {
      string body = model.Message.Replace("{Link}", model.Link)
                                  .Replace("{Person}", item.User.Name).Replace("{DAYS}", parameter.Content);

      MailLog sendMail = new MailLog
      {
        From = new MailLogAddress("suporte@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
        To = sendTest ?
          new List<MailLogAddress>()
            {
                  new MailLogAddress("ricardo@resolution.com.br", "Ricardo teste mensageria")
            } :
          new List<MailLogAddress>()
            {
                  new MailLogAddress(item.Manager.Mail, item.Manager.Name)
            },
        Priority = EnumPriorityMail.Low,
        _idPerson = item._id,
        NamePerson = item.User.Name,
        Body = body,
        StatusMail = EnumStatusMail.Sended,
        Included = DateTime.Now,
        Subject = model.Subject
      };
      //MailLog mailObj = serviceMailLog.InsertNewVersion(sendMail).Result;
      sendMail = serviceMailLog.InsertNewVersion(sendMail).Result;
      string token = SendMailApi(path, item, sendMail._id);
    }
  }
  catch (Exception e)
  {
    throw e;
  }
}

private void MailMonitoringDeadlinePerson(Person item, bool sendTest, MailModel model)
{
  try
  {
    //{DAYS}
    var parameter = serviceParameter.GetNewVersion(p => p.Key == "DeadlineMonitoring").Result;

    if (model.StatusMail == EnumStatus.Disabled)
      return;

    string body = model.Message.Replace("{Link}", model.Link).Replace("{Manager}", item.Manager?.Name)
                                .Replace("{Person}", item.User.Name).Replace("{DAYS}", parameter.Content);

    MailLog sendMail = new MailLog
    {
      From = new MailLogAddress("suporte@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
      To = sendTest ?
        new List<MailLogAddress>()
          {
                  new MailLogAddress("ricardo@resolution.com.br", "Ricardo teste mensageria")
          } :
        new List<MailLogAddress>()
          {
                  new MailLogAddress(item.User.Mail, item.User.Name)
          },
      Priority = EnumPriorityMail.Low,
      _idPerson = item._id,
      NamePerson = item.User.Name,
      Body = body,
      StatusMail = EnumStatusMail.Sended,
      Included = DateTime.Now,
      Subject = model.Subject
    };
    //MailLog mailObj = serviceMailLog.InsertNewVersion(sendMail).Result;
    sendMail = serviceMailLog.InsertNewVersion(sendMail).Result;
    string token = SendMailApi(path, item, sendMail._id);

  }
  catch (Exception e)
  {
    throw e;
  }
}

#endregion

#region Onboarding
private void OnboardingAdmission(bool sendTest, MailModel model)
{
  try
  {
    List<Person> persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.TypeJourney == EnumTypeJourney.OnBoarding && p.Manager != null).Result;
    foreach (Person item in persons)
    {
      ViewCrudLogMessages view = new ViewCrudLogMessages()
      {
        Subject = "Novo Colaborador",
        Message = string.Format("{0} admitido(a)!", item.User.Name),
        Person = item.GetViewListBase(),
        StatusMessage = EnumStatusMessage.New
      };
      view = serviceLogMessages.NewNotExist(view);
      if (view != null)
        if (serviceOnboarding.CountNewVersion(p => p.Person._id == item._id && p.StatusOnBoarding == EnumStatusOnBoarding.End).Result == 0)
          MailOnboardingAdmission(item, sendTest, model);
    }
  }
  catch (Exception)
  {

  }
}
private void MailOnboardingAdmission(Person person, bool sendTest, MailModel model)
{
  try
  {
    //searsh model mail database


    string body = model.Message.Replace("{Person}", person.User.Name)
                               .Replace("{Link}", model.Link)
                               .Replace("{Manager}", person.Manager.Name)
                               .Replace("{Company}", person.Company.Name)
                               .Replace("{Occupation}", person.Occupation.Name);

    MailLog sendMail = new MailLog
    {
      From = new MailLogAddress("suporte@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
      To = sendTest ?
        new List<MailLogAddress>()
          {
                new MailLogAddress("ricardo@resolution.com.br", "Ricardo teste mensageria")
          } :
        new List<MailLogAddress>()
          {
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
    sendMail = serviceMailLog.InsertNewVersion(sendMail).Result;
    string token = SendMailApi(path, person, sendMail._id);
  }
  catch (Exception e)
  {
    throw e;
  }
}

private void OnboardingManagerDeadlineV2(bool sendTest, MailModel model, MailModel modelEmployee)
{
  try
  {
    Parameter parameter = serviceParameter.GetNewVersion(p => p.Key == "DeadlineAdmOnboarding").Result;
    //int daysOnboarding = -30;
    int daysOnboarding = int.Parse(parameter.Content) * -1;
    List<ManagerWorkNotification> listManager = new List<ManagerWorkNotification>();
    // Onboarding vencidos (sem data de admissão)
    List<Person> persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.TypeJourney == EnumTypeJourney.OnBoarding && p.User.DateAdm == null && p.Manager != null).Result;
    foreach (Person item in persons)
    {
      if (serviceOnboarding.CountNewVersion(p => p.Person._id == item._id && p.StatusOnBoarding == EnumStatusOnBoarding.End).Result == 0)
        listManager.Add(new ManagerWorkNotification()
        {
          Manager = item.Manager,
          Person = item,
          Type = ManagerListType.Defeated
        });

      if (serviceOnboarding.CountNewVersion(p => p.Person._id == item._id && (p.StatusOnBoarding == EnumStatusOnBoarding.WaitPerson || p.StatusOnBoarding == EnumStatusOnBoarding.InProgressPerson)).Result == 0)
        MailMonitoringDeadlinePerson(item, sendTest, modelEmployee);

    }
    // Onboarding vencidos
    DateTime nowLimit = DateTime.Now.AddDays(daysOnboarding - 1).Date;

    persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.TypeJourney == EnumTypeJourney.OnBoarding && p.User.DateAdm <= nowLimit && p.Manager != null).Result;
    ////var person1 = servicePerson.GetNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.TypeJourney == EnumTypeJourney.OnBoarding && p._id == "5cee660a00212a7a305be6cb" && p.Manager != null).Result;
    ////var person2 = servicePerson.GetNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.TypeJourney == EnumTypeJourney.OnBoarding 
    ////&& p._id == "5cee660a00212a7a305be6cb" && p.Manager != null && p.User.DateAdm <= nowLimit).Result;
    ////if(person1.User.DateAdm <= nowLimit)
    ////{
    ////  var a = "test";
    ////}

    foreach (Person item in persons)
    {
      if ((serviceOnboarding.CountNewVersion(p => p.Person._id == item._id && p.StatusOnBoarding == EnumStatusOnBoarding.End).Result == 0)
        && (serviceOnboarding.CountNewVersion(p => p.Person._id == item._id && p.StatusOnBoarding == EnumStatusOnBoarding.WaitPerson).Result == 0))
        listManager.Add(new ManagerWorkNotification()
        {
          Manager = item.Manager,
          Person = item,
          Type = ManagerListType.Defeated
        });
    }
    // Onboarding vencendo hoje
    nowLimit = DateTime.Now.AddDays(daysOnboarding).Date;
    persons = servicePerson.GetAllNewVersion(p => p.TypeJourney == EnumTypeJourney.OnBoarding && p.User.DateAdm == nowLimit && p.Manager != null).Result;
    foreach (Person item in persons)
    {
      if (serviceOnboarding.CountNewVersion(p => p.Person._id == item._id && p.StatusOnBoarding == EnumStatusOnBoarding.End).Result == 0)
        listManager.Add(new ManagerWorkNotification()
        {
          Manager = item.Manager,
          Person = item,
          Type = ManagerListType.DefeatedNow
        });
    }
    // Onboarding vencendo até 10 dias
    var nowFirst = DateTime.Now.AddDays(daysOnboarding + 10).Date;
    var nowLast = DateTime.Now.AddDays(daysOnboarding + 10).Date;
    persons = servicePerson.GetAllNewVersion(p => p.TypeJourney == EnumTypeJourney.OnBoarding && p.User.DateAdm >= nowFirst && p.User.DateAdm <= nowLast && p.Manager != null).Result;
    foreach (Person item in persons)
    {
      if (serviceOnboarding.CountNewVersion(p => p.Person._id == item._id && p.StatusOnBoarding == EnumStatusOnBoarding.End).Result == 0)
        listManager.Add(new ManagerWorkNotification()
        {
          Manager = item.Manager,
          Person = item,
          Type = ManagerListType.LastSevenDays
        });
    }
    //// Onboarding vencendo até 7 dias
    //var nowFirst = DateTime.Now.AddDays(daysOnboarding + 7).Date;
    //var nowLast = DateTime.Now.AddDays(daysOnboarding + 1).Date;
    //persons = servicePerson.GetAllNewVersion(p => p.TypeJourney == EnumTypeJourney.OnBoarding && p.User.DateAdm >= nowFirst && p.User.DateAdm <= nowLast && p.Manager != null).Result;
    //foreach (Person item in persons)
    //{
    //  if (serviceOnboarding.CountNewVersion(p => p.Person._id == item._id && p.StatusOnBoarding == EnumStatusOnBoarding.End).Result == 0)
    //    listManager.Add(new ManagerWorkNotification()
    //    {
    //      Manager = item.Manager,
    //      Person = item,
    //      Type = ManagerListType.LastSevenDays
    //    });
    //}
    //// Onboarding vencendo em 15 dias
    //nowLimit = DateTime.Now.AddDays(daysOnboarding + 15).Date;
    //persons = servicePerson.GetAllNewVersion(p => p.TypeJourney == EnumTypeJourney.OnBoarding && p.User.DateAdm == nowLimit && p.Manager != null).Result;
    //foreach (Person item in persons)
    //{
    //  listManager.Add(new ManagerWorkNotification()
    //  {
    //    Manager = item.Manager,
    //    Person = item,
    //    Type = ManagerListType.FifteenDays
    //  });
    //}
    if (listManager.Count > 0)
    {
      // Emitir e-mails
      List<ManagerNotification> listManagerNotification = new List<ManagerNotification>();
      listManager = listManager.OrderBy(o => o.Type).OrderBy(o => o.Manager.Name).ToList();
      ManagerNotification managerNotification = new ManagerNotification()
      {
        Manager = listManager[0].Manager,
        Defeated = new List<Person>(),
        DefeatedNow = new List<Person>(),
        LastSevenDays = new List<Person>(),
        FifteenDays = new List<Person>(),
        ThirtyDays = new List<Person>(),
      };
      foreach (var item in listManager)
      {
        if (managerNotification.Manager.Name != item.Manager.Name)
        {
          managerNotification.Defeated = managerNotification.Defeated.OrderBy(o => o.User.Name).ToList();
          managerNotification.DefeatedNow = managerNotification.DefeatedNow.OrderBy(o => o.User.Name).ToList();
          managerNotification.LastSevenDays = managerNotification.LastSevenDays.OrderBy(o => o.User.DateAdm).OrderBy(o => o.User.Name).ToList();
          managerNotification.FifteenDays = managerNotification.FifteenDays.OrderBy(o => o.User.Name).ToList();
          listManagerNotification.Add(managerNotification);
          managerNotification = new ManagerNotification()
          {
            Manager = item.Manager,
            Defeated = new List<Person>(),
            DefeatedNow = new List<Person>(),
            LastSevenDays = new List<Person>(),
            FifteenDays = new List<Person>(),
            ThirtyDays = new List<Person>(),
          };
        }
        switch (item.Type)
        {
          case ManagerListType.Defeated:
            managerNotification.Defeated.Add(item.Person);
            break;
          case ManagerListType.DefeatedNow:
            managerNotification.DefeatedNow.Add(item.Person);
            break;
          case ManagerListType.LastSevenDays:
            managerNotification.LastSevenDays.Add(item.Person);
            break;
          case ManagerListType.FifteenDays:
            managerNotification.FifteenDays.Add(item.Person);
            break;
          case ManagerListType.ThirtyDays:
            break;
          default:
            break;
        }
      }
      managerNotification.Defeated = managerNotification.Defeated.OrderBy(o => o.User.Name).ToList();
      managerNotification.DefeatedNow = managerNotification.DefeatedNow.OrderBy(o => o.User.Name).ToList();
      managerNotification.LastSevenDays = managerNotification.LastSevenDays.OrderBy(o => o.User.DateAdm).OrderBy(o => o.User.Name).ToList();
      managerNotification.FifteenDays = managerNotification.FifteenDays.OrderBy(o => o.User.Name).ToList();
      listManagerNotification.Add(managerNotification);
      MailOnboardingManagerDeadline(listManagerNotification, sendTest, model);
    }
  }
  catch (Exception)
  {

  }
}

private void MailOnboardingManagerDeadline(List<ManagerNotification> listManager, bool sendTest, MailModel model)
{
  try
  {
    Person personManager = servicePerson.GetNewVersion(p => p._id == listManager[0].Manager._id).Result;

    if (model.StatusMail == EnumStatus.Disabled)
      return;
    foreach (var item in listManager)
    {
      string body = model.Message.Replace("{Link}", model.Link)
                                  .Replace("{Manager}", item.Manager.Name);

      string list = string.Empty;
      foreach (var person in item.Defeated)
        if (person.User.DateAdm == null)
          list = string.Concat(list, string.Format("{0} - {1}<br>", "Sem Data", person.User.Name));
        else
          list = string.Concat(list, string.Format("{0} - {1}<br>", ((DateTime)person.User.DateAdm).ToString("dd/MM/yyyy"), person.User.Name));

      if (!string.IsNullOrEmpty(list))
      {
        list = string.Concat("Colaboradores que estão <strong>prazo expirado</strong>:<br>", list, "<br>");
      }
      body = body.Replace("{LIST1}", list);

      list = string.Empty;
      foreach (var person in item.DefeatedNow)
        list = string.Concat(list, string.Format("{0}<br>", person.User.Name));
      if (!string.IsNullOrEmpty(list))
      {
        list = string.Concat("Colaboradores onde a jornada <strong>vence hoje</strong>:<br>", list, "<br>");
      }
      body = body.Replace("{LIST2}", list);

      list = string.Empty;
      foreach (var person in item.LastSevenDays)
        list = string.Concat(list, string.Format("{0}<br>", person.User.Name));
      if (!string.IsNullOrEmpty(list))
      {
        list = string.Concat("Colaboradores onde a jornada <strong>vence em 10 dias</strong>:<br>", list, "<br>");
      }
      body = body.Replace("{LIST3}", list);

      list = string.Empty;
      foreach (var person in item.FifteenDays)
        list = string.Concat(list, string.Format("{0}<br>", person.User.Name));
      if (!string.IsNullOrEmpty(list))
      {
        list = string.Concat("Colaboradores onde a jornada <strong>vence em 15 dias</strong>:<br>", list, "<br>");
      }
      body = body.Replace("{LIST4}", list);

      MailLog sendMail = new MailLog
      {
        From = new MailLogAddress("suporte@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
        To = sendTest ?
          new List<MailLogAddress>()
          {
                new MailLogAddress("ricardo@resolution.com.br", "Ricardo teste mensageria")
          } :
          new List<MailLogAddress>()
          {
                new MailLogAddress(item.Manager.Mail, item.Manager.Name)
          },
        Priority = EnumPriorityMail.Low,
        _idPerson = item.Manager._id,
        NamePerson = item.Manager.Name,
        Body = body,
        StatusMail = EnumStatusMail.Sended,
        Included = DateTime.Now,
        Subject = model.Subject
      };
      if (personManager != null)
      {
        sendMail = serviceMailLog.InsertNewVersion(sendMail).Result;
        string token = SendMailApi(path, personManager, sendMail._id);
      }
      else
      {
        sendMail.StatusMail = EnumStatusMail.Error;
        sendMail.MessageError = "Manager null in onboarding manager messages.";
        sendMail = serviceMailLog.InsertNewVersion(sendMail).Result;
      }
    }
  }
  catch (Exception e)
  {
    throw e;
  }
}
#endregion

#region Plan Action
private void PlanManagerDeadline(bool sendTest, MailModel model)
{
  try
  {
    var monitoringsant = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End).Result;
    var monitorings = monitoringsant.Select(p => p._id);

    List<PlanWorkNotification> listManager = new List<PlanWorkNotification>();
    // Pessoas ativas e com gestores
    List<Person> persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled).Result;
    foreach (Person person in persons)
    {

      if (person._id == "5c807bc45e4ccea2266f256a")
      {
        var x = "";
      }
      List<Plan> plans = servicePlan.GetAllNewVersion(p => p.Person._id == person._id).Result.Where(p => monitorings.Contains(p._idMonitoring)).ToList();

      var plansfilter = plans.Where(p => p.StatusPlan == EnumStatusPlan.Open).ToList();
      foreach (Plan plan in plansfilter)
      {
        PlanWorkNotification work = PlanManagerDeadline(person, plan, sendTest);
        if (work != null)
          listManager.Add(work);
      }

      plansfilter = plans.Where(p => p.Status == EnumStatus.Enabled).ToList();
      foreach (Plan plan in plansfilter)
      {

        if (plan.StatusPlanApproved == EnumStatusPlanApproved.Wait)
        {
          listManager.Add(new PlanWorkNotification()
          {
            Manager = person.Manager,
            Person = person,
            Plan = plan,
            Type = ManagerListType.Wait
          });
        }
      }

    }

    if (listManager.Count > 0)
    {
      // Emitir e-mails
      List<PlanManagerNotification> listManagerNotification = new List<PlanManagerNotification>();
      listManager = listManager.Where(p => p.Manager != null).OrderBy(o => o.Type).OrderBy(o => o.Plan.Deadline).OrderBy(o => o.Person.User.Name).OrderBy(o => o.Manager.Name).ToList();
      PlanManagerNotification managerNotification = new PlanManagerNotification()
      {
        Manager = listManager[0].Manager,
        Defeated = new List<PlanWorkPerson>(),
        DefeatedNow = new List<PlanWorkPerson>(),
        LastSevenDays = new List<PlanWorkPerson>(),
        FifteenDays = new List<PlanWorkPerson>(),
        ThirtyDays = new List<PlanWorkPerson>(),
        Wait = new List<PlanWorkPerson>(),
      };
      foreach (PlanWorkNotification item in listManager)
      {
        if (managerNotification.Manager.Name != item.Manager.Name)
        {
          managerNotification.Defeated = managerNotification.Defeated.OrderBy(o => o.Person.User.Name).ToList();
          managerNotification.DefeatedNow = managerNotification.DefeatedNow.OrderBy(o => o.Person.User.Name).ToList();
          managerNotification.LastSevenDays = managerNotification.LastSevenDays.OrderBy(o => o.Person.User.Name).ToList();
          managerNotification.FifteenDays = managerNotification.FifteenDays.OrderBy(o => o.Person.User.Name).ToList();
          managerNotification.ThirtyDays = managerNotification.ThirtyDays.OrderBy(o => o.Person.User.Name).ToList();
          listManagerNotification.Add(managerNotification);
          managerNotification = new PlanManagerNotification()
          {
            Manager = item.Manager,
            Defeated = new List<PlanWorkPerson>(),
            DefeatedNow = new List<PlanWorkPerson>(),
            LastSevenDays = new List<PlanWorkPerson>(),
            FifteenDays = new List<PlanWorkPerson>(),
            ThirtyDays = new List<PlanWorkPerson>(),
            Wait = new List<PlanWorkPerson>()
          };
        }
        switch (item.Type)
        {
          case ManagerListType.Defeated:
            managerNotification.Defeated.Add(new PlanWorkPerson() { Person = item.Person, Plan = item.Plan });
            break;
          case ManagerListType.DefeatedNow:
            managerNotification.DefeatedNow.Add(new PlanWorkPerson() { Person = item.Person, Plan = item.Plan });
            break;
          case ManagerListType.LastSevenDays:
            managerNotification.LastSevenDays.Add(new PlanWorkPerson() { Person = item.Person, Plan = item.Plan });
            break;
          case ManagerListType.FifteenDays:
            managerNotification.FifteenDays.Add(new PlanWorkPerson() { Person = item.Person, Plan = item.Plan });
            break;
          case ManagerListType.ThirtyDays:
            managerNotification.ThirtyDays.Add(new PlanWorkPerson() { Person = item.Person, Plan = item.Plan });
            break;
          case ManagerListType.Wait:
            managerNotification.Wait.Add(new PlanWorkPerson() { Person = item.Person, Plan = item.Plan });
            break;
          default:
            break;
        }
      }
      managerNotification.Defeated = managerNotification.Defeated.OrderBy(o => o.Person.User.Name).ToList();
      managerNotification.DefeatedNow = managerNotification.DefeatedNow.OrderBy(o => o.Person.User.Name).ToList();
      managerNotification.LastSevenDays = managerNotification.LastSevenDays.OrderBy(o => o.Person.User.Name).ToList();
      managerNotification.FifteenDays = managerNotification.FifteenDays.OrderBy(o => o.Person.User.Name).ToList();
      managerNotification.ThirtyDays = managerNotification.ThirtyDays.OrderBy(o => o.Person.User.Name).ToList();
      managerNotification.Wait = managerNotification.Wait.OrderBy(o => o.Person.User.Name).ToList();
      listManagerNotification.Add(managerNotification);
      MailPlanManagerDeadline(listManagerNotification, sendTest, model);
      // Enviar para o colaborador
      List<PlanNotification> listNotification = new List<PlanNotification>();
      listManager = listManager.Where(p => p.Manager != null).OrderBy(o => o.Type).OrderBy(o => o.Plan.Deadline).OrderBy(o => o.Person.User.Name).OrderBy(o => o.Manager.Name).ToList();
      var notification = new PlanNotification()
      {
        Person = listManager[0].Person,
        Defeated = new List<Plan>(),
        DefeatedNow = new List<Plan>(),
        LastSevenDays = new List<Plan>(),
        FifteenDays = new List<Plan>(),
        ThirtyDays = new List<Plan>(),
        Wait = new List<Plan>()
      };
      foreach (PlanWorkNotification item in listManager)
      {
        if (managerNotification.Manager.Name != item.Manager.Name)
        {
          notification.Defeated = notification.Defeated.OrderBy(o => o.Deadline).ToList();
          notification.DefeatedNow = notification.DefeatedNow.OrderBy(o => o.Deadline).ToList();
          notification.LastSevenDays = notification.LastSevenDays.OrderBy(o => o.Deadline).ToList();
          notification.FifteenDays = notification.FifteenDays.OrderBy(o => o.Deadline).ToList();
          notification.ThirtyDays = notification.ThirtyDays.OrderBy(o => o.Deadline).ToList();
          notification.Wait = notification.Wait.OrderBy(o => o.Deadline).ToList();
          listNotification.Add(notification);
          notification = new PlanNotification()
          {
            Person = listManager[0].Person,
            Defeated = new List<Plan>(),
            DefeatedNow = new List<Plan>(),
            LastSevenDays = new List<Plan>(),
            FifteenDays = new List<Plan>(),
            ThirtyDays = new List<Plan>(),
            Wait = new List<Plan>()
          };
        }
        switch (item.Type)
        {
          case ManagerListType.Defeated:
            notification.Defeated.Add(item.Plan);
            break;
          case ManagerListType.DefeatedNow:
            notification.DefeatedNow.Add(item.Plan);
            break;
          case ManagerListType.LastSevenDays:
            notification.LastSevenDays.Add(item.Plan);
            break;
          case ManagerListType.FifteenDays:
            notification.FifteenDays.Add(item.Plan);
            break;
          case ManagerListType.ThirtyDays:
            notification.ThirtyDays.Add(item.Plan);
            break;
          case ManagerListType.Wait:
            notification.Wait.Add(item.Plan);
            break;
          default:
            break;
        }
      }
      notification.Defeated = notification.Defeated.OrderBy(o => o.Deadline).ToList();
      notification.DefeatedNow = notification.DefeatedNow.OrderBy(o => o.Deadline).ToList();
      notification.LastSevenDays = notification.LastSevenDays.OrderBy(o => o.Deadline).ToList();
      notification.FifteenDays = notification.FifteenDays.OrderBy(o => o.Deadline).ToList();
      notification.ThirtyDays = notification.ThirtyDays.OrderBy(o => o.Deadline).ToList();
      notification.Wait = notification.Wait.OrderBy(o => o.Deadline).ToList();
      listNotification.Add(notification);
      MailPlanDeadline(listNotification, sendTest);
    }
  }
  catch (Exception)
  {

  }
}
private PlanWorkNotification PlanManagerDeadline(Person person, Plan plan, bool sendTest)
{
  PlanWorkNotification result = new PlanWorkNotification()
  {
    Manager = person.Manager,
    Person = person,
    Plan = plan,
  };

  var deadline = (DateTime)DateTime.Parse(plan.Deadline.Value.ToString("dd/MM/yyyy"));
  var now = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
  int days = (deadline - now).Days;

  if (days < 0)
  {
    // Vencido
    result.Type = ManagerListType.Defeated;
  }
  if (days == 0)
  {
    // Vence hoje
    result.Type = ManagerListType.DefeatedNow;
  }
  //if (days <= 7 && days >= 1)
  //{
  //  // vence em 7 dias
  //  result.Type = ManagerListType.LastSevenDays;
  //}
  if (days == 10)
  {
    // Vence em 10 dias
    result.Type = ManagerListType.FifteenDays;
  }
  if (days == 30)
  {
    // Vence em 30 dias
    result.Type = ManagerListType.ThirtyDays;
  }
  return result;
}
private void MailPlanManagerDeadline(List<PlanManagerNotification> listManager, bool sendTest, MailModel model)
{
  try
  {
    Person personManager = servicePerson.GetNewVersion(p => p._id == listManager[0].Manager._id).Result;

    //searsh model mail database
    if (model.StatusMail == EnumStatus.Disabled)
      return;

    foreach (var item in listManager)
    {
      string body = model.Message.Replace("{Link}", model.Link)
                                  .Replace("{Manager}", item.Manager.Name);

      string list = string.Empty;
      string saveName = string.Empty;
      foreach (var personPlan in item.Defeated)
      {
        if ((DateTime)personPlan.Plan.Deadline < DateTime.Now)
        {
          if (saveName != personPlan.Person.User.Name)
            list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", personPlan.Person.User.Name, ((DateTime)personPlan.Plan.Deadline).ToString("dd/MM/yyyy"), personPlan.Plan.Description));
          else
            list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", string.Empty, ((DateTime)personPlan.Plan.Deadline).ToString("dd/MM/yyyy"), personPlan.Plan.Description));
        }
      }



      if (!string.IsNullOrEmpty(list))
        list = string.Concat("Colaboradores com ação de desenvolvimento <strong>vencida</strong>:<br><table>", list, "</table><br>");

      body = body.Replace("{LIST1}", list);


      list = string.Empty;
      saveName = string.Empty;
      foreach (var personPlan in item.Wait)
      {
        if (saveName != personPlan.Person.User.Name)
          list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", personPlan.Person.User.Name, ((DateTime)personPlan.Plan.Deadline).ToString("dd/MM/yyyy"), personPlan.Plan.Description));
        else
          list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", string.Empty, ((DateTime)personPlan.Plan.Deadline).ToString("dd/MM/yyyy"), personPlan.Plan.Description));

      }

      if (!string.IsNullOrEmpty(list))
        list = string.Concat("Colaboradores com ações de desenvolvimento <strong>faltando apenas sua validação</strong>:<br><table>", list, "</table><br>");

      body = body.Replace("{LIST0}", list);

      list = string.Empty;
      saveName = string.Empty;
      foreach (var personPlan in item.DefeatedNow)
        if (saveName != personPlan.Person.User.Name)
          list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", personPlan.Person.User.Name, ((DateTime)personPlan.Plan.Deadline).ToString("dd/MM/yyyy"), personPlan.Plan.Description));
        else
          list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", string.Empty, ((DateTime)personPlan.Plan.Deadline).ToString("dd/MM/yyyy"), personPlan.Plan.Description));

      if (!string.IsNullOrEmpty(list))
        list = string.Concat("Colaboradores com ação de desenvolvimento que <strong>vence hoje</strong>:<br><table>", list, "</table><br>");

      body = body.Replace("{LIST2}", list);

      list = string.Empty;
      saveName = string.Empty;
      foreach (var personPlan in item.LastSevenDays)
        if (saveName != personPlan.Person.User.Name)
          list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", personPlan.Person.User.Name, ((DateTime)personPlan.Plan.Deadline).ToString("dd/MM/yyyy"), personPlan.Plan.Description));
        else
          list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", string.Empty, ((DateTime)personPlan.Plan.Deadline).ToString("dd/MM/yyyy"), personPlan.Plan.Description));

      if (!string.IsNullOrEmpty(list))
        list = string.Concat("Colaboradores com ação de desenvolvimento que <strong>vence em 7 dias</strong>:<br><table>", list, "</table><br>");

      body = body.Replace("{LIST3}", list);

      list = string.Empty;
      saveName = string.Empty;
      foreach (var personPlan in item.FifteenDays)
        if (saveName != personPlan.Person.User.Name)
          list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", personPlan.Person.User.Name, ((DateTime)personPlan.Plan.Deadline).ToString("dd/MM/yyyy"), personPlan.Plan.Description));
        else
          list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", string.Empty, ((DateTime)personPlan.Plan.Deadline).ToString("dd/MM/yyyy"), personPlan.Plan.Description));

      if (!string.IsNullOrEmpty(list))
        list = string.Concat("Colaboradores com ação de desenvolvimento que <strong>vence em 10 dias</strong>:<br><table>", list, "</table><br>");

      body = body.Replace("{LIST4}", list);

      list = string.Empty;
      saveName = string.Empty;
      foreach (var personPlan in item.ThirtyDays)
        if (saveName != personPlan.Person.User.Name)
          list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", personPlan.Person.User.Name, ((DateTime)personPlan.Plan.Deadline).ToString("dd/MM/yyyy"), personPlan.Plan.Description));
        else
          list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", string.Empty, ((DateTime)personPlan.Plan.Deadline).ToString("dd/MM/yyyy"), personPlan.Plan.Description));

      if (!string.IsNullOrEmpty(list))
        list = string.Concat("Colaboradores com ação de desenvolvimento que <strong>vence em 30 dias</strong>:<br><table>", list, "</table><br>");

      body = body.Replace("{LIST5}", list);

      MailLog sendMail = new MailLog
      {
        From = new MailLogAddress("suporte@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
        To = sendTest ?
          new List<MailLogAddress>()
          {
                new MailLogAddress("ricardo@resolution.com.br", "Ricardo teste mensageria")
          } :
          new List<MailLogAddress>()
          {
                new MailLogAddress(item.Manager.Mail, item.Manager.Name)
          },
        Priority = EnumPriorityMail.Low,
        _idPerson = item.Manager._id,
        NamePerson = item.Manager.Name,
        Body = body,
        StatusMail = EnumStatusMail.Sended,
        Included = DateTime.Now,
        Subject = model.Subject
      };
      //MailLog mailObj = serviceMailLog.InsertNewVersion(sendMail).Result;
      MailLog mailObj = null;
      if (personManager != null)
      {
        sendMail = serviceMailLog.InsertNewVersion(sendMail).Result;
        mailObj = sendMail;
        string token = SendMailApi(path, personManager, sendMail._id);
      }
      else
      {
        sendMail.StatusMail = EnumStatusMail.Error;
        sendMail.MessageError = "Manager null in plan action manager messages.";
        sendMail = serviceMailLog.InsertNewVersion(sendMail).Result;
      }
    }
  }
  catch (Exception e)
  {
    throw e;
  }
}
private void MailPlanDeadline(List<PlanNotification> listPlan, bool sendTest)
{
  try
  {
    //searsh model mail database
    MailModel model = serviceMailModel.PlanDeadline(path);
    if (model.StatusMail == EnumStatus.Disabled)
      return;
    foreach (var item in listPlan)
    {
      string body = model.Message.Replace("{Link}", model.Link)
                                  .Replace("{Person}", item.Person.User.Name);

      string list = string.Empty;
      string saveName = string.Empty;
      foreach (var personPlan in item.Defeated)
        list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td></tr>", ((DateTime)personPlan.Deadline).ToString("dd/MM/yyyy"), personPlan.Description));

      if (!string.IsNullOrEmpty(list))
        list = string.Concat("Ação de desenvolvimento <strong>vencida</strong>:<br><table>", list, "</table><br>");

      body = body.Replace("{LIST1}", list);

      list = string.Empty;
      saveName = string.Empty;
      foreach (var personPlan in item.DefeatedNow)
        list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td></tr>", ((DateTime)personPlan.Deadline).ToString("dd/MM/yyyy"), personPlan.Description));

      if (!string.IsNullOrEmpty(list))
        list = string.Concat("Ação de desenvolvimento que <strong>vence hoje</strong>:<br><table>", list, "</table><br>");

      body = body.Replace("{LIST2}", list);

      list = string.Empty;
      saveName = string.Empty;
      foreach (var personPlan in item.LastSevenDays)
        list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td></tr>", ((DateTime)personPlan.Deadline).ToString("dd/MM/yyyy"), personPlan.Description));

      if (!string.IsNullOrEmpty(list))
        list = string.Concat("Ação de desenvolvimento que <strong>vence em 7 dias</strong>:<br><table>", list, "</table><br>");

      body = body.Replace("{LIST3}", list);

      list = string.Empty;
      saveName = string.Empty;
      foreach (var personPlan in item.FifteenDays)
        list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td></tr>", ((DateTime)personPlan.Deadline).ToString("dd/MM/yyyy"), personPlan.Description));

      if (!string.IsNullOrEmpty(list))
        list = string.Concat("Ação de desenvolvimento que <strong>vence em 10 dias</strong>:<br><table>", list, "</table><br>");

      body = body.Replace("{LIST4}", list);

      list = string.Empty;
      saveName = string.Empty;
      foreach (var personPlan in item.ThirtyDays)
        list = string.Concat(list, string.Format("<tr><td>{0}</td><td>{1}</td></tr>", ((DateTime)personPlan.Deadline).ToString("dd/MM/yyyy"), personPlan.Description));

      if (!string.IsNullOrEmpty(list))
        list = string.Concat("Ação de desenvolvimento que <strong>vence em 30 dias</strong>:<br><table>", list, "</table><br>");

      body = body.Replace("{LIST5}", list);

      MailLog sendMail = new MailLog
      {
        From = new MailLogAddress("suporte@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
        To = sendTest ?
          new List<MailLogAddress>()
          {
                new MailLogAddress("ricardo@resolution.com.br", "Ricardo teste mensageria")
          } :
          new List<MailLogAddress>()
          {
                new MailLogAddress(item.Person.User.Mail, item.Person.User.Name)
          },
        Priority = EnumPriorityMail.Low,
        _idPerson = item.Person._id,
        NamePerson = item.Person.User.Name,
        Body = body,
        StatusMail = EnumStatusMail.Sended,
        Included = DateTime.Now,
        Subject = model.Subject
      };
      sendMail = serviceMailLog.InsertNewVersion(sendMail).Result;
      string token = SendMailApi(path, item.Person, sendMail._id);
    }
  }
  catch (Exception e)
  {
    throw e;
  }
}

private void BirthCompany(Person person, bool sendTest)
{
  try
  {
    //searsh model mail database
    MailModel model = serviceMailModel.BirthCompany(path);
    if (model.StatusMail == EnumStatus.Disabled)
      return;

    string body = model.Message.Replace("{Link}", model.Link)
                                .Replace("{Person}", person.User.Name);

    MailLog sendMail = new MailLog
    {
      From = new MailLogAddress("suporte@fluidstate.com.br", "Sucesso do Cliente | Fluid"),
      To = sendTest ?
        new List<MailLogAddress>()
        {
                new MailLogAddress("ricardo@resolution.com.br", "Ricardo teste mensageria")
        } :
        new List<MailLogAddress>()
        {
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
    sendMail = serviceMailLog.InsertNewVersion(sendMail).Result;
    string token = SendMailApi(path, person, sendMail._id);

  }
  catch (Exception e)
  {
    throw e;
  }
}
#endregion

#region Sucess Factory
private void BirthCompany(bool sendTest)
{
  try
  {
    var date = DateTime.Now;

    List<Person> persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled
    && p.User.DateAdm != null).Result;

    foreach (Person person in persons)
    {
      if (person.User != null)
        if (person.User.DateAdm != null)
          if (person.User?.DateAdm?.Day == date.Day && person.User?.DateAdm?.Month == date.Month)
            BirthCompany(person, sendTest);


    }
  }
  catch (Exception e)
  {
    var message = e;
  }
}

#endregion

#region SendMail Api
private string SendMailApi(string link, Person person, string idmail)
{
  try
  {
    string token = serviceAuthentication.AuthenticationMail(person);
    using (HttpClient client = new HttpClient())
    {
      client.BaseAddress = new Uri(link.Substring(0, link.Length - 1) + ":5201/");
      client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
      HttpResponseMessage resultMail = client.PostAsync(string.Format("sendmail/{0}", idmail), null).Result;
      return "Ok!";
    }
  }
  catch (Exception e)
  {
    throw e;
  }
}
#endregion



#region Onboarding CS
public List<string> EmployeeWaiting(EnumActionNotification action)
{
  throw new Exception("Não implementado");
}
public List<string> ManagerWaiting(EnumActionNotification action)
{
  throw new Exception("Não implementado");
}
    #endregion

  }
}
