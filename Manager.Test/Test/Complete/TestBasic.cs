﻿using Manager.Core.Base;
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
  public class TestBasic : TestCommons<Account>
  {
    private readonly IServiceAccount serviceAccount;
    private readonly ServiceGeneric<Person> servicePerson;
    private IServiceIntegration serviceIntegration;

    public TestBasic()
    {
      try
      {
        base.InitOffAccount();
        serviceAccount = new ServiceAccount(base.context);
        servicePerson = new ServiceGeneric<Person>(base.context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void AccountCompleteTest()
    {
      try
      {
        var view = new ViewNewAccount()
        {
          Mail = "suporte1@jmsoft.com.br",
          NameAccount = "Support Test",
          NameCompany = "Test 1",
          Password = "1234"
        };
        this.serviceAccount.NewAccount(view);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void IntegrationTest()
    {
      try
      {
        base.Init();

        var account = this.serviceAccount.GeAccount(p => p.Name == "Support")._id;
        var baseUser = new BaseUser()
        {
          _idAccount = account
        };
        base.Init();
        serviceIntegration = new ServiceIntegration(base.context);
        serviceIntegration.SetUser(baseUser);

        var list = new List<ViewPersonImport>();

        //Person 1
        var view = new ViewPersonImport()
        {
          Mail = "morgana@jmsoft.com.br",
          Name = "Morgana Basso",
          NameCompany = "Test",
          NameManager = "Miguel Silva",
          NameSchooling = "Posgraduate",
          Password = "1234",
          Phone = "05432025412",
          Registration = 1,
          StatusUser = EnumStatusUser.Enabled,
          DateAdm = DateTime.Parse("2012-01-01"),
          DateBirth = DateTime.Parse("1993-05-11"),
          Document = "01",
          DocumentManager = "01050376056",
          TypeUser = EnumTypeUser.Employee
        };
        list.Add(view);

        //Person 2
        view = new ViewPersonImport()
        {
          Mail = "miguel@jmsoft.com.br",
          Name = "Miguel Silva",
          NameCompany = "System Analisty",
          NameManager = "Juremir Milani",
          NameSchooling = "Graduate",
          Password = "1234",
          Phone = "054992655775",
          Registration = 1,
          StatusUser = EnumStatusUser.Enabled,
          DateAdm = DateTime.Parse("2010-01-01"),
          DateBirth = DateTime.Parse("1990-08-06"),
          Document = "01050376056",
          DocumentManager = "02",
          TypeUser = EnumTypeUser.Administrator
        };
        list.Add(view);

        //Person 3
        view = new ViewPersonImport()
        {
          Mail = "juremir@jmsoft.com.br",
          Name = "Juremir Milani",
          NameCompany = "Test",
          NameManager = "",
          NameSchooling = "Graduate",
          Password = "1234",
          Phone = "",
          Registration = 1,
          StatusUser = EnumStatusUser.Enabled,
          DateAdm = DateTime.Parse("1990-01-01"),
          DateBirth = DateTime.Parse("1990-01-01"),
          Document = "02",
          DocumentManager = "",
          TypeUser = EnumTypeUser.Manager
        };
        list.Add(view);

        //Person 4
        view = new ViewPersonImport()
        {
          Mail = "karine@jmsoft.com.br",
          Name = "Karine Brach",
          NameCompany = "Test",
          NameManager = "Miguel Silva",
          NameSchooling = "Graduate",
          Password = "1234",
          Phone = "05432025412",
          Registration = 1,
          StatusUser = EnumStatusUser.Enabled,
          DateAdm = DateTime.Parse("2018-01-01"),
          DateBirth = DateTime.Parse("1990-01-01"),
          Document = "03",
          DocumentManager = "01050376056",
          TypeUser = EnumTypeUser.Employee
        };
        list.Add(view);

        //Person 5
        view = new ViewPersonImport()
        {
          Mail = "ariel@jmsoft.com.br",
          Name = "Ariel Muterle",
          NameCompany = "Test",
          NameManager = "Juremir Milani",
          NameSchooling = "Graduate",
          Password = "1234",
          Phone = "05432025412",
          Registration = 1,
          StatusUser = EnumStatusUser.Enabled,
          DateAdm = DateTime.Parse("2000-01-01"),
          DateBirth = DateTime.Parse("1990-05-11"),
          Document = "04",
          DocumentManager = "02",
          TypeUser = EnumTypeUser.Manager
        };
        list.Add(view);

        //Person 6
        view = new ViewPersonImport()
        {
          Mail = "cleide@jmsoft.com.br",
          Name = "Cleide Muterle",
          NameCompany = "Test",
          NameSchooling = "Graduate",
          Password = "1234",
          Phone = "05432025412",
          Registration = 1,
          StatusUser = EnumStatusUser.Enabled,
          DateAdm = DateTime.Parse("1995-01-01"),
          DateBirth = DateTime.Parse("1990-05-11"),
          Document = "05",
          TypeUser = EnumTypeUser.Employee
        };
        list.Add(view);

        var result = this.serviceIntegration.ImportPerson(list);

        this.serviceIntegration.UpdateManager();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void ManagerTest()
    {
      try
      {
        base.Init();
        servicePerson._user = base.baseUser;
        var user = servicePerson.GetAll(p => p.Mail == "ariel@jmsoft.com.br").FirstOrDefault();
        var manager = servicePerson.GetAll(p => p.Mail == "juremir@jmsoft.com.br").FirstOrDefault();
        user.Manager = manager;
        servicePerson.Update(user, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void ListPersonsTest()
    {
      try
      {
        base.Init();

        var account = this.serviceAccount.GeAccount(p => p.Name == "Support")._id;
        var baseUser = new BaseUser()
        {
          _idAccount = account
        };
        base.Init();
        serviceIntegration = new ServiceIntegration(base.context);
        serviceIntegration.SetUser(baseUser);

        var list = new List<ViewPersonImport>();
        long i = 0;
        for (i = 0; i < 100; i++)
        {
          var nametest = "test " + i;
          list.Add(new ViewPersonImport()
          {
            Mail = nametest + "@jmsoft.com.br",
            Name = nametest,
            NameCompany = "Test",
            NameManager = "Juremir Milani",
            NameSchooling = "Posgraduate",
            Password = "1234",
            Phone = "05432025412",
            Registration = 1,
            StatusUser = EnumStatusUser.Enabled,
            DateAdm = DateTime.Parse("2012-01-01"),
            DateBirth = DateTime.Parse("1993-05-11"),
            Document = nametest,
            DocumentManager = "02",
            TypeUser = EnumTypeUser.Employee
          });
        }

        var result = this.serviceIntegration.ImportPerson(list);

        this.serviceIntegration.UpdateManager();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }

}
