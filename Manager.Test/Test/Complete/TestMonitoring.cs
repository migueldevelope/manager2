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
    public class TestMonitoring : TestCommons<Monitoring>
  {
    private ServiceMonitoring serviceMonitoring;
    private readonly ServiceGeneric<Person> servicePerson;

    public TestMonitoring()
    {
      base.Init();
      serviceMonitoring = new ServiceMonitoring(base.context);
      serviceMonitoring.SetUser(base.contextAccessor);

      servicePerson = new ServiceGeneric<Person>(base.context);
      servicePerson._user = base.baseUser;
    }

    [Fact]
    public void TestMonitoringComplete()
    {
      try
      {
        long total = 0;
        var person = servicePerson.GetAll(p => p.Name.Contains("Ariel")).FirstOrDefault();

        var list = serviceMonitoring.ListMonitoringsWait(person.Manager._id, ref total, "Ariel", 10, 1).FirstOrDefault();
        var newOn = serviceMonitoring.NewMonitoring(list, person.Manager._id);

        foreach (var item in newOn.SkillsCompany)
        {
          item.CommentsManager = "teste 1";
        }
        newOn.StatusMonitoring = EnumStatusMonitoring.Wait;
        serviceMonitoring.UpdateMonitoring(newOn, person.Manager._id);

        var listskills = serviceMonitoring.GetSkills(person._id);


      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
