using Manager.Core.Base;
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

namespace Manager.Services.Specific
{
  public class ServiceManager : Repository<Person>, IServiceManager
  {
    private readonly ServiceGeneric<DirectTeam> serviceDirectTeam;
    private readonly ServiceGeneric<ListManager> serviceListManager;
    private readonly ServiceGeneric<StructManager> serviceStructManager;
    private readonly IQueueClient queueClient;
    private readonly IServiceControlQueue serviceControlQueue;
    public ServiceManager(DataContext contextStruct, IServiceControlQueue _serviceControlQueue, string serviceBusConnectionString) : base(contextStruct)
    {
      try
      {
        serviceDirectTeam = new ServiceGeneric<DirectTeam>(contextStruct);
        serviceListManager = new ServiceGeneric<ListManager>(contextStruct);
        serviceStructManager = new ServiceGeneric<StructManager>(contextStruct);
        serviceControlQueue = _serviceControlQueue;
        queueClient = new QueueClient(serviceBusConnectionString, "structmanager");
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

    private void NewDirectTeam(string idManager, string idPerson)
    {
      try
      {
        var exists = serviceDirectTeam.CountNewVersion(p => p._idManager == idManager && p._idPerson == idPerson).Result;
        if (exists == 0)
        {
          var result = serviceDirectTeam.InsertNewVersion(new DirectTeam()
          {
            _idPerson = idPerson,
            _idManager = idManager
          });
        }
        RemoveManager(idPerson, idManager);

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void NewStructManager()
    {
      try
      {

      }
      catch (Exception e)
      {
        throw e;
      }
    }



    private async Task ProcessMessagesAsync(Message message, CancellationToken token)
    {
      var view = JsonConvert.DeserializeObject<ViewListStructManagerSend>(Encoding.UTF8.GetString(message.Body));
      SetUser(new BaseUser()
      {
        _idAccount = view._idAccount
      });

      // add news managers
      NewListManager(view._idManager);
      // adjust direct team
      NewDirectTeam(view._idManager, view._idPerson);
      NewStructManager();
      // clean list
      RemoveListManager();

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
