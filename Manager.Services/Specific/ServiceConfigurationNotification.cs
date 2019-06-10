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
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceConfigurationNotifications : Repository<ConfigurationNotification>, IServiceConfigurationNotifications
  {
    private readonly ServiceGeneric<ConfigurationNotification> configurationNotificationsService;
    private readonly ServiceGeneric<Person> personService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceConfigurationNotifications(DataContext context)
      : base(context)
    {
      try
      {
        configurationNotificationsService = new ServiceGeneric<ConfigurationNotification>(context);
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

    public async Task<string> New(ConfigurationNotification view)
    {
      try
      {
        configurationNotificationsService.InsertNewVersion(view);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<string> Update(ConfigurationNotification view)
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

    public async Task<string> Remove(string id)
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

    public async Task<ConfigurationNotification> Get(string id)
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
    public async Task<ConfigurationNotification> GetByName(string name)
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
    public Task<List<ConfigurationNotification>> List( ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = configurationNotificationsService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = configurationNotificationsService.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return Task.FromResult(detail.ToList());

      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
