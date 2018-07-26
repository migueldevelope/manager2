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
  public class TestInfra : TestCommons<Group>
  {
    private ServiceInfra serviceInfra;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Group> serviceGroup;
    private readonly ServiceGeneric<Occupation> serviceOccupation;

    public TestInfra()
    {
      base.Init();
      serviceInfra = new ServiceInfra(base.context);
      serviceInfra.SetUser(base.contextAccessor);

      servicePerson = new ServiceGeneric<Person>(base.context);
      serviceGroup = new ServiceGeneric<Group>(base.context);
      serviceOccupation = new ServiceGeneric<Occupation>(base.context);

      servicePerson._user = base.baseUser;
      serviceGroup._user = base.baseUser;
      serviceOccupation._user = base.baseUser;


    }

    [Fact]
    public void TestBasicVersion0()
    {
      try
      {
        var company = serviceInfra.GetCompanies().FirstOrDefault();
        serviceInfra.AddSkill(new ViewAddSkill() { Name = "Skill 1", Type = EnumTypeSkill.Hard });
        long total = 0;
        var skill = serviceInfra.GetSkills(ref total, "", 100, 1).FirstOrDefault();
        serviceInfra.AddSphere(new ViewAddSphere() { Name = "Estrategico", Type = EnumTypeSphere.Strategic, Company = company });
        var sphere = serviceInfra.GetSpheres().FirstOrDefault();
        serviceInfra.AddAxis(new ViewAddAxis() { Name = "Gestão", Type = EnumTypeAxis.A, Sphere = sphere });
        var axis = serviceInfra.GetAxis().FirstOrDefault();
        serviceInfra.AddEssential(new ViewAddEssential() { Company = company, Skill = skill });

      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
