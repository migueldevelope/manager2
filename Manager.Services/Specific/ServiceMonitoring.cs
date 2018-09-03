﻿using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Manager.Services.Specific
{
  public class ServiceMonitoring : Repository<Monitoring>, IServiceMonitoring
  {
    private readonly ServiceGeneric<Monitoring> monitoringService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<Plan> planService;
    private readonly ServiceLog logService;
    private readonly ServiceMailModel mailModelService;
    private readonly ServiceGeneric<MailMessage> mailMessageService;
    private readonly ServiceGeneric<MailLog> mailService;
    public string path;

    public ServiceMonitoring(DataContext context, string pathToken)
      : base(context)
    {
      try
      {
        monitoringService = new ServiceGeneric<Monitoring>(context);
        personService = new ServiceGeneric<Person>(context);
        planService = new ServiceGeneric<Plan>(context);
        logService = new ServiceLog(_context);
        mailModelService = new ServiceMailModel(context);
        mailMessageService = new ServiceGeneric<MailMessage>(context);
        mailService = new ServiceGeneric<MailLog>(context);
        path = pathToken;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Monitoring> ListMonitoringsEnd(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(idmanager, "ListEnd");
        int skip = (count * (page - 1));
        var detail = monitoringService.GetAll(p => p.Person.Manager._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private void newOnZero()
    {
      try
      {
        var on = monitoringService.GetAuthentication(p => p.Status == EnumStatus.Disabled).Count();
        if (on == 0)
        {
          var person = personService.GetAll().FirstOrDefault();
          var zero = monitoringService.Insert(new Monitoring() { Person = person, Status = EnumStatus.Disabled, StatusMonitoring = EnumStatusMonitoring.End });
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public List<Monitoring> ListMonitoringsWait(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(idmanager, "List");
        newOnZero();
        int skip = (count * (page - 1));
        var list = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.Monitoring & p.Manager._id == idmanager
        & p.Name.ToUpper().Contains(filter.ToUpper()))
        .ToList().Select(p => new { Person = p, Monitoring = monitoringService.GetAll(x => x.StatusMonitoring != EnumStatusMonitoring.End & x.Person._id == p._id).FirstOrDefault() })
        .ToList();

        var detail = new List<Monitoring>();
        foreach (var item in list)
        {
          if (item.Monitoring == null)
            detail.Add(new Monitoring
            {
              Person = item.Person,
              _id = null,
              StatusMonitoring = EnumStatusMonitoring.Open
            });
          else
            if (item.Monitoring.StatusMonitoring != EnumStatusMonitoring.End)
            detail.Add(item.Monitoring);
        }

        total = detail.Count();
        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Monitoring> PersonMonitoringsEnd(string idmanager)
    {
      try
      {
        LogSave(idmanager, "PersonEnd");
        return monitoringService.GetAll(p => p.Person._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Monitoring PersonMonitoringsWait(string idmanager)
    {
      try
      {
        LogSave(idmanager, "PersonWait");
        var item = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.Monitoring & p._id == idmanager)
        .ToList().Select(p => new { Person = p, Monitoring = monitoringService.GetAll(x => x.StatusMonitoring != EnumStatusMonitoring.End & x.Person._id == p._id).FirstOrDefault() })
        .FirstOrDefault();

        if (item == null)
          return null;

        if (item.Monitoring == null)
          return new Monitoring
          {
            Person = item.Person,
            _id = null,
            StatusMonitoring = EnumStatusMonitoring.Open
          };
        else
         if (item.Monitoring.StatusMonitoring != EnumStatusMonitoring.End)
          return item.Monitoring;
        else
          return null;


      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Monitoring GetMonitorings(string id)
    {
      try
      {
        return monitoringService.GetAll(p => p._id == id).ToList().Select(p => new Monitoring()
        {
          _id = p._id,
          _idAccount = p._idAccount,
          Status = p.Status,
          Person = p.Person,
          DateBeginPerson = p.DateBeginPerson,
          DateBeginManager = p.DateBeginManager,
          DateBeginEnd = p.DateBeginEnd,
          DateEndPerson = p.DateEndPerson,
          DateEndManager = p.DateEndManager,
          DateEndEnd = p.DateEndEnd,
          CommentsPerson = p.CommentsPerson,
          CommentsManager = p.CommentsManager,
          CommentsEnd = p.CommentsEnd,
          SkillsCompany = p.SkillsCompany.OrderBy(x => x.Skill.Name).ToList(),
          Schoolings = p.Schoolings.OrderBy(x => x.Schooling.Order).ToList(),
          Activities = p.Activities.OrderBy(x => x.Activities.Order).ToList(),
          StatusMonitoring = p.StatusMonitoring
        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private Monitoring loadMap(Monitoring monitoring)
    {
      try
      {
        monitoring.SkillsCompany = new List<MonitoringSkills>();
        foreach (var item in monitoring.Person.Company.Skills)
        {
          monitoring.SkillsCompany.Add(new MonitoringSkills() { Skill = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<Plan>() });
        }

        monitoring.Activities = new List<MonitoringActivities>();
        foreach (var item in monitoring.Person.Occupation.Activities)
        {
          monitoring.Activities.Add(new MonitoringActivities() { Activities = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<Plan>() });
        }

        monitoring.Schoolings = new List<MonitoringSchooling>();
        foreach (var item in monitoring.Person.Occupation.Schooling)
        {
          monitoring.Schoolings.Add(new MonitoringSchooling() { Schooling = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<Plan>() });
        }

        return monitoring;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Monitoring NewMonitoring(Monitoring monitoring, string idperson)
    {
      try
      {
        LogSave(monitoring.Person._id, "Monitoring Process");
        if (monitoring._id == null)
        {
          loadMap(monitoring);

          if (monitoring.Person._id == idperson)
          {
            monitoring.DateBeginPerson = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressPerson;
          }
          else
          {
            monitoring.DateBeginManager = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressManager;
          }

          monitoringService.Insert(monitoring);
        }
        else
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Wait)
          {
            monitoring.DateBeginEnd = DateTime.Now;
          }
          monitoringService.Update(monitoring, null);
        }

        return monitoring;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateMonitoring(Monitoring monitoring, string idperson)
    {
      try
      {
        var userInclude = personService.GetAll(p => p._id == idperson).FirstOrDefault();

        if (monitoring.Person._id != idperson)
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Wait)
          {
            monitoring.DateEndManager = DateTime.Now;
            Mail(monitoring.Person);
          }
        }
        else
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.End)
          {
            monitoring.DateEndEnd = DateTime.Now;
          }
          else if (monitoring.StatusMonitoring == EnumStatusMonitoring.WaitManager)
          {
            monitoring.DateEndPerson = DateTime.Now;
            Mail(monitoring.Person.Manager);
          }

        }

        //verify plan;
        foreach (var item in monitoring.Activities)
        {
          var listActivities = new List<Plan>();
          foreach (var plan in item.Plans)
          {
            if (plan._id == null)
              listActivities.Add(AddPlan(plan, userInclude));
            else
            {
              UpdatePlan(plan);
              listActivities.Add(plan);
            }
          }
          item.Plans = listActivities;
        }

        foreach (var item in monitoring.Schoolings)
        {
          var listSchoolings = new List<Plan>();
          foreach (var plan in item.Plans)
          {
            if (plan._id == null)
              listSchoolings.Add(AddPlan(plan, userInclude));
            else
            {
              UpdatePlan(plan);
              listSchoolings.Add(plan);
            }
          }
          item.Plans = listSchoolings;
        }

        foreach (var item in monitoring.SkillsCompany)
        {
          var listSkillsCompany = new List<Plan>();
          foreach (var plan in item.Plans)
          {
            if (plan._id == null)
              listSkillsCompany.Add(AddPlan(plan, userInclude));
            else
            {
              UpdatePlan(plan);
              listSkillsCompany.Add(plan);
            }
          }
          item.Plans = listSkillsCompany;
        }



        monitoringService.Update(monitoring, null);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private string UpdatePlan(Plan plan)
    {
      try
      {
        planService.Update(plan, null);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private Plan AddPlan(Plan plan, Person person)
    {
      try
      {
        plan.DateInclude = DateTime.Now;
        plan.UserInclude = person;
        return planService.Insert(plan);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      personService._user = _user;
      monitoringService._user = _user;
      planService._user = _user;
      logService._user = _user;
      mailModelService._user = _user;
      mailMessageService._user = _user;
      mailService._user = _user;
    }

    public async void LogSave(string iduser, string local)
    {
      try
      {
        var user = personService.GetAll(p => p._id == iduser).FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "Access Monitoring ",
          Local = local,
          Person = user
        };
        logService.NewLog(log);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Skill> GetSkills(string idperson)
    {
      try
      {
        var skills = personService.GetAll(p => p._id == idperson).FirstOrDefault().Occupation.Group.Skills;
        var skillsgroup = personService.GetAll(p => p._id == idperson).FirstOrDefault().Occupation.Skills;
        var list = skillsgroup;
        foreach (var item in skills)
        {
          list.Add(item);
        }
        return list;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }


    // send mail
    public void Mail(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.MonitoringApproval(path);
        var url = "";
        var body = model.Message.Replace("{Person}", person.Name).Replace("{Link}", model.Link);
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Suporte"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Mail, person.Name)
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

    public string RemoveAllMonitoring(string idperson)
    {
      try
      {

        LogSave(_user._idPerson, "RemoveAllMonitoring:" + idperson);
        var monitorings = monitoringService.GetAll(p => p.Person._id == idperson).ToList();
        foreach (var monitoring in monitorings)
        {
          monitoring.Status = EnumStatus.Disabled;
          monitoringService.Update(monitoring, null);
        }

        return "deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string RemoveMonitoring(string idmonitoring)
    {
      try
      {
        LogSave(_user._idPerson, "RemoveMonitoring:" + idmonitoring);
        var monitoring = monitoringService.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        monitoring.Status = EnumStatus.Disabled;
        monitoringService.Update(monitoring, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string RemoveLastMonitoring(string idperson)
    {
      try
      {
        LogSave(_user._idPerson, "RemoveLastMonitoring:" + idperson);
        var monitoring = monitoringService.GetAll(p => p.Person._id == idperson).LastOrDefault();
        monitoring.Status = EnumStatus.Disabled;
        monitoringService.Update(monitoring, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Monitoring> GetListExclud(ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(_user._idPerson, "ListExclud");
        int skip = (count * (page - 1));
        var detail = monitoringService.GetAll(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
}