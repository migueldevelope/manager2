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
  public class ServiceNotification : Repository<ConfigurationNotifications>, IServiceNotification
  {
    private readonly ServiceGeneric<ConfigurationNotifications> configurationNotificationsService;
    private readonly ServiceGeneric<Person> personService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceNotification(DataContext context)
      : base(context)
    {
      try
      {
        configurationNotificationsService = new ServiceGeneric<ConfigurationNotifications>(context);
        personService = new ServiceGeneric<Person>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SendMessage()
    {
      throw new NotImplementedException();
    }
  }
}
