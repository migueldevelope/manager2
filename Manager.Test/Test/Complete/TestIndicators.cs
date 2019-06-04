using Manager.Core.Base;
using Manager.Core.Business;
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
        serviceIndicators = new ServiceIndicators(context, context, "");
        servicePerson = new ServiceGeneric<Person>(context);


        servicePerson._user = base.baseUser;

        serviceIndicators.SetUser(base.baseUser);
        //serviceIndicators._user = base.baseUser;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void IndicatorssTest()
    {
      try
      {
        //var idmanager = "5b7c06188dc3106609728c6d";
        //long total = 0;
        //var indicators = serviceIndicators.ListTagsCloud(idmanager);

        //var export = serviceIndicators.ExportStatusOnboarding("", 9999999, 1);

      }

      catch (Exception e)
      {
        throw e;
      }
    }

  }

}
