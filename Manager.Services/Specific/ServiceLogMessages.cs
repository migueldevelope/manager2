using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceLogMessages : Repository<LogMessages>, IServiceLogMessages
  {
    private readonly ServiceGeneric<LogMessages> serviceLogMessages;
    private readonly ServiceGeneric<Person> servicePerson;

    #region Constructor
    public ServiceLogMessages(DataContext context) : base(context)
    {
      try
      {
        serviceLogMessages = new ServiceGeneric<LogMessages>(context);
        servicePerson = new ServiceGeneric<Person>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceLogMessages._user = _user;
      servicePerson._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceLogMessages._user = user;
      servicePerson._user = user;
    }
    #endregion

    #region Mensageria
    public List<ViewListLogMessages> ListPerson(string idperson,  ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListLogMessages> detail = serviceLogMessages.GetAllNewVersion(p => p.Person._id == idperson && p.Person.Name.ToUpper().Contains(filter.ToUpper()),count, count * (page - 1), "Register")
          .Result.Select(x => new ViewListLogMessages()
          {
            _id = x._id,
            Person = x.Person,
          }).ToList();
        total = serviceLogMessages.CountNewVersion(p => p.Person._id == idperson && p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListLogMessages> ListManager(string idmanager,  ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        throw new Exception("Falta a seleção da equipe do gestor");
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  string New(ViewCrudLogMessages view)
    {
      try
      {
        LogMessages logMessages = new LogMessages()
        {
          Person = servicePerson.GetNewVersion(p => p._id == view.Person._id).Result.GetViewListBase(),
          Subject = view.Subject,
          StatusMessage = view.StatusMessage,
          Message = view.Message,
          Register = DateTime.Now
        };
        serviceLogMessages.InsertNewVersion(logMessages).Wait();
        return "Messagery added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  ViewCrudLogMessages NewNotExist(ViewCrudLogMessages view)
    {
      try
      {
        LogMessages logMessage =  serviceLogMessages.GetNewVersion(p => p.Subject == view.Subject && p.Person._id == view.Person._id).Result;

        // Se já existir a notificação de admissão, não enviar mais
        if (logMessage != null)
          return null;

        logMessage = new LogMessages()
        {
          Person = servicePerson.GetNewVersion(p => p._id == view.Person._id).Result.GetViewListBase(),
          Subject = view.Subject,
          StatusMessage = view.StatusMessage,
          Message = view.Message,
          Register = DateTime.Now
        };
        logMessage = serviceLogMessages.InsertNewVersion(logMessage).Result;
        return new ViewCrudLogMessages()
        {
          Subject = logMessage.Subject,
          Message = logMessage.Message,
          Register = logMessage.Register,
          StatusMessage = logMessage.StatusMessage,
          Person = logMessage.Person,
          _id = logMessage._id
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  ViewCrudLogMessages Get(string id)
    {
      try
      {
        var logMessages =  serviceLogMessages.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudLogMessages()
        {
          _id = logMessages._id,
          Message = logMessages.Message,
          Register = logMessages.Register,
          StatusMessage = logMessages.StatusMessage,
          Subject = logMessages.Subject,
          Person = logMessages.Person
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  string Update(ViewCrudLogMessages view)
    {
      try
      {
        LogMessages logMessages = serviceLogMessages.GetNewVersion(p => p._id == view._id).Result;
        logMessages.Person = servicePerson.GetNewVersion(p => p._id == view.Person._id).Result.GetViewListBase();
        logMessages.Subject = view.Subject;
        logMessages.StatusMessage = view.StatusMessage;
        logMessages.Message = view.Message;
         serviceLogMessages.Update(logMessages, null).Wait();
        return "Messagery altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  string Delete(string id)
    {
      try
      {
        var item = serviceLogMessages.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
        item.Status = EnumStatus.Disabled;
         serviceLogMessages.Update(item, null).Wait();
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Internal Call
    public void NewLogMessage(string subject, string message, Person person)
    {
      try
      {
        LogMessages model = new LogMessages
        {
          Message = message,
          Subject = subject,
          StatusMessage = EnumStatusMessage.New,
          Person = person.GetViewListBase()
        };
         serviceLogMessages.InsertNewVersion(model).Wait();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

  }
}
