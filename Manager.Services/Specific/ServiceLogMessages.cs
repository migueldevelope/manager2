using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Services.Specific
{
  public class ServiceLogMessages : Repository<LogMessages>, IServiceLogMessages
  {
    private readonly ServiceGeneric<LogMessages> logMessagesService;
    private readonly ServiceGeneric<Person> personService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceLogMessages(DataContext context)
      : base(context)
    {
      try
      {
        logMessagesService = new ServiceGeneric<LogMessages>(context);
        personService = new ServiceGeneric<Person>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void NewLogMessage(string subject, string message, Person person)
    {
      try
      {
        var model = new LogMessages();
        model.Message = message;
        model.Subject = subject;
        model.StatusMessage = EnumStatusMessage.New;
        model.Person = person;
        logMessagesService.Insert(model);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      logMessagesService._user = _user;
      personService._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      _user = baseUser;
      logMessagesService._user = baseUser;
      personService._user = baseUser;
    }

    public string New(LogMessages view)
    {
      try
      {
        logMessagesService.Insert(view);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Update(LogMessages view)
    {
      try
      {
        logMessagesService.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Remove(string id)
    {
      try
      {
        var item = logMessagesService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        logMessagesService.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public LogMessages Get(string id)
    {
      try
      {
        return logMessagesService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<LogMessages> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = logMessagesService.GetAll(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.Name).Skip(skip).Take(count).ToList();
        total = logMessagesService.GetAll(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<LogMessages> ListManager(string idmanager, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        //var manager = personService.GetAll(p => p._id == idmanager).FirstOrDefault();
        var detail = logMessagesService.GetAll(p => p.Person.Manager._id == idmanager & p.Person.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.Name).Skip(skip).Take(count).ToList();
        total = logMessagesService.GetAll(p => p.Person.Manager._id == idmanager & p.Person.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<LogMessages> ListPerson(string idperson, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = logMessagesService.GetAll(p => p.Person._id == idperson & p.Person.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.Name).Skip(skip).Take(count).ToList();
        total = logMessagesService.GetAll(p => p.Person._id == idperson & p.Person.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
}
