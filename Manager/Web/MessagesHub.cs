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
  public class MessagesHub : Hub
  {

    private readonly IServiceIndicators service;
    private readonly IServicePerson servicePerson;

    public MessagesHub(IServiceIndicators _service, IServicePerson _servicePerson)
    {
      service = _service;
      servicePerson = _servicePerson;
    }

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


      Clients.All.SendAsync("ReceiveMessageNotes" + idperson + idaccount, service.GetNotes(idperson));
    }

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


      Clients.All.SendAsync("ReceiveMessageNotesPerson" + idperson + idaccount, service.GetNotesPerson(idperson));
    }

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


      Clients.All.SendAsync("ReceiveMessageTeam" + idperson + idaccount, servicePerson.GetFilterPersons(idperson));
    }

  }
}
