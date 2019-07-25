using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Test.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Manager.Test.Test.Complete
{
  public class Test0010Infra : TestCommons<Group>
  {
    public Test0010Infra()
    {
      InitUserInfra();
    }

    [Fact]
    public void TestCompany()
    {
      try
      {
        ServiceCompany serviceCompany = new ServiceCompany(context);
        serviceCompany.SetUser(baseUser);

        ViewCrudCompany view = new ViewCrudCompany()
        {
          Name = string.Format("Company test {0}", DateTime.Now.Date),
          Logo = null
        };
        string result = serviceCompany.New(view);
        if (result != "Company added!")
          throw new Exception("Erro ao incluir nova empresa");

        view = serviceCompany.GetByName(view.Name);
        if (view == null)
          throw new Exception("Erro ao buscar pelo nome da empresa");

        view = serviceCompany.Get(view._id);
        if (view == null)
          throw new Exception("Erro ao buscar pelo id da empresa");

        view.Name = string.Concat(view.Name, " alterada!");
        string saveName = view.Name;
        result = serviceCompany.Update(view);
        if (result != "Company altered!")
          throw new Exception("Erro ao alterar empresa");

        view = serviceCompany.GetByName(view.Name);
        if (view == null)
          throw new Exception("Erro ao buscar pelo nome alterado da empresa");

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void TestEstablishment()
    {
      try
      {
        ServiceCompany serviceCompany = new ServiceCompany(context);
        serviceCompany.SetUser(baseUser);

        ViewListCompany company = serviceCompany.GetNewVersion(p => p.Name == string.Format("Company test {0} alterada!", DateTime.Now.Date)).Result.GetViewList();

        ViewCrudEstablishment view = new ViewCrudEstablishment()
        {
          Name = string.Format("Estabelecimento test {0}", DateTime.Now),
          Company = company
        };
        string result = serviceCompany.NewEstablishment(view);
        if (result != "Establishment added!")
          throw new Exception("Erro ao incluir novo estabelecimento");

        view = serviceCompany.GetEstablishmentByName(view.Company._id, view.Name);
        if (view == null)
          throw new Exception("Erro ao buscar pelo nome do estabelecimento");

        view = serviceCompany.GetEstablishment(view._id);
        if (view == null)
          throw new Exception("Erro ao buscar pelo id do estabelecimento");

        view.Name = string.Concat(view.Name, " alterado!");
        string saveName = view.Name;
        result = serviceCompany.UpdateEstablishment(view);
        if (result != "Establishment altered!")
          throw new Exception("Erro ao alterar estabelecimento");

        view = serviceCompany.GetEstablishmentByName(view.Company._id, view.Name);
        if (view == null)
          throw new Exception("Erro ao buscar pelo nome alterado do estabelecimento");

        view = new ViewCrudEstablishment()
        {
          Name = "Estabelecimento Padrão",
          Company = company
        };
        result = serviceCompany.NewEstablishment(view);
        if (result != "Establishment added!")
          throw new Exception("Erro ao incluir novo estabelecimento padrão");

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void TestSkill()
    {
      try
      {
        ServiceInfra serviceInfra = new ServiceInfra(context);
        serviceInfra.SetUser(baseUser);

        ViewCrudSkill viewCrudSkill;
        for (int i = 1; i < 6; i++)
        {
          viewCrudSkill = new ViewCrudSkill()
          {
            TypeSkill = EnumTypeSkill.Soft,
            Name = string.Format("Essencial {0}",i),
            Concept = string.Format("Conceito da competência essencial {0}", i)
          };
          viewCrudSkill = serviceInfra.AddSkill(viewCrudSkill);
        }

        viewCrudSkill = serviceInfra.GetSkill("Essencial 1");
        if (viewCrudSkill == null)
          throw new Exception("Skill essencial não incluída!");

        viewCrudSkill.Name = "Essencial 1 alterada";
        var result = serviceInfra.UpdateSkill(viewCrudSkill);
        if (result != "update")
          throw new Exception("Skill essencial não alterada!");

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void TestEssentialSkills()
    {
      try
      {
        ServiceCompany serviceCompany = new ServiceCompany(context);
        ServiceInfra serviceInfra = new ServiceInfra(context);
        serviceCompany.SetUser(baseUser);
        serviceInfra.SetUser(baseUser);

        ViewListCompany company = serviceCompany.GetNewVersion(p => p.Name == "Teste Company").Result.GetViewList();

        ViewCrudSkill viewCrudSkill = serviceInfra.GetSkill("Essencial 1 alterada");
        ViewCrudEssential view = new ViewCrudEssential()
        {
          Skill = viewCrudSkill,
          _idCompany = company._id
        };
        var result = serviceInfra.AddEssential(view);
        if (result != "ok")
          throw new Exception("Skill essencial 1 não adicionada no mapa essencial!");

        for (int i = 2; i < 6; i++)
        {
          viewCrudSkill = serviceInfra.GetSkill(string.Format("Essencial {0}", i));
          view = new ViewCrudEssential()
          {
            Skill = viewCrudSkill,
            _idCompany = company._id
          };
          result = serviceInfra.AddEssential(view);
          if (result != "ok")
            throw new Exception(string.Format("Skill essencial {0} não adicionada no mapa essencial!",i));
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void TestSphere()
    {
      try
      {
        ServiceCompany serviceCompany = new ServiceCompany(context);
        ServiceInfra serviceInfra = new ServiceInfra(context);
        serviceCompany.SetUser(baseUser);
        serviceInfra.SetUser(baseUser);

        ViewListCompany company = serviceCompany.GetNewVersion(p => p.Name == string.Format("Company test {0} alterada!", DateTime.Now.Date)).Result.GetViewList();

        ViewCrudSphere view = new ViewCrudSphere()
        {
          Name = "Esfera teste",
          Company = company,
          TypeSphere = EnumTypeSphere.Operational
        };
        string result = serviceInfra.AddSphere(view);
        if (result != "ok")
          throw new Exception("Erro ao incluir nova esfera");

        ViewListSphere viewList = serviceInfra.GetSpheres(company._id)[0];
        if (viewList.Name != "Esfera teste")
          throw new Exception("Erro ao buscar pelo nome da esfera");

        view.Name = string.Concat(view.Name, " alterada!");
      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
}
