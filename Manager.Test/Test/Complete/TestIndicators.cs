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
  public class TestIndicators : TestCommons<Account>
  {
    private readonly IServiceIndicators serviceIndicators;
    private readonly ServiceGeneric<Person> servicePerson;

    public TestIndicators()
    {
      try
      {
        base.Init();
        serviceIndicators = new ServiceIndicators(base.context, "");
        servicePerson = new ServiceGeneric<Person>(base.context);


        servicePerson._user = base.baseUser;

        serviceIndicators.SetUser(base.contextAccessor);
        //serviceIndicators._user = base.baseUser;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void indicatorssTest()
    {
      try
      {
        var idmanager = "5b7c06188dc3106609728c6d";
        
        var indicators = serviceIndicators.ListTagsCloud(idmanager);

      }

      catch (Exception e)
      {
        throw e;
      }
    }

  }

}
