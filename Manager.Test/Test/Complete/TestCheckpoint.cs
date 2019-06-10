using Manager.Core.Business;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Test.Commons;
using Manager.Views.Enumns;
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
      serviceCheckpoint = new ServiceCheckpoint(context, context, "http://10.0.0.14/");
      serviceCheckpoint.SetUser(base.baseUser);

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
        long total = 0;
        var person = servicePerson.GetAllNewVersion(p => p.User.Name.Contains("Ariel")).Result.FirstOrDefault();

        var list = serviceCheckpoint.ListWaitManager(person.Manager._id, ref total, "Ariel", 10, 1).Result.FirstOrDefault();
        var newChecklist = serviceCheckpoint.NewCheckpoint(list._idPerson).Result;
        var getChecklist = serviceCheckpoint.GetCheckpoint(newChecklist._id).Result;

        getChecklist.Comments = "test comments";
        foreach(var item in getChecklist.Questions)
        {
          item.Mark = 2;
        }

        getChecklist.StatusCheckpoint = EnumStatusCheckpoint.End;

        var result = serviceCheckpoint.UpdateCheckpoint(getChecklist).Result;

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

        var person = servicePerson.GetAllNewVersion(p => p.TypeJourney == EnumTypeJourney.Checkpoint).Result.FirstOrDefault();
        serviceCheckpoint.DeleteCheckpoint(person._id);

      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
