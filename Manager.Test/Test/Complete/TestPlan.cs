using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Test.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Manager.Test.Test.Complete
{
  public class TestPlan : TestCommons<Account>
  {
    private readonly IServicePlan servicePlan;
    private readonly ServiceGeneric<Person> servicePerson;

    public TestPlan()
    {
      try
      {
        base.Init();
        servicePlan = new ServicePlan(base.context);
        servicePerson = new ServiceGeneric<Person>(base.context);


        servicePerson._user = base.baseUser;

        servicePlan.SetUser(base.contextAccessor);
        //servicePlan._user = base.baseUser;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void ManagerTest()
    {
      try
      {
        var person = servicePerson.GetAll(p => p.Name.Contains("Miguel")).FirstOrDefault();
        long total = 0;
        var plans = servicePlan.ListPlansPerson(ref total, person._id, "", 100, 1);

      }

      catch (Exception e)
      {
        throw e;
      }
    }

  }

}
