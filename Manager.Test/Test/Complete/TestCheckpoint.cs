using Manager.Core.Business;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Test.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Manager.Test.Test.Complete
{
  public class TestCheckpoint : TestCommons<Checkpoint>
  {
    private ServiceCheckpoint serviceCheckpoint;
    private readonly ServiceGeneric<Person> servicePerson;

    public TestCheckpoint()
    {
      base.Init();
      serviceCheckpoint = new ServiceCheckpoint(context, context, "");
      serviceCheckpoint.SetUser(base.contextAccessor);

      servicePerson = new ServiceGeneric<Person>(base.context)
      {
        _user = base.baseUser
      };
    }

    [Fact]
    public void TestCheckpointComplete()
    {
      try
      {
        //long total = 0;
        //var person = servicePerson.GetAll(p => p.User.Name.Contains("Ariel")).FirstOrDefault();

        //var list = serviceCheckpoint.ListCheckpointsWaitOld(person.Manager._id, ref total, "Ariel", 10, 1).FirstOrDefault();
        //var newOn = serviceCheckpoint.NewCheckpointOld(list, person.Manager._id);



      }
      catch (Exception e)
      {
        throw e;
      }
    }


    [Fact]
    public void TestDelete()
    {
      try
      {

        //var person = "5b8e9a6adc2492055f5fb68b";
        //serviceCheckpoint.RemoveCheckpointOld(person);

      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
