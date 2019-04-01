﻿using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Manager.Core.Base;
using Manager.Services.Auth;
using Manager.Views.Enumns;
using Manager.Views.BusinessView;

namespace Manager.Services.Specific
{
  public class ServiceAutoManager : Repository<AutoManager>, IServiceAutoManager
  {

    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceGeneric<AutoManager> serviceAutoManager;
    private readonly ServiceGeneric<MailLog> serviceMailLog;
    private readonly ServiceGeneric<MailMessage> serviceMailMessage;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceWorkflow serviceWorkflow;

    #region Constructor
    public ServiceAutoManager(DataContext context) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context);
        serviceAutoManager = new ServiceGeneric<AutoManager>(context);
        serviceMailLog = new ServiceGeneric<MailLog>(context);
        serviceMailModel = new ServiceMailModel(context);
        serviceMailMessage = new ServiceGeneric<MailMessage>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceWorkflow = new ServiceWorkflow(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceAutoManager._user = _user;
      serviceMailLog._user = _user;
      serviceMailMessage._user = _user;
      serviceMailModel.SetUser(_user);
      servicePerson._user = _user;
      serviceWorkflow.SetUser(_user);
    }
    public void SetUser(BaseUser user)
    {
      serviceAutoManager._user = user;
      serviceMailLog._user = user;
      serviceMailMessage._user = user;
      serviceMailModel.SetUser(user);
      servicePerson._user = user;
      serviceWorkflow.SetUser(user);
    }
    #endregion

