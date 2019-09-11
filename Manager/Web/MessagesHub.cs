using Manager.Core.Base;
using Manager.Core.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.Web
{
  /// <summary>
  /// 
  /// </summary>
  public class MessagesHub : Hub
  {

    private readonly IServiceIndicators service;
    private readonly IServicePerson servicePerson;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_service"></param>
    /// <param name="_servicePerson"></param>
    public MessagesHub(IServiceIndicators _service, IServicePerson _servicePerson)
    {
      service = _service;
      servicePerson = _servicePerson;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <param name="idaccount"></param>
    /// <returns></returns>
    public async Task GetNotes(string idperson, string idaccount)
    {
      if (service.VerifyAccount(idaccount) == false)
        throw new Exception();

      var baseUser = new BaseUser()
      {
        _idAccount = idaccount
      };
      service.SetUser(baseUser);
      //servicePerson._user;


      await Clients.All.SendAsync("ReceiveMessageNotes" + idperson + idaccount, service.GetNotes(idperson));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <param name="idaccount"></param>
    /// <returns></returns>
    public async Task GetNotesPerson(string idperson, string idaccount)
    {
      if (service.VerifyAccount(idaccount) == false)
        throw new Exception();

      var baseUser = new BaseUser()
      {
        _idAccount = idaccount
      };
      service.SetUser(baseUser);
      //servicePerson.SetUser(baseUser);


      await Clients.All.SendAsync("ReceiveMessageNotesPerson" + idperson + idaccount, service.GetNotesPerson(idperson));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <param name="idaccount"></param>
    /// <returns></returns>
    public async Task GetFilterPersons(string idperson, string idaccount)
    {
      if (service.VerifyAccount(idaccount) == false)
        throw new Exception();

      var baseUser = new BaseUser()
      {
        _idAccount = idaccount
      };
      service.SetUser(baseUser);
      //servicePerson._user;


      //await Clients.All.SendAsync("ReceiveMessageTeam" + idperson + idaccount, servicePerson.GetFilterPersons(idperson));
    }

  }
}
