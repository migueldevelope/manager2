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
      serviceLogMessages._user = user;
      servicePerson._user = user;
    }
    #endregion

    #region Mensageria
    public List<ViewListLogMessages> ListPerson(string idperson, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListLogMessages> detail = serviceLogMessages.GetAllNewVersion(p => p.Person._id == idperson && p.Person.User.Name.ToUpper().Contains(filter.ToUpper()),count, count * (page - 1), "Register")
          .Result.Select(x => new ViewListLogMessages()
          {
            _id = x._id,
            Person = new ViewListPerson()
            {
              _id = x.Person._id,
              Company = new ViewListCompany() { _id = x.Person.Company._id, Name = x.Person.Company.Name },
              Establishment = new ViewListEstablishment() { _id = x.Person.Establishment._id , Name = x.Person.Establishment.Name },
              Registration = x.Person.Registration,
              User = new ViewListUser() { _id = x.Person.User._id, Name = x.Person.User.Name, Document = x.Person.User.Document, Mail = x.Person.User.Mail, Phone = x.Person.User.Phone }
            }
          }).ToList();
        total = serviceLogMessages.CountNewVersion(p => p.Person._id == idperson && p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListLogMessages> ListManager(string idmanager, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        throw new Exception("Falta a seleção da equipe do gestor");
        //List<ViewListLogMessages> detail = logMessagesService.GetAllNewVersion(p => p.Person._id == idmanager && p.Person.User.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Register")
        //  .Result.Select(x => new ViewListLogMessages()
        //  {
        //    _id = x._id,
        //    Person = new ViewListPerson()
        //    {
        //      _id = x.Person._id,
        //      Company = new ViewListCompany() { _id = x.Person.Company._id, Name = x.Person.Company.Name },
        //      Establishment = new ViewListEstablishment() { _id = x.Person.Establishment._id, Name = x.Person.Establishment.Name },
        //      Registration = x.Person.Registration,
        //      User = new ViewListUser() { _id = x.Person.User._id, Name = x.Person.User.Name, Document = x.Person.User.Document, Mail = x.Person.User.Mail, Phone = x.Person.User.Phone }
        //    }
        //  }).ToList();
        //total = logMessagesService.CountNewVersion(p => p.Person._id == idmanager && p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        //return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string New(ViewCrudLogMessages view)
    {
      try
      {
        LogMessages logMessages = new LogMessages()
        {
          Person = servicePerson.GetNewVersion(p => p._id == view.Person._id).Result,
          Subject = view.Subject,
          StatusMessage = view.StatusMessage,
          Message = view.Message,
          Register = DateTime.Now
        };
        logMessages = serviceLogMessages.InsertNewVersion(logMessages).Result;
        return "Messagery added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudLogMessages Get(string id)
    {
      try
      {
        var logMessages = serviceLogMessages.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudLogMessages()
        {
          _id = logMessages._id,
          Message = logMessages.Message,
          Register = logMessages.Register,
          StatusMessage = logMessages.StatusMessage,
          Subject = logMessages.Subject,
          Person = new ViewListPerson()
          {
            _id = logMessages.Person._id,
            Company = new ViewListCompany() { _id = logMessages.Person.Company._id, Name = logMessages.Person.Company.Name },
            Establishment = new ViewListEstablishment() { _id = logMessages.Person.Establishment._id, Name = logMessages.Person.Establishment.Name },
            Registration = logMessages.Person.Registration,
            User = new ViewListUser() { _id = logMessages.Person.User._id, Name = logMessages.Person.User.Name, Document = logMessages.Person.User.Document, Mail = logMessages.Person.User.Mail, Phone = logMessages.Person.User.Phone }
          }
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudLogMessages view)
    {
      try
      {
        LogMessages logMessages = serviceLogMessages.GetNewVersion(p => p._id == view._id).Result;
        logMessages.Person = servicePerson.GetNewVersion(p => p._id == view.Person._id).Result;
        logMessages.Subject = view.Subject;
        logMessages.StatusMessage = view.StatusMessage;
        logMessages.Message = view.Message;
        serviceLogMessages.Update(logMessages, null);
        return "Messagery altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Delete(string id)
    {
      try
      {
        var item = serviceLogMessages.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        serviceLogMessages.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Old
    public void NewLogMessage(string subject, string message, Person person)
    {
      try
      {
        var model = new LogMessages();
        model.Message = message;
        model.Subject = subject;
        model.StatusMessage = EnumStatusMessage.New;
        model.Person = person;
        serviceLogMessages.Insert(model);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string New(LogMessages view)
    {
      try
      {
        serviceLogMessages.Insert(view);
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
        serviceLogMessages.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string RemoveOld(string id)
    {
      try
      {
        var item = serviceLogMessages.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        serviceLogMessages.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public LogMessages GetOld(string id)
    {
      try
      {
        return serviceLogMessages.GetAll(p => p._id == id).FirstOrDefault();
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
        var detail = serviceLogMessages.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceLogMessages.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<LogMessages> ListManagerOld(string idmanager, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        //var manager = personService.GetAll(p => p._id == idmanager).FirstOrDefault();
        var detail = serviceLogMessages.GetAll(p => p.Person.Manager._id == idmanager & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceLogMessages.GetAll(p => p.Person.Manager._id == idmanager & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<LogMessages> ListPersonOld(string idperson, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceLogMessages.GetAll(p => p.Person._id == idperson & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceLogMessages.GetAll(p => p.Person._id == idperson & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion
  }
}
