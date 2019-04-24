using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Services.Specific
{
  public class ServiceLog : Repository<Log>, IServiceLog
  {

    private readonly ServiceGeneric<Log> serviceLog;
    private readonly ServiceGeneric<LogOld> serviceLogOld;
    private readonly ServiceGeneric<Person> servicePerson;

    #region Constructor
    public ServiceLog(DataContext context, DataContext contextLog) : base(context)
    {
      try
      {
        serviceLog = new ServiceGeneric<Log>(contextLog);
        serviceLogOld = new ServiceGeneric<LogOld>(contextLog);
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
      serviceLog._user = _user;
      servicePerson._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceLog._user = user;
      servicePerson._user = user;
    }
    #endregion

    #region Log
    public void NewLog(ViewLog view)
    {
      try
      {
        var person = servicePerson.GetAll(p => p._id == view._idPerson).FirstOrDefault();
        var log = new Log
        {
          Person = person.GetViewList(),
          DataLog = DateTime.Now,
          Description = view.Description,
          Status = EnumStatus.Enabled,
          Local = view.Local
        };
        log = serviceLog.InsertNewVersion(log).Result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListLog> ListLogs(string idaccount, ref long total, int count, int page, string filter)
    {
      try
      {
        int skip = (count * (page - 1));
        List<Log> detail = serviceLog.GetAllFreeNewVersion(p => p._idAccount == idaccount && p.Person.User.Name.ToUpper().Contains(filter.ToUpper()), count, skip, "DataLog DESC").Result;
        total = serviceLog.CountFreeNewVersion(p => p._idAccount == idaccount && p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail.Select(p => new ViewListLog()
        {
          DataLog = p.DataLog,
          Description = p.Description,
          Local = p.Local,
          Person = (p.Person == null)? "Service" : p.Person.User.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Migrar LogOld -> Log
    public void MigrarOld()
    {
      try
      {
        IQueryable<LogOld> logs = serviceLogOld.GetAll();
        Log log = null;
        foreach (var logOld in logs)
        {
          log = new Log()
          {
            DataLog = logOld.DataLog,
            Description = logOld.Description,
            Local = logOld.Local,
            Person = logOld.Person.GetViewList(),
            Status = logOld.Status,
            _id = logOld._id,
            _idAccount = logOld._idAccount
          };
          log = serviceLog.InsertFreeNewVersion(log).Result;
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion
  }
}
