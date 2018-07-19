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
  public class TestInfra : TestCommons<Career>
  {
    private ServiceInfra serviceInfra;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<OccupationGroup> serviceOccupationGroup;
    private readonly ServiceGeneric<Occupation> serviceOccupation;

    public TestInfra()
    {
      base.Init();
      serviceInfra = new ServiceInfra(base.context);
      serviceInfra.SetUser(base.contextAccessor);

      servicePerson = new ServiceGeneric<Person>(base.context);
      serviceOccupationGroup = new ServiceGeneric<OccupationGroup>(base.context);
      serviceOccupation = new ServiceGeneric<Occupation>(base.context);

      servicePerson._user = base.baseUser;
      serviceOccupationGroup._user = base.baseUser;
      serviceOccupation._user = base.baseUser;


    }

    [Fact]
    public void NewOccupationLineTest()
    {
      try
      {
        var manager = this.servicePerson.GetAll(p => p.Mail == "suporte@jmsoft.com.br").FirstOrDefault();
        var occupationgroup = this.serviceOccupationGroup.GetAll(p => p.Name == "Analisty").FirstOrDefault();
        var area = this.serviceInfra.NewArea(new ViewAreaNew() { Name = "Financeiro", IdCompany = manager.Company._id.ToString(),  });

        var view = new ViewOccupationLineNew()
        {
          IdArea = area.Id,
          NameOccupation = "System Analisty",
          IdOccupationGroup = occupationgroup._id,
          
          TypeSphere = EnumTypeSphere.Operational
        };

        var occupation = this.serviceInfra.NewOccupationLine(view);

        var list = this.serviceInfra.GetLinesOccuation(manager.Company._id);
        if (list.Count() == 0)
          new Exception();

        var areas = this.serviceInfra.GetArea(manager.Company._id);
        if (areas.Count() == 0)
          new Exception();

        this.serviceInfra.DeleteOccupationLine(occupation);
        if (list.Count() > 0)
          new Exception();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void NewCareerTest()
    {
      try
      {
        //Get user manutation
        var manager = this.servicePerson.GetAll(p => p.Mail == "suporte@jmsoft.com.br").FirstOrDefault();

        // New career
        var viewCareer = new ViewCareerNew()
        {
          Name = "Carreira X",
          IdCompany = manager.Company._id,
          
          Type = EnumTypeCareer.X
        };
        this.serviceInfra.NewCareer(viewCareer);

        // New Sphere
        var viewSphere = new ViewSphereNew()
        {
          Name = "Operacional",
          IdCompany = manager.Company._id,
          
          Type = EnumTypeSphere.Operational
        };
        this.serviceInfra.NewSphere(viewSphere);

        viewSphere = new ViewSphereNew()
        {
          Name = "Tático",
          IdCompany = manager.Company._id,
          
          Type = EnumTypeSphere.Tactical
        };
        this.serviceInfra.NewSphere(viewSphere);

        viewSphere = new ViewSphereNew()
        {
          Name = "Estratégico",
          IdCompany = manager.Company._id,
          
          Type = EnumTypeSphere.Strategic
        };
        this.serviceInfra.NewSphere(viewSphere);

        // New Axis
        var viewAxis = new ViewAxisNew()
        {
          Name = "Gestão",
          IdCompany = manager.Company._id,
          Type = EnumTypeAxis.A
        };
        this.serviceInfra.NewAxis(viewAxis);

        viewAxis = new ViewAxisNew()
        {
          Name = "Técnico",
          IdCompany = manager.Company._id,
          Type = EnumTypeAxis.B
        };
        this.serviceInfra.NewAxis(viewAxis);

        viewAxis = new ViewAxisNew()
        {
          Name = "Administrativo",
          IdCompany = manager.Company._id,
          Type = EnumTypeAxis.C
        };
        this.serviceInfra.NewAxis(viewAxis);

        viewAxis = new ViewAxisNew()
        {
          Name = "Operacional",
          IdCompany = manager.Company._id,
          Type = EnumTypeAxis.D
        };
        this.serviceInfra.NewAxis(viewAxis);

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void NewCareerGroupTest()
    {
      try
      {
        //Get user manutation
        var manager = this.servicePerson.GetAll(p => p.Mail == "suporte@jmsoft.com.br").FirstOrDefault();
        var career = this.serviceInfra.GetAll().FirstOrDefault();

        //Groups new
        //Axis A

        var viewOccupationGroup = new ViewOccupationGroupCareerNew()
        {
          NameCareer = career.Name,
          IdCareer = career._id,
          TypeAxis = EnumTypeAxis.A,
          TypeSphere = EnumTypeSphere.Strategic,
          NameOccupationGroup = "Coordenador",
          IdCompany = manager.Company._id,
          
        };
        this.serviceInfra.NewOccupationGroupCareer(viewOccupationGroup, EnumTypeCareer.X);

        viewOccupationGroup = new ViewOccupationGroupCareerNew()
        {
          NameCareer = career.Name,
          IdCareer = career._id,
          TypeAxis = EnumTypeAxis.A,
          TypeSphere = EnumTypeSphere.Strategic,
          NameOccupationGroup = "Gerente",
          IdCompany = manager.Company._id,
          
        };
        this.serviceInfra.NewOccupationGroupCareer(viewOccupationGroup, EnumTypeCareer.X);

        viewOccupationGroup = new ViewOccupationGroupCareerNew()
        {
          NameCareer = career.Name,
          IdCareer = career._id,
          TypeAxis = EnumTypeAxis.A,
          TypeSphere = EnumTypeSphere.Strategic,
          NameOccupationGroup = "Diretor",
          IdCompany = manager.Company._id,
          
        };
        this.serviceInfra.NewOccupationGroupCareer(viewOccupationGroup, EnumTypeCareer.X);

        //Axis B
        viewOccupationGroup = new ViewOccupationGroupCareerNew()
        {
          NameCareer = career.Name,
          IdCareer = career._id,
          TypeAxis = EnumTypeAxis.B,
          TypeSphere = EnumTypeSphere.Tactical,
          NameOccupationGroup = "Especialista",
          IdCompany = manager.Company._id,
          
        };
        this.serviceInfra.NewOccupationGroupCareer(viewOccupationGroup, EnumTypeCareer.X);

        viewOccupationGroup = new ViewOccupationGroupCareerNew()
        {
          NameCareer = career.Name,
          IdCareer = career._id,
          TypeAxis = EnumTypeAxis.B,
          TypeSphere = EnumTypeSphere.Strategic,
          NameOccupationGroup = "Assessor",
          IdCompany = manager.Company._id,
          
        };
        this.serviceInfra.NewOccupationGroupCareer(viewOccupationGroup, EnumTypeCareer.X);

        //Axis C
        viewOccupationGroup = new ViewOccupationGroupCareerNew()
        {
          NameCareer = career.Name,
          IdCareer = career._id,
          TypeAxis = EnumTypeAxis.C,
          TypeSphere = EnumTypeSphere.Tactical,
          NameOccupationGroup = "Analisty",
          IdCompany = manager.Company._id,
          
        };

        this.serviceInfra.NewOccupationGroupCareer(viewOccupationGroup, EnumTypeCareer.X);
        viewOccupationGroup = new ViewOccupationGroupCareerNew()
        {
          NameCareer = career.Name,
          IdCareer = career._id,
          TypeAxis = EnumTypeAxis.C,
          TypeSphere = EnumTypeSphere.Operational,
          NameOccupationGroup = "Assistentes",
          IdCompany = manager.Company._id,
          
        };
        this.serviceInfra.NewOccupationGroupCareer(viewOccupationGroup, EnumTypeCareer.X);

        viewOccupationGroup = new ViewOccupationGroupCareerNew()
        {
          NameCareer = career.Name,
          IdCareer = career._id,
          TypeAxis = EnumTypeAxis.C,
          TypeSphere = EnumTypeSphere.Operational,
          NameOccupationGroup = "Auxiliar",
          IdCompany = manager.Company._id,
          
        };
        this.serviceInfra.NewOccupationGroupCareer(viewOccupationGroup, EnumTypeCareer.X);

        viewOccupationGroup = new ViewOccupationGroupCareerNew()
        {
          NameCareer = career.Name,
          IdCareer = career._id,
          TypeAxis = EnumTypeAxis.D,
          TypeSphere = EnumTypeSphere.Tactical,
          NameOccupationGroup = "Facilitador",
          IdCompany = manager.Company._id,
          
        };
        this.serviceInfra.NewOccupationGroupCareer(viewOccupationGroup, EnumTypeCareer.X);

        viewOccupationGroup = new ViewOccupationGroupCareerNew()
        {
          NameCareer = career.Name,
          IdCareer = career._id,
          TypeAxis = EnumTypeAxis.D,
          TypeSphere = EnumTypeSphere.Operational,
          NameOccupationGroup = "Técnico",
          IdCompany = manager.Company._id,
          
        };
        this.serviceInfra.NewOccupationGroupCareer(viewOccupationGroup, EnumTypeCareer.X);

        viewOccupationGroup = new ViewOccupationGroupCareerNew()
        {
          NameCareer = career.Name,
          IdCareer = career._id,
          TypeAxis = EnumTypeAxis.D,
          TypeSphere = EnumTypeSphere.Operational,
          NameOccupationGroup = "Operador",
          IdCompany = manager.Company._id,
          
        };
        this.serviceInfra.NewOccupationGroupCareer(viewOccupationGroup, EnumTypeCareer.X);

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void GetCareerGroupTest()
    {
      try
      {
        //Get user manutation
        var manager = this.servicePerson.GetAll(p => p.Mail == "suporte@jmsoft.com.br").FirstOrDefault();

        var listcompany = this.serviceInfra.ListGetCareer();
        var listA = this.serviceInfra.GetViewOccupationGroupCareers(manager.Company._id, EnumTypeAxis.A, EnumTypeCareer.X);
        var listB = this.serviceInfra.GetViewOccupationGroupCareers(manager.Company._id, EnumTypeAxis.B, EnumTypeCareer.X);
        var listC = this.serviceInfra.GetViewOccupationGroupCareers(manager.Company._id, EnumTypeAxis.C, EnumTypeCareer.X);
        var listD = this.serviceInfra.GetViewOccupationGroupCareers(manager.Company._id, EnumTypeAxis.D, EnumTypeCareer.X);

        var max = this.serviceInfra.GetMaxPosition(manager.Company._id, EnumTypeCareer.X);

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void UpdateOccupationLineTest()
    {
      try
      {
        //Get user manutation
        var manager = this.servicePerson.GetAll(p => p.Mail == "suporte@jmsoft.com.br").FirstOrDefault();
        var occupation = this.serviceOccupation.GetAll(p => p.Name == "System Analisty").FirstOrDefault();
        var group = this.serviceOccupationGroup.GetAll(p => p.Name == "Assistentes").FirstOrDefault();
        var view = new ViewOccupationLineNew()
        {
          IdArea = occupation.Area._id,
          IdOccupation = occupation._id,
          NameOccupation = occupation.Name,
          Position = occupation.Position,
          IdOccupationGroup = group._id
        };

        serviceInfra.UpdateOccupationLine(view, occupation._id);

      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
