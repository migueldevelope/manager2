using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Test.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Manager.Test.Test.Complete
{
  public class TestOnBoarding : TestCommons<OnBoarding>
  {
    private ServiceOnBoarding serviceOnBoarding;
    private readonly ServiceGeneric<Person> servicePerson;

    public TestOnBoarding()
    {
      base.Init();
      serviceOnBoarding = new ServiceOnBoarding(base.context);
      serviceOnBoarding.SetUser(base.contextAccessor);

      servicePerson = new ServiceGeneric<Person>(base.context);
      servicePerson._user = base.baseUser;
    }

    [Fact]
    public void TestOnBoardingComplete()
    {
      try
      {
        long total = 0;
        var person = servicePerson.GetAll(p => p.Name.Contains("Ariel")).FirstOrDefault();

        var list = serviceOnBoarding.ListOnBoardingsWait(person.Manager._id, ref total, "Ariel", 10, 1).FirstOrDefault();
        var newOn = serviceOnBoarding.NewOnBoarding(list, person.Manager._id);

        foreach(var item in newOn.SkillsCompany)
        {
          item.CommentsManager = "teste 1";
        }
        newOn.StatusOnBoarding = EnumStatusOnBoarding.Wait;
        serviceOnBoarding.UpdateOnBoarding(newOn, person.Manager._id);


      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
