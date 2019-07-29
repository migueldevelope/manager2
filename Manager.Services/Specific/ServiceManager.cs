using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Services.Specific
{
  public class ServiceManager : Repository<Person>, IServiceManager
  {
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<DirectTeam> serviceDirectTeam;
    private readonly ServiceGeneric<ListManager> serviceListManager;
    private readonly ServiceGeneric<StructManager> serviceStructManager;

    private readonly IServiceControlQueue serviceControlQueue;
    public ServiceManager(DataContext context, IServiceControlQueue _serviceControlQueue) : base(context)
    {
      try
      {
        servicePerson = new ServiceGeneric<Person>(context);
        serviceDirectTeam = new ServiceGeneric<DirectTeam>(context);
        serviceListManager = new ServiceGeneric<ListManager>(context);
        serviceStructManager = new ServiceGeneric<StructManager>(context);
        serviceControlQueue = _serviceControlQueue;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #region public

    public void UpdateStructManager()
    {
      try
      {
        // clean list
        RemoveListManager();
        // add news managers
        NewListManager();
        // adjust direct team
        NewDirectTeam();
        NewStructManager();
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    #endregion


    #region private

    private void NewListManager()
    {
      try
      {
        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.ErrorIntegration
        & p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator &
        p.TypeUser != EnumTypeUser.Support).Result;

        foreach (var item in persons)
        {
          if (item.Manager != null)
          {
            var exists = serviceListManager.CountNewVersion(p => p._idManager == item.Manager._id).Result;
            if (exists == 0)
            {
              var result = serviceListManager.InsertNewVersion(new ListManager() { _idManager = item.Manager._id });
            }
          }
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
          var persons = servicePerson.CountNewVersion(p => p.StatusUser != EnumStatusUser.ErrorIntegration
          & p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator &
            p.TypeUser != EnumTypeUser.Support & p.Manager._id == item._idManager).Result;
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

    private void NewDirectTeam()
    {
      try
      {
        var listManager = serviceListManager.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        foreach (var item in listManager)
        {
          var team = servicePerson.GetAllNewVersion(p => p.Manager._id == item._idManager).Result;
          foreach (var person in team)
          {
            var exists = serviceDirectTeam.CountNewVersion(p => p._idManager == item._idManager && p._idPerson == person._id).Result;
            if (exists == 0)
            {
              var result = serviceDirectTeam.InsertNewVersion(new DirectTeam()
              {
                _idPerson = person._id,
                _idManager = item._idManager
              });
            }
            RemoveManager(item._id, item._idManager);
          }
        }
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

    #endregion
  }
}
