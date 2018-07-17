using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using System;

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
  }
}
