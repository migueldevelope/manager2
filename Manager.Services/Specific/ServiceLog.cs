using Manager.Core.Base;
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
    private readonly ServiceGeneric<Log> logsService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceLog(DataContext context)
      : base(context)
    {
      try
      {
        logsService = new ServiceGeneric<Log>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

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
        logsService.Insert(log);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      logsService._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      logsService._user = baseUser;
    }

    public List<Log> GetLogs(string idaccount, ref long total, int count, int page, string filter)
    {
      try
      {
        int skip = (count * (page - 1));
        //var detail = logsService.GetAll(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        var detail = logsService.GetAuthentication(p => p.Person._idAccount == idaccount & p.Person.Name.ToUpper().Contains(filter.ToUpper())).OrderByDescending(p => p.DataLog).Skip(skip).Take(count);
        //total = detail.Count();
        total = logsService.GetAll(p => p.Person._idAccount == idaccount & p.Person.Name.ToUpper().Contains(filter.ToUpper())).Count();

        //return detail.Skip(skip).Take(count).OrderByDescending(p => p.DataLog).ToList();
        return detail.OrderByDescending(p => p.DataLog).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
  }
}
