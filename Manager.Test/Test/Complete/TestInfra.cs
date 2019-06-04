using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Test.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
      serviceInfra.SetUser(base.baseUser);

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

    //    serviceCompany.InsertNewVersion(comp);


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
        var company = serviceInfra.GetCompanies().Result.FirstOrDefault();
        serviceInfra.AddSkill(new ViewCrudSkill() { Name = "Skill 3", TypeSkill = EnumTypeSkill.Hard });
        long total = 0;
        var skill = serviceInfra.GetSkills("3", 100, 1).Result.FirstOrDefault();
        //serviceInfra.AddSphere(new Sphere() { Name = "Tatico", TypeSphere = EnumTypeSphere.Strategic, Company = company });
        var sphere = serviceInfra.GetSpheres().Result.FirstOrDefault();
        //serviceInfra.AddAxis(new Axis() { Name = "Tecnico", TypeAxis = EnumTypeAxis.Administrator });
        var axis = serviceInfra.GetAxis().Result.FirstOrDefault();
        //serviceInfra.AddEssential(new ViewAddEssential() { Company = company, Skill = skill });

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
        var group = servicePerson.GetAll(p => p.User.Mail == "miguel@jmsoft.com.br").FirstOrDefault().Occupation.Group;

        var view = new ViewAddMapGroupSchooling()
        {
          Group = group,
          Schooling = schooling
        };

        //var test = serviceInfra.AddMapGroupSchooling(view);


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

        //var idcompany = "5b59d5bda49e0f344cd97fb6";
        //var id = "5b5a23bd3ac6f1466cdd7d3d";
        //serviceInfra.DeleteEssential(idcompany, id);

        var schooling = serviceInfra.GetSchooling().Result.Where(p => p.Name.Contains("Teste")).FirstOrDefault();
        schooling.Name = schooling.Name + " Teste 1";
        //serviceInfra.UpdateSchooling(schooling);
        var company = serviceInfra.GetCompanies().Result.FirstOrDefault();
        var area = serviceInfra.GetAreas().Result.FirstOrDefault();
        var map = serviceInfra.GetOccupations(company._id, area._id);

        var group = serviceInfra.GetGroups(company._id);
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    [Fact]
    public void ScriptArea()
    {
      try
      {
        var occupations = serviceOccupation.GetAll(p => p.Status == EnumStatus.Enabled).ToList();
        foreach (var item in occupations)
        {
          //serviceInfra.UpdateOccupation(item);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string[] Export(string[] rel, string message)
    {
      try
      {
        string[] text = rel;
        string[] lines = null;
        try
        {
          lines = new string[text.Count() + 1];
          var count = 0;
          foreach (var item in text)
          {
            lines.SetValue(item, count);
            count += 1;
          }
          lines.SetValue(message, text.Count());
        }
        catch (Exception)
        {
          lines = new string[1];
          lines.SetValue(message, 0);
        }

        return lines;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void TestComplement()
    {
      try
      {
        var occupations = serviceOccupation.GetAll(p => p.Status == EnumStatus.Enabled).ToList();
        var list = new List<Occupation>();
        string[] rel = new string[1];

        foreach (var item in occupations)
        {
          if (item.Schooling.Where(p => p.Complement != null).Count() > 0)
          {
            foreach (var school in item.Schooling)
            {
              if (school.Complement != null)
              {
                string message = item._id + ";" + school._id + ";" + school.Complement;
                rel = Export(rel, message);
              }
            }

          }
          //list.Add(item);
        }
        var filename = "reports/COMP" + DateTime.Now.ToString("yyyyMMddHHmmss") + "a" + ".csv";


        File.WriteAllLines(filename, rel, Encoding.GetEncoding("iso-8859-1"));
        //var test = list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void TestOccupations()
    {
      try
      {
        //var list = new List<Occupation>();
        string[] rel = new string[1];
        foreach (var item in serviceOccupation.GetAll().ToList())
        {
          foreach (var item2 in serviceOccupation.GetAll().ToList())
          {
            if((item2._id != item._id)& (item2.Name == item.Name))
            {
              string message = item._id + ";" + item.Name;
              rel = Export(rel, message);
            }
            
          }
        }
        var filename = "reports/OCCU" + DateTime.Now.ToString("yyyyMMddHHmmss") + "a" + ".csv";
        File.WriteAllLines(filename, rel, Encoding.GetEncoding("iso-8859-1"));
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    //[Fact]
    //public void TestGroupNewAndReorder()
    //{
    //  try
    //  {
    //    var company = serviceInfra.GetCompanies().FirstOrDefault();
    //    var axis = serviceInfra.GetAxis(company._id).FirstOrDefault();
    //    var sphere = serviceInfra.GetSpheres(company._id).FirstOrDefault();
    //    long total = 0;

    //    //var viewGroup = new ViewAddGroup()
    //    //{
    //    //  Axis = axis,
    //    //  Sphere = sphere,
    //    //  Company = company,
    //    //  Line = 0,
    //    //  Name = "Teste group miguel"
    //    //};
    //   // serviceInfra.AddGroup(viewGroup);
    //    var group = serviceInfra.GetGroups(company._id).Where(p => p.Name == "Teste group miguel").FirstOrDefault();


    //    var view = new ViewAddMapGroupScope()
    //    {
    //      Group = group,
    //      Scope = new Scope() { Name = "teste", Order = 99 }
    //    };
    //    //serviceInfra.AddMapGroupScope(view);

    //    var view2 = new ViewAddMapGroupScope()
    //    {
    //      Group = group,
    //      Scope = new Scope() { Name = "teste scope 2", Order = 99 }
    //    };

    //    //serviceInfra.AddMapGroupScope(view2);

    //    var scopeS = serviceInfra.GetGroups(company._id).Where(p => p.Name == "Teste group miguel").FirstOrDefault().Scope.Where(p => p.Order == 1).FirstOrDefault();

    //    serviceInfra.ReorderGroupScope(company._id, group._id, scopeS._id, true);

    //    var view3 = new ViewAddMapGroupScope()
    //    {
    //      Group = group,
    //      Scope = new Scope() { Name = "teste scope 3", Order = 99 }
    //    };

    //    //serviceInfra.AddMapGroupScope(view3);

    //    var scopeD = serviceInfra.GetGroups(company._id).Where(p => p.Name == "Teste group miguel").FirstOrDefault().Scope.Where(p => p.Order == 3).FirstOrDefault();

    //    serviceInfra.ReorderGroupScope(company._id, group._id, scopeD._id, false);

    //  }
    //  catch (Exception e)
    //  {
    //    throw e;
    //  }
    //}


    [Fact]
    public void TestEdiSubProcess()
    {
      try
      {
        //var item = serviceInfra.GetProcessLevelTwo().Where(p => p._id == "5ba912282aacd866424e566f").FirstOrDefault();
        //item.Name = "test sub novo 2";
        //serviceInfra.UpdateProcessLevelTwo(item);

        serviceInfra.GetCSVCompareGroup("5b6c4f54d9090156f08775ab", "");
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    [Fact]
    public void TestEdiArea()
    {
      try
      {
        var item = serviceInfra.GetAreas().Result.Where(p => p._id == "5ba8d349ea4b69b2d3e2408d").FirstOrDefault();
        item.Name = "nv area 51 v5";
        //serviceInfra.UpdateArea(item);

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
        var company = serviceInfra.GetCompanies().Result.FirstOrDefault();
        long total = 0;
        var groups = serviceInfra.GetGroups();
        //var skills = serviceInfra.GetSkills("", 100, 1);
        var skill = serviceInfra.GetSkills(company._id, "", 100, 1).Result.FirstOrDefault();
        var sphere = serviceInfra.GetSpheres().Result.FirstOrDefault();
        var axis = serviceInfra.GetAxis().Result.FirstOrDefault();

        sphere.Name = sphere.Name + " test";
        //serviceInfra.UpdateSphere(sphere);

        //var idcompany = "5b59d5bda49e0f344cd97fb6";
        //var id = "5b5a23bd3ac6f1466cdd7d3d";



      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
