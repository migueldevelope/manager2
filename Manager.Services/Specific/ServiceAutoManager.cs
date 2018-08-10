using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Manager.Core.Base;
using Manager.Services.Auth;

namespace Manager.Services.Specific
{
  public class ServiceAutoManager : Repository<AutoManager>, IServiceAutoManager
  {
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<AutoManager> autoManagerService;
    private readonly ServiceWorkflow workflowService;
    private readonly ServiceGeneric<MailLog> mailService;
    private readonly ServiceMailModel mailModelService;
    private readonly ServiceGeneric<MailMessage> mailMessageService;

    public BaseUser user { get => base._user; set => _user = base._user; }

    public ServiceAutoManager(DataContext context, IServicePerson servicePerson)
      : base(context)
    {
      try
      {
        personService = new ServiceGeneric<Person>(context);
        autoManagerService = new ServiceGeneric<AutoManager>(context);
        mailService = new ServiceGeneric<MailLog>(context);
        mailModelService = new ServiceMailModel(context);
        mailMessageService = new ServiceGeneric<MailMessage>(context);
        workflowService = new ServiceWorkflow(context, servicePerson);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewAutoManagerPerson> List(string idManager, string filter)
    {
      try
      {
        if (filter != string.Empty)
          return ListEnd(idManager, filter);
        else
          return ListOpen(idManager);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewAutoManagerPerson> ListOpen(string idManager)
    {
      try
      {
        return (from person in personService.GetAll()
                where person.StatusUser != EnumStatusUser.Disabled & person.Manager == null & person.StatusUser != EnumStatusUser.Disabled & person._id != idManager
                select person).ToList().Select(person => new ViewAutoManagerPerson { IdPerson = person._id, NamePerson = person.Name, Status = EnumStatusAutoManagerView.Open }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewAutoManagerPerson> ListEnd(string idManager, string filter)
    {
      try
      {
        var result = new List<ViewAutoManagerPerson>();

        foreach (var item in personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.Name.ToUpper().Contains(filter.ToUpper()) & p.StatusUser != EnumStatusUser.Disabled & p._id != idManager & p.Manager._id != idManager).ToList())
        {
          var view = new ViewAutoManagerPerson();
          var exists = autoManagerService.GetAll(p => p.Person._id == item._id & p.Requestor._id == idManager & p.StatusAutoManager == EnumStatusAutoManager.Requested).Count();
          var existsManager = personService.GetAll(p => p._id == item._id & p.Manager._id != null).Count();

          view.IdPerson = item._id;
          view.NamePerson = item.Name;
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
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetManagerPerson(ViewManager view, string idPerson, string path)
    {
      try
      {
        var person = personService.GetAll(p => p._id == idPerson).FirstOrDefault();
        var manager = personService.GetAll(p => p._id == view.IdManager).FirstOrDefault();
        if (view.Status == EnumStatusAutoManagerView.Open)
        {
          person.Manager = manager;
          var exists = personService.GetAll(p => p.Manager._id == view.IdManager).Count();
          if (exists == 0 & manager.TypeUser == EnumTypeUser.Employee)
          {
            manager.TypeUser = EnumTypeUser.Manager;
            personService.Update(manager, null);
          }
          personService.Update(person, null);
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
            Workflow = workflowService.NewFlow(viewFlow)
          };
          autoManagerService.Insert(auto);
          //searsh model mail database
          var model = mailModelService.AutoManager(path);
          var url = path + "evaluationconfiguration/automanager/" + person._id.ToString() + "/approved/" + manager._id.ToString();
          var message = new MailMessage
          {
            Type = EnumTypeMailMessage.Put,
            Name = model.Name,
            Url = url,
            Body = " { '._idWorkFlow': '" + auto.Workflow.FirstOrDefault()._id.ToString() + "' } "
          };
          var idMessageApv = mailMessageService.Insert(message)._id;
          var requestor = personService.GetAll(p => p._id == auto.Workflow.FirstOrDefault().Requestor._id).FirstOrDefault();
          var body = model.Message.Replace("{Person}", auto.Workflow.FirstOrDefault().Requestor.Name);
          body = body.Replace("{Requestor}", auto.Requestor.Name);
          body = body.Replace("{Employee}", person.Name);
          // approved link
          body = body.Replace("{Approved}", model.Link + "/" + idMessageApv.ToString());
          url = path + "evaluationconfiguration/automanager/" + person._id.ToString() + "/disapproved/" + manager._id.ToString();
          //disapproved link
          message.Url = url;
          var idMessageDis = mailMessageService.Insert(message)._id;
          body = body.Replace("{Disapproved}", model.Link + "/" + idMessageDis.ToString());
          var sendMail = new MailLog
          {
            From = new MailLogAddress("suporte@jmsoft.com.br", "Suporte"),
            To = new List<MailLogAddress>(){
                  new MailLogAddress(requestor.Mail, requestor.Name)
              },
            Priority = EnumPriorityMail.Low,
            _idPerson = person._id,
            NamePerson = person.Name,
            Body = body,
            StatusMail = EnumStatusMail.Sended,
            Included = DateTime.Now,
            Subject = model.Subject,
          };
          mailService.Insert(sendMail);
          var messageApv = mailMessageService.GetAll(p => p._id == idMessageApv).FirstOrDefault();
          var messageDis = mailMessageService.GetAll(p => p._id == idMessageDis).FirstOrDefault();
          var token = SendMail(path, person, sendMail._id.ToString());
          messageApv.Token = token;
          mailMessageService.Update(messageApv, null);
          messageDis.Token = token;
          mailMessageService.Update(messageDis, null);
        };
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
          //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiSnVyZW1pciBNaWxhbmkiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9oYXNoIjoiNWI0ZGYwNzNlYzhjOGUwYzYwZWFjZDQ5IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoianVyZW1pckBqbXNvZnQuY29tLmJyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiJTdXBwb3J0IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy91c2VyZGF0YSI6IjViNGRmMGJmNDc4MTg4MjE2MDAzMzRmZiIsImV4cCI6MTU2MzM3MjgzOCwiaXNzIjoibG9jYWxob3N0IiwiYXVkIjoibG9jYWxob3N0In0.icZWGLcjYQ_iK4e3my5EzXY2m5b0kF7USxcn75vLZCQ");
          var resultMail = client.PostAsync("mail/sendmail/" + idmail, null).Result;
          return auth.Token;
          
        }
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void Disapproved(ViewWorkflow view, string idPerson, string idManager)
    {
      try
      {
        var auto = autoManagerService.GetAll(p => p.Person._id == idPerson & p.Requestor._id == idManager & p.StatusAutoManager == EnumStatusAutoManager.Requested).FirstOrDefault();
        var list = new List<Workflow>();
        foreach (var item in auto.Workflow)
          list.Add(workflowService.Disapproved(view));

        auto.Workflow = list;
        auto.StatusAutoManager = EnumStatusAutoManager.Disapproved;
        autoManagerService.Update(auto, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void Approved(ViewWorkflow view, string idPerson, string idManager)
    {
      try
      {
        var auto = autoManagerService.GetAll(p => p.Person._id == idPerson & p.Requestor._id == idManager & p.StatusAutoManager == EnumStatusAutoManager.Requested).FirstOrDefault();
        var list = new List<Workflow>();
        foreach (var item in auto.Workflow)
          list.Add(workflowService.Approved(view));

        auto.Workflow = list;
        auto.StatusAutoManager = EnumStatusAutoManager.Approved;
        var manager = personService.GetAll(p => p._id == idManager).FirstOrDefault();
        var person = personService.GetAll(p => p._id == idPerson).FirstOrDefault();
        person.Manager = manager;
        personService.Update(person, null);
        if (manager.TypeUser == EnumTypeUser.Employee)
        {
          manager.TypeUser = EnumTypeUser.Manager;
          personService.Update(manager, null);
        }
        autoManagerService.Update(auto, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void Canceled(string idPerson, string idManager)
    {
      try
      {
        var auto = autoManagerService.GetAll(p => p.Person._id == idPerson & p.Requestor._id == idManager & p.StatusAutoManager == EnumStatusAutoManager.Requested).FirstOrDefault();
        auto.StatusAutoManager = EnumStatusAutoManager.Canceled;
        autoManagerService.Update(auto, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewAutoManager> GetApproved(string idManager)
    {
      try
      {
        return (from auto in autoManagerService.GetAll()
                select auto
                    ).ToList()
                    .Where(p => p.Workflow.Where(t => t.StatusWorkflow == EnumWorkflow.Open
                    & t.Requestor._id == idManager).Count() > 0)
                    .Select(auto => new ViewAutoManager()
                    {
                      IdWorkflow = auto.Workflow.FirstOrDefault()._id,
                      IdRequestor = auto.Requestor._id,
                      NameRequestor = auto.Requestor.Name,
                      IdPerson = auto.Person._id.ToString(),
                      NamePerson = auto.Person.Name
                    }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void DeleteManager(string idPerson)
    {
      try
      {
        var person = personService.GetAll(p => p._id == idPerson).FirstOrDefault();
        person.Manager = null;
        personService.Update(person, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      workflowService.SetUser(contextAccessor);
      personService._user = _user;
      autoManagerService._user = _user;
      mailService._user = _user;
      mailModelService._user = _user;
      mailMessageService._user = _user;
    }
  }
}
