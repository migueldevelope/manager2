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
    private readonly ServiceGeneric<Company> serviceCompany;

    public TestInfra()
    {
      base.Init();
      serviceInfra = new ServiceInfra(base.context);
      serviceInfra.SetUser(base.contextAccessor);

      servicePerson = new ServiceGeneric<Person>(base.context);
      serviceGroup = new ServiceGeneric<Group>(base.context);
      serviceOccupation = new ServiceGeneric<Occupation>(base.context);
      serviceCompany = new ServiceGeneric<Company>(base.context);

      servicePerson._user = base.baseUser;
      serviceGroup._user = base.baseUser;
      serviceOccupation._user = base.baseUser;
      serviceCompany._user = base.baseUser;


    }



    //[Fact]
    //public void TestCompany()
    //{
    //  try
    //  {
    //    foreach (var item in serviceCompany.GetAll().ToList())
    //    {
    //      item.Skills = new List<Skill>();
    //      serviceCompany.Update(item, null);
    //    }

    //    var comp = new Company()
    //    {
    //      Name = "Outra ",
    //      Skills = new List<Skill>(),
    //      Status = EnumStatus.Enabled
    //    };

    //    serviceCompany.Insert(comp);


    //  }
    //  catch (Exception e)
    //  {
    //    throw e;
    //  }
    //}

    [Fact]
    public void TestBasicVersion0()
    {
      try
      {
        var ares = serviceInfra.GetAreas();
        var company = serviceInfra.GetCompanies().FirstOrDefault();
        serviceInfra.AddSkill(new ViewAddSkill() { Name = "Skill 3", TypeSkill = EnumTypeSkill.Hard });
        long total = 0;
        var skill = serviceInfra.GetSkills(ref total, "3", 100, 1).FirstOrDefault();
        serviceInfra.AddSphere(new Sphere() { Name = "Tatico", TypeSphere = EnumTypeSphere.Strategic, Company = company });
        var sphere = serviceInfra.GetSpheres().FirstOrDefault();
        serviceInfra.AddAxis(new Axis() { Name = "Tecnico", TypeAxis = EnumTypeAxis.Administrator, Sphere = sphere, });
        var axis = serviceInfra.GetAxis().FirstOrDefault();
        serviceInfra.AddEssential(new ViewAddEssential() { Company = company, Skill = skill });

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void TestBasicValidSchooling()
    {
      try
      {
        var schooling = new Schooling()
        {
          Name = "Teste1",
          Complement = "1",
          Type = EnumTypeSchooling.Excellence,
          Status = EnumStatus.Enabled
        };
        var group = servicePerson.GetAll(p => p.Mail == "miguel@jmsoft.com.br").FirstOrDefault().Occupation.Group;

        var view = new ViewAddMapGroupSchooling()
        {
          Group = group,
          Schooling = schooling
        };

        var test = serviceInfra.AddMapGroupSchooling(view);


      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void TestDelete0()
    {
      try
      {
        //5b59d5bda49e0f344cd97fb6/5b5a23bd3ac6f1466cdd7d3d

        var idcompany = "5b59d5bda49e0f344cd97fb6";
        var id = "5b5a23bd3ac6f1466cdd7d3d";
        serviceInfra.DeleteEssential(idcompany, id);
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    [Fact]
    public void TestUpdateBasicVersion0()
    {
      try
      {
        var company = serviceInfra.GetCompanies().FirstOrDefault();
        long total = 0;
        var groups = serviceInfra.GetGroups();
        //var skills = serviceInfra.GetSkills(ref total, "", 100, 1);
        var skill = serviceInfra.GetSkills(company._id, ref total, "", 100, 1).FirstOrDefault();
        var sphere = serviceInfra.GetSpheres().FirstOrDefault();
        var axis = serviceInfra.GetAxis().FirstOrDefault();

        sphere.Name = sphere.Name + " test";
        serviceInfra.UpdateSphere(sphere);

        var idcompany = "5b59d5bda49e0f344cd97fb6";
        var id = "5b5a23bd3ac6f1466cdd7d3d";



      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
