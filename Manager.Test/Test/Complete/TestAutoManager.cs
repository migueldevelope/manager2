using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Test.Commons;
using Manager.Views.BusinessView;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Manager.Test.Test.Complete
{
  public class TestAutoManager : TestCommons<AutoManager>
  {

    private readonly IServiceAutoManager serviceAutoManager;
    private readonly IServicePerson servicePerson;

    public TestAutoManager()
    {
      base.Init();
      servicePerson = new ServicePerson(base.context);
      servicePerson.SetUser(base.contextAccessor);
      serviceAutoManager = new ServiceAutoManager(base.context);
      serviceAutoManager.SetUser(base.contextAccessor);
    }

    [Fact]
    public void TestAutoManagerComplete()
    {
      try
      {
        long total = 0;
        var manager = this.servicePerson.ListPerson(p => p.User.Mail == "miguel@jmsoft.com.br").FirstOrDefault();
        var employee = this.servicePerson.ListPerson(p => p.User.Mail == "ariel@jmsoft.com.br").FirstOrDefault();
        var origin = this.servicePerson.ListPerson(p => p._id == employee.Manager._id).FirstOrDefault();
        //List Persons
        var listPersons = this.serviceAutoManager.List(manager._id.ToString(), ref total, 999, 1, "");
        var listTeam = this.servicePerson.GetPersonTeam(ref total, manager._id.ToString(), "", 10, 1);
        //List Persons Filter
        listPersons = this.serviceAutoManager.List(manager._id.ToString(), ref total, 999, 1, "Ariel");
        //Request
        foreach (var item in listPersons)
        {
          var view = new ViewManager() { IdManager = manager.Manager._id, Status = item.Status };
          this.serviceAutoManager.SetManagerPerson(view, item.IdPerson, "http://10.0.0.15");
        }
        //Approved
        var listApproved = this.serviceAutoManager.GetApproved(employee.Manager._id);
        foreach (var item in listApproved)
        {
          var view = new ViewWorkflow()
          {
            IdWorkflow = item.IdWorkflow,
            Comments = "ok"
          };
          this.serviceAutoManager.Approved(view, item.IdPerson, item.IdRequestor);
        }

        //List Team Filter
        employee = this.servicePerson.ListPerson(p => p.User.Mail == "ariel@jmsoft.com.br").FirstOrDefault();
        listTeam = this.servicePerson.GetPersonTeam(ref total, employee.Manager._id, "Ariel", 10, 1);
        foreach (var item in listTeam)
        {
          this.serviceAutoManager.DeleteManager(item.IdPerson);
        }

        //Set origin manager
        listPersons = this.serviceAutoManager.List(manager._id.ToString(), ref total, 999, 1, "Ariel");
        foreach (var item in listPersons)
        {
          var view = new ViewManager()
          {
            IdManager = origin._id.ToString(),
            Status = item.Status
          };

          this.serviceAutoManager.SetManagerPerson(view, item.IdPerson, "http://10.0.0.15");
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }

}
