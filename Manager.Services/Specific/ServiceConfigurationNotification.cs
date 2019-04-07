using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Services.Specific
{
  public class ServiceConfigurationNotifications : Repository<ConfigurationNotifications>, IServiceConfigurationNotifications
  {
    private readonly ServiceGeneric<ConfigurationNotifications> configurationNotificationsService;
    private readonly ServiceGeneric<Person> personService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceConfigurationNotifications(DataContext context)
      : base(context)
    {
      try
      {
        configurationNotificationsService = new ServiceGeneric<ConfigurationNotifications>(context);
        personService = new ServiceGeneric<Person>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      configurationNotificationsService._user = _user;
      personService._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      configurationNotificationsService._user = baseUser;
      personService._user = baseUser;
    }

    public string New(ConfigurationNotifications view)
    {
      try
      {
        configurationNotificationsService.Insert(view);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Update(ConfigurationNotifications view)
    {
      try
      {
        configurationNotificationsService.Update(view, null);
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
        var item = configurationNotificationsService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        configurationNotificationsService.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ConfigurationNotifications Get(string id)
    {
      try
      {
        return configurationNotificationsService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ConfigurationNotifications GetByName(string name)
    {
      try
      {
        return configurationNotificationsService.GetAll(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ConfigurationNotifications> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = configurationNotificationsService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = configurationNotificationsService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
