using Manager.Core.Base;
using Manager.Core.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
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
      servicePerson.SetUser(baseUser);


      await Clients.All.SendAsync("ReceiveMessageNotes" + idperson, service.GetNotes(idperson));
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
      servicePerson.SetUser(baseUser);


      await Clients.All.SendAsync("ReceiveMessageNotesPerson" + idperson, service.GetNotesPerson(idperson));
    }

  }
}
