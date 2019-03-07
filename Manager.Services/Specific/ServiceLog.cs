using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Services.Specific
{
  public class ServiceLog : Repository<Log>, IServiceLog
  {

    private readonly ServiceGeneric<Log> serviceLog;

    #region Constructor
    public ServiceLog(DataContext context) : base(context)
    {
      try
      {
        serviceLog = new ServiceGeneric<Log>(context);
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
    }
    #endregion

    #region Log
    public void NewLog(ViewLog view)
    {
      try
      {
        var log = new Log
        {
          Person = view.Person,
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
    public List<Log> GetLogs(string idaccount, ref long total, int count, int page, string filter)
    {
      try
      {
        int skip = (count * (page - 1));
        List<Log> detail = serviceLog.GetAllFreeNewVersion(p => p.Person._idAccount == idaccount && p.Person.User.Name.ToUpper().Contains(filter.ToUpper()), count, skip, "DataLog DESC").Result;
        total = serviceLog.CountFreeNewVersion(p => p.Person._idAccount == idaccount && p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
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