    #region AutoManager
    public List<ViewAutoManagerPerson> List(string idManager, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        if (filter != string.Empty)
        {
          var listEnd = ListEnd(idManager, filter);
          total = listEnd.Count();
          return listEnd.Skip(skip).Take(count).ToList();
        }
        else
          return ListOpen(idManager, ref total, count, page, filter);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewAutoManagerPerson> ListOpen(string idManager, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var list = (from person in servicePerson.GetAll()
                    where person.TypeUser != EnumTypeUser.Support && person.TypeUser != EnumTypeUser.Administrator && person.StatusUser != EnumStatusUser.Disabled && person.Manager == null && person.StatusUser != EnumStatusUser.Disabled && person._id != idManager
                    select person).ToList().Select(person => new ViewAutoManagerPerson { IdPerson = person._id, NamePerson = person.User.Name, Status = EnumStatusAutoManagerView.Open }).Skip(skip).Take(count).ToList();

        total = (from person in servicePerson.GetAll()
                 where person.TypeUser != EnumTypeUser.Support && person.TypeUser != EnumTypeUser.Administrator && person.StatusUser != EnumStatusUser.Disabled && person.Manager == null && person.StatusUser != EnumStatusUser.Disabled && person._id != idManager
                 select person).ToList().Select(person => new ViewAutoManagerPerson { IdPerson = person._id, NamePerson = person.User.Name, Status = EnumStatusAutoManagerView.Open }).Count();

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewAutoManagerPerson> ListEnd(string idManager, string filter)
    {
      try
      {
        var result = new List<ViewAutoManagerPerson>();
        foreach (var item in servicePerson.GetAll(p => p.TypeUser != EnumTypeUser.Support && p.TypeUser != EnumTypeUser.Administrator && p.StatusUser != EnumStatusUser.Disabled && p.StatusUser != EnumStatusUser.ErrorIntegration && p.User.Name.ToUpper().Contains(filter.ToUpper()) && p.StatusUser != EnumStatusUser.Disabled && p.StatusUser != EnumStatusUser.ErrorIntegration && p._id != idManager && p.Manager._id != idManager).ToList())
        {
          var view = new ViewAutoManagerPerson();
          var exists = serviceAutoManager.GetAll(p => p.Person._id == item._id && p.Requestor._id == idManager && p.StatusAutoManager == EnumStatusAutoManager.Requested).Count();
          var existsManager = servicePerson.GetAll(p => p._id == item._id && p.Manager._id != null).Count();
          view.IdPerson = item._id;
          view.NamePerson = item.User.Name;
          if (exists > 0)
            view.Status = EnumStatusAutoManagerView.Requestor;
          else if (existsManager > 0)
            view.Status = EnumStatusAutoManagerView.Manager;
          else
            view.Status = EnumStatusAutoManagerView.Open;
          result.Add(view);
        }
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetManagerPerson(ViewManager view, string idPerson, string path)
    {
      try
      {
        var person = servicePerson.GetAll(p => p._id == idPerson).FirstOrDefault();
        var manager = servicePerson.GetAll(p => p._id == view.IdManager).FirstOrDefault();
        if (view.Status == EnumStatusAutoManagerView.Open)
        {
          person.Manager = new BaseFields() { _id = manager._id, Mail = manager.User.Mail, Name = manager.User.Name };
          var exists = servicePerson.GetAll(p => p.Manager._id == view.IdManager).Count();
          if (exists == 0 & manager.TypeUser == EnumTypeUser.Employee)
          {
            manager.TypeUser = EnumTypeUser.Manager;
            servicePerson.Update(manager, null);
          }
          servicePerson.Update(person, null);
        }
        else
        {
          var viewFlow = new ViewFlow
          {
            IdPerson = idPerson,
            Type = EnumTypeFlow.Manager
          };
          var auto = new AutoManager
          {
            Person = person,
            Requestor = manager,
            StatusAutoManager = EnumStatusAutoManager.Requested,
            Status = EnumStatus.Enabled,
            OpenDate = DateTime.Now,
            Workflow = serviceWorkflow.NewFlow(viewFlow)
          };
          serviceAutoManager.Insert(auto);
          //searsh model mail database
          var model = serviceMailModel.AutoManager(path);
          if (model.StatusMail == EnumStatus.Disabled)
            return;

          var url = path + "evaluationconfiguration/automanager/" + person._id.ToString() + "/approved/" + manager._id.ToString();
          var message = new MailMessage
          {
            Type = EnumTypeMailMessage.Put,
            Name = model.Name,
            Url = url,
            Body = " { '._idWorkFlow': '" + auto.Workflow.FirstOrDefault()._id.ToString() + "' } "
          };
          var idMessageApv = serviceMailMessage.Insert(message)._id;
          var requestor = servicePerson.GetAll(p => p._id == auto.Workflow.FirstOrDefault().Requestor._id).FirstOrDefault();
          var body = model.Message.Replace("{Person}", auto.Workflow.FirstOrDefault().Requestor.User.Name);
          body = body.Replace("{Requestor}", auto.Requestor.User.Name);
          body = body.Replace("{Employee}", person.User.Name);
          // approved link
          body = body.Replace("{Approved}", model.Link + "/" + idMessageApv.ToString());
          url = path + "evaluationconfiguration/automanager/" + person._id.ToString() + "/disapproved/" + manager._id.ToString();
          //disapproved link
          message.Url = url;
          var idMessageDis = serviceMailMessage.Insert(message)._id;
          body = body.Replace("{Disapproved}", model.Link + "/" + idMessageDis.ToString());
          var sendMail = new MailLog
          {
            From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
            To = new List<MailLogAddress>(){
                  new MailLogAddress(requestor.User.Mail, requestor.User.Name)
              },
            Priority = EnumPriorityMail.Low,
            _idPerson = person._id,
            NamePerson = person.User.Name,
            Body = body,
            StatusMail = EnumStatusMail.Sended,
            Included = DateTime.Now,
            Subject = model.Subject,
          };
          serviceMailLog.Insert(sendMail);
          var messageApv = serviceMailMessage.GetAll(p => p._id == idMessageApv).FirstOrDefault();
          var messageDis = serviceMailMessage.GetAll(p => p._id == idMessageDis).FirstOrDefault();
          var token = SendMail(path, person, sendMail._id.ToString());
          messageApv.Token = token;
          serviceMailMessage.Update(messageApv, null);
          messageDis.Token = token;
          serviceMailMessage.Update(messageDis, null);
        };
      }
      catch (Exception e)
      {
        throw e;
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
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + view.Token);
          var resultMail = client.PostAsync("mail/sendmail/" + idmail, null).Result;
          return view.Token;
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void Disapproved(ViewWorkflow view, string idPerson, string idManager)
    {
      try
      {
        var auto = serviceAutoManager.GetAll(p => p.Person._id == idPerson & p.Requestor._id == idManager & p.StatusAutoManager == EnumStatusAutoManager.Requested).FirstOrDefault();
        var list = new List<Workflow>();
        foreach (var item in auto.Workflow)
          list.Add(serviceWorkflow.Disapproved(view));
        auto.Workflow = list;
        auto.StatusAutoManager = EnumStatusAutoManager.Disapproved;
        serviceAutoManager.Update(auto, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void Approved(ViewWorkflow view, string idPerson, string idManager)
    {
      try
      {
        var auto = serviceAutoManager.GetAll(p => p.Person._id == idPerson & p.Requestor._id == idManager & p.StatusAutoManager == EnumStatusAutoManager.Requested).FirstOrDefault();
        var list = new List<Workflow>();
        foreach (var item in auto.Workflow)
          list.Add(serviceWorkflow.Approved(view));
        auto.Workflow = list;
        auto.StatusAutoManager = EnumStatusAutoManager.Approved;
        var manager = servicePerson.GetAll(p => p._id == idManager).FirstOrDefault();
        var person = servicePerson.GetAll(p => p._id == idPerson).FirstOrDefault();
        person.Manager = new BaseFields() { _id = manager._id, Mail = manager.User.Mail, Name = manager.User.Name };
        servicePerson.Update(person, null);
        if (manager.TypeUser == EnumTypeUser.Employee)
        {
          manager.TypeUser = EnumTypeUser.Manager;
          servicePerson.Update(manager, null);
        }
        serviceAutoManager.Update(auto, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void Canceled(string idPerson, string idManager)
    {
      try
      {
        var auto = serviceAutoManager.GetAll(p => p.Person._id == idPerson & p.Requestor._id == idManager & p.StatusAutoManager == EnumStatusAutoManager.Requested).FirstOrDefault();
        auto.StatusAutoManager = EnumStatusAutoManager.Canceled;
        serviceAutoManager.Update(auto, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewAutoManager> ListApproved(string idManager)
    {
      try
      {
        return (from auto in serviceAutoManager.GetAll()
                select auto
                    ).ToList()
                    .Where(p => p.Workflow.Where(t => t.StatusWorkflow == EnumWorkflow.Open
                    & t.Requestor._id == idManager).Count() > 0)
                    .Select(auto => new ViewAutoManager()
                    {
                      IdWorkflow = auto.Workflow.FirstOrDefault()._id,
                      IdRequestor = auto.Requestor._id,
                      NameRequestor = auto.Requestor.User.Name,
                      IdPerson = auto.Person._id.ToString(),
                      NamePerson = auto.Person.User.Name
                    }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void DeleteManager(string idPerson)
    {
      try
      {
        var person = servicePerson.GetAll(p => p._id == idPerson).FirstOrDefault();
        person.Manager = null;
        servicePerson.Update(person, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

  }
}
