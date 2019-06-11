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
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceLog : Repository<Log>, IServiceLog
  {

    private readonly ServiceGeneric<Log> serviceLog;
    private readonly ServiceGeneric<Person> servicePerson;

    #region Constructor
    public ServiceLog(DataContext context, DataContext contextLog) : base(context)
    {
      try
      {
        serviceLog = new ServiceGeneric<Log>(contextLog);
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
    public async Task NewLog(ViewLog view)
    {
      try
      {
        var person = servicePerson.GetAllNewVersion(p => p._id == view._idPerson).Result.FirstOrDefault();
        var log = new Log
        {
          Person = person.GetViewList(),
          DataLog = DateTime.Now,
          Description = view.Description,
          Status = EnumStatus.Enabled,
          Local = view.Local
        };
        log = await serviceLog.InsertNewVersion(log);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task NewLogService(ViewLog view)
    {
      try
      {
        var log = new Log
        {
          Person = null,
          DataLog = DateTime.Now,
          Description = view.Description,
          Status = EnumStatus.Enabled,
          Local = view.Local
        };
        log = await serviceLog.InsertNewVersion(log);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public Task<List<ViewListLog>> ListLogs(string idaccount,  ref long total, int count, int page, string filter)
    {
      try
      {
        int skip = (count * (page - 1));
        List<Log> detail = serviceLog.GetAllFreeNewVersion(p => p._idAccount == idaccount && p.Person.User.Name.ToUpper().Contains(filter.ToUpper()), count, skip, "DataLog DESC").Result;
        total = serviceLog.CountFreeNewVersion(p => p._idAccount == idaccount && p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return Task.FromResult(detail.Select(p => new ViewListLog()
        {
          DataLog = p.DataLog,
          Description = p.Description,
          Local = p.Local,
          Person = (p.Person == null)? "Service" : p.Person.User.Name
        }).ToList());
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

  }
}
