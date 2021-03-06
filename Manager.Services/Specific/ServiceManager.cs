﻿using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Manager.Services.Specific
{
  public class ServiceManager : Repository<Person>, IServiceManager
  {
    private readonly ServiceGeneric<DirectTeam> serviceDirectTeam;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<ListManager> serviceListManager;
    private readonly ServiceGeneric<StructManager> serviceStructManager;
    private readonly IQueueClient queueClient;
    private readonly IServiceControlQueue serviceControlQueue;
    private List<DirectTeam> directteam;

    public ServiceManager(DataContext contextStruct, DataContext context, IServiceControlQueue _serviceControlQueue, string _serviceBusConnectionString) : base(contextStruct)
    {
      try
      {
        serviceDirectTeam = new ServiceGeneric<DirectTeam>(contextStruct);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceListManager = new ServiceGeneric<ListManager>(contextStruct);
        serviceStructManager = new ServiceGeneric<StructManager>(contextStruct);
        serviceControlQueue = _serviceControlQueue;
        queueClient = new QueueClient(_serviceBusConnectionString, "structmanager");
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #region public

    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceListManager._user = user;
      serviceStructManager._user = user;
      serviceDirectTeam._user = user;
      servicePerson._user = user;
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {

      User(contextAccessor);
      var user = _user;
      serviceListManager._user = user;
      serviceStructManager._user = user;
      serviceDirectTeam._user = user;
      servicePerson._user = user;
    }

    public void UpdateStructManager()
    {
      try
      {
        var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
        {
          MaxConcurrentCalls = 1,
          AutoComplete = false
        };

        queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);


      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListStructManager> GetHierarchy(string idmanager)
    {
      try
      {
        directteam = serviceDirectTeam.GetAllFreeNewVersion(p => p.Status == EnumStatus.Enabled).Result;

        var personsmanager = directteam.Where(p => p._idManager == idmanager).ToList();
        //var personsmanager = servicePerson.GetAllNewVersion(p => p.Manager._id == idmanager).Result;
        var list = new List<ViewListStructManager>();
        var managers = new List<string>();

        foreach (var item in personsmanager)
        {
          var structmanager = new ViewListStructManager()
          {
            _idManager = idmanager,
            _idPerson = item._idPerson,
            //Name = item.User?.Name,
            Name = item.Name,
            Team = new List<ViewListStructManager>()
          };

          structmanager.Team = GetTeam(new ViewListStructManager { _idPerson = item._idPerson, _idManager = idmanager, Name = item.Name }, managers);

          list.Add(structmanager);
        }


        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    /*  public List<ViewListStructManager> GetHierarchy(string idmanager)
      {
        try
        {
          var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled).Result;

          var list = serviceStructManager.GetAllNewVersion(p => p._idManager == idmanager).Result
            .Select(p => new ViewListStructManager()
            {
              _idManager = p._idManager,
              _id = p._id,
              _idPerson = p._idPerson,
              Name= persons.Where(x => x._id == p._idManager).FirstOrDefault()?.User?.Name,
              Team = p.Team
            }).ToList();

          return list;
        }
        catch (Exception e)
        {
          throw e;
        }
      }*/

    #endregion

    #region private

    private void NewListManager(string idManager)
    {
      try
      {
        var exists = serviceListManager.CountNewVersion(p => p._idManager == idManager).Result;
        if (exists == 0)
        {
          var result = serviceListManager.InsertNewVersion(new ListManager() { _idManager = idManager });
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void RemoveManager(string idperson, string idmanager)
    {
      try
      {
        var remove = serviceDirectTeam.GetAllNewVersion(p => p._idPerson == idperson && p._idManager != idmanager).Result;
        foreach (var item in remove)
        {
          serviceDirectTeam.Delete(item._id, false);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void RemoveListManager()
    {
      try
      {
        var listManager = serviceListManager.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        foreach (var item in listManager)
        {
          var persons = serviceDirectTeam.CountNewVersion(p => p._idManager == item._idManager).Result;
          if (persons == 0)
          {
            serviceListManager.Delete(item._id, false);
          }
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void RemoveDirectTeam(string idperson)
    {
      try
      {
        var person = serviceDirectTeam.GetNewVersion(p => p._idPerson == idperson).Result;
        serviceDirectTeam.Delete(person._id, false);
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    private void NewDirectTeam(string idManager, string idPerson, string name)
    {
      try
      {
        var exists = serviceDirectTeam.CountNewVersion(p => p._idManager == idManager && p._idPerson == idPerson).Result;
        if (exists == 0)
        {
          var result = serviceDirectTeam.InsertNewVersion(new DirectTeam()
          {
            _idPerson = idPerson,
            _idManager = idManager,
            Name = name
          });
        }
        RemoveManager(idPerson, idManager);

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    /*private void NewStructManager()
    {
      try
      {
        serviceStructManager.DeleteAccount();

        var listmanager = serviceListManager.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;

        var managers = new List<string>();

        foreach (var manager in listmanager)
        {
          //Filter Team Manger
          var directteam = serviceDirectTeam.GetAllNewVersion(p => p._idManager == manager._idManager).Result;
          foreach (var it in directteam)
          {
            var structmanager = new StructManager()
            {
              _idManager = it._idManager,
              _idPerson = it._idPerson,
              _idAccount = manager._idAccount,
              Status = 0,
              Team = new List<ViewListStructManager>()
            };

            structmanager.Team = GetTeam(new ViewListStructManager { _idPerson = it._idPerson, _idManager = it._idManager }, managers);
        
            var add = serviceStructManager.InsertNewVersion(structmanager);
          }

        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }*/

    private List<ViewListStructManager> GetTeam(ViewListStructManager it, List<string> managers)
    {

      if (managers.Contains(it._idPerson))
        return null;

      var list = new List<ViewListStructManager>();
      if (it != null)
      {
        //var i = 0;
        //var directteam = serviceDirectTeam.GetAllNewVersion(p => p._idManager == it._idPerson).Result;
         var directteamlocal = directteam.Where(p => p._idManager == it._idPerson).ToList();
        foreach (var person in directteamlocal)
        {

          var team = new ViewListStructManager
          {
            _idManager = person._idManager,
            _idPerson = person._idPerson,
            Name = person.Name,
            //_id = person._id,
            Team = new List<ViewListStructManager>()
          };
          team.Team = GetTeam(team, managers);

          list.Add(team);
          //i += 1;
        }
        managers.Add(it._idPerson);
      }
      return list;
    }

    private async Task ProcessMessagesAsync(Message message, CancellationToken token)
    {
      var view = JsonConvert.DeserializeObject<ViewListStructManagerSend>(Encoding.UTF8.GetString(message.Body));
      SetUser(new BaseUser()
      {
        _idAccount = view._idAccount
      });

      if (view._idManager == null)
      {
        RemoveDirectTeam(view._idPerson);
      }
      else
      {
        // add news managers
        NewListManager(view._idManager);
        // adjust direct team
        NewDirectTeam(view._idManager, view._idPerson, view.Name);
      }
      // clean list
      RemoveListManager();
      //NewStructManager();

      await queueClient.CompleteAsync(message.SystemProperties.LockToken);
    }

    private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
    {
      var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
      return Task.CompletedTask;
    }

    #endregion

  }
}
