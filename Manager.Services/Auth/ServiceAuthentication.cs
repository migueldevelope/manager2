using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using System.Linq;
using System;
using Manager.Services.Specific;
using Tools;
using System.Net.Http;
using Manager.Core.Enumns;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Services.Auth
{
#pragma warning disable 1998
  public class ServiceAuthentication : IServiceAuthentication
  {
    private readonly IServicePerson servicePerson;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<User> userService;
    private ServiceGeneric<Account> accountService;
    private ServiceDictionarySystem dictionarySystemService;
    private IServiceCompany companyService;
    private IServiceLog logService;
    private IServiceUser serviceUser;
    private DataContext _context;

    public ServiceAuthentication(DataContext context, IServiceLog _logService,
      IServicePerson _servicePerson, IServiceCompany _companyService, IServiceUser _serviceUser)
    {
      try
      {
        servicePerson = _servicePerson;
        accountService = new ServiceGeneric<Account>(context);
        companyService = _companyService;
        logService = _logService;
        serviceUser = _serviceUser;
        personService = new ServiceGeneric<Person>(context);
        userService = new ServiceGeneric<User>(context);
        dictionarySystemService = new ServiceDictionarySystem(context);
        _context = context;
      }
      catch (Exception)
      {
        //throw new ServiceException(null, e);
      }
    }




    public ViewPerson AuthenticationMaristas(string mail, string password)
    {
      try
      {
        User user = null;
        if (GetMaristas(mail, password) == "ok")
          user = userService.GetAuthentication(p => p.Mail == mail & p.Status == EnumStatus.Enabled).FirstOrDefault();

        if (user == null)
          throw new ServiceException(new BaseUser() { _idAccount = "000000000000000000000000" }, new Exception("Usuário/Senha inválido!"), _context);

        var persons = personService.GetAuthentication(p => p.User._id == user._id).ToList();

        var _user = new BaseUser { _idAccount = user._idAccount };
        companyService.SetUser(_user);
        dictionarySystemService.SetUser(_user);
        long total = 0;
        var listDictionary = dictionarySystemService.List(ref total, 9999999, 1, "");


        ViewPerson person = new ViewPerson()
        {
          IdUser = user._id,
          Name = user.Name,
          IdAccount = user._idAccount,
          ChangePassword = user.ChangePassword,
          Photo = user.PhotoUrl,
          NameAccount = accountService.GetAuthentication(p => p._id == user._idAccount).FirstOrDefault().Name,
          DictionarySystem = listDictionary
        };

        foreach (var item in persons)
        {
          person.Contracts = new List<ViewContract>();
          person.Contracts.Add(new ViewContract()
          {
            IdPerson = item._id,
            Logo = item.Company.Logo,
            TypeUser = item.TypeUser
          });
        }


        LogSave(persons.FirstOrDefault());

        return person;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewPerson AuthenticationEncryptMaristas(string mail, string password)
    {
      try
      {
        User user = null;
        //if (GetMaristas(mail, password) == "ok")
        user = userService.GetAuthentication(p => p.Mail == mail & p.Status == EnumStatus.Enabled).FirstOrDefault();

        if (user == null)
          throw new ServiceException(new BaseUser() { _idAccount = "000000000000000000000000" }, new Exception("Usuário/Senha inválido!"), _context);


        var persons = personService.GetAuthentication(p => p.User._id == user._id).ToList();

        ViewPerson person = new ViewPerson()
        {
          IdUser = user._id,
          Name = user.Name,
          IdAccount = user._idAccount,
          ChangePassword = user.ChangePassword,
          Photo = user.PhotoUrl,
          NameAccount = accountService.GetAuthentication(p => p._id == user._idAccount).FirstOrDefault().Name
        };

        foreach (var item in persons)
        {
          person.Contracts = new List<ViewContract>();
          person.Contracts.Add(new ViewContract()
          {
            IdPerson = item._id,
            Logo = item.Company.Logo,
            TypeUser = item.TypeUser
          });
        }

        var _user = new BaseUser()
        {
          _idAccount = user._idAccount,
          NamePerson = user.Name,
          Mail = user.Mail,
          _idPerson = user._id,
          NameAccount = person.NameAccount
        };

        dictionarySystemService.SetUser(_user);
        long total = 0;
        var listDictionary = dictionarySystemService.List(ref total, 9999999, 1, "");

        person.DictionarySystem = listDictionary;

        logService = new ServiceLog(_context);

        var log = new ViewLog()
        {
          Description = "Login",
          Local = "Authentication",
          Person = persons.FirstOrDefault()
        };
        logService.NewLog(log);

        return person;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewPerson Authentication(string mail, string password)
    {
      try
      {
        var user = serviceUser.GetAuthentication(mail, EncryptServices.GetMD5Hash(password));
        if (user == null)
          throw new ServiceException(new BaseUser() { _idAccount = "000000000000000000000000" }, new Exception("Usuário/Senha inválido!"), _context);


        var persons = personService.GetAuthentication(p => p.User._id == user._id).ToList();
        var _user = new BaseUser { _idAccount = user._idAccount };
        companyService.SetUser(_user);
        dictionarySystemService.SetUser(_user);
        long total = 0;
        var listDictionary = dictionarySystemService.List(ref total, 9999999, 1, "");


        ViewPerson person = new ViewPerson()
        {
          IdUser = user._id,
          Name = user.Name,
          IdAccount = user._idAccount,
          ChangePassword = user.ChangePassword,
          Photo = user.PhotoUrl,
          NameAccount = accountService.GetAuthentication(p => p._id == user._idAccount).FirstOrDefault().Name,
          DictionarySystem = listDictionary
        };

        foreach (var item in persons)
        {
          person.Contracts = new List<ViewContract>();
          person.Contracts.Add(new ViewContract()
          {
            IdPerson = item._id,
            Logo = item.Company.Logo,
            TypeUser = item.TypeUser
          });
        }

        LogSave(persons.FirstOrDefault());

        return person;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewPerson AuthenticationEncrypt(string mail, string password)
    {
      try
      {
        var user = serviceUser.GetAuthentication(mail, password);
        if (user == null)
          throw new ServiceException(new BaseUser() { _idAccount = "000000000000000000000000" }, new Exception("Usuário/Senha inválido!"), _context);


        var persons = personService.GetAuthentication(p => p.User._id == user._id).ToList();
        ViewPerson person = new ViewPerson()
        {
          IdUser = user._id,
          Name = user.Name,
          IdAccount = user._idAccount,
          ChangePassword = user.ChangePassword,
          Photo = user.PhotoUrl,
          NameAccount = accountService.GetAuthentication(p => p._id == user._idAccount).FirstOrDefault().Name
        };

        foreach (var item in persons)
        {
          person.Contracts = new List<ViewContract>();
          person.Contracts.Add(new ViewContract()
          {
            IdPerson = item._id,
            Logo = item.Company.Logo,
            TypeUser = item.TypeUser
          });
        }

        var _user = new BaseUser()
        {
          _idAccount = user._idAccount,
          NamePerson = user.Name,
          Mail = user.Mail,
          _idPerson = user._id,
          NameAccount = person.NameAccount
        };

        dictionarySystemService.SetUser(_user);
        long total = 0;
        var listDictionary = dictionarySystemService.List(ref total, 9999999, 1, "");

        person.DictionarySystem = listDictionary;

        logService = new ServiceLog(_context);

        var log = new ViewLog()
        {
          Description = "Login",
          Local = "Authentication",
          Person = persons.FirstOrDefault()
        };
        logService.NewLog(log);

        return person;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewPerson Logoff(string idPerson)
    {
      try
      {
        var user = servicePerson.GetPerson(idPerson);
        //ViewPerson person = new ViewPerson()
        //{
        //  IdPerson = user._id.ToString(),
        //  Name = user.Name
        //};

        //var _user = new BaseUser()
        //{
        //  _idAccount = user._idAccount,
        //  NamePerson = user.Name,
        //  Mail = user.User.Mail,
        //  _idPerson = user._id
        //};

        logService = new ServiceLog(_context);

        var log = new ViewLog()
        {
          Description = "Logoff",
          Local = "Authentication",
          Person = user
        };
        logService.NewLog(log);

        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async void LogSave(Person user)
    {
      try
      {
        var _user = new BaseUser()
        {
          _idAccount = user._idAccount,
          NamePerson = user.User.Name,
          Mail = user.User.Mail,
          _idPerson = user._id
        };
        logService = new ServiceLog(_context);
        var log = new ViewLog()
        {
          Description = "Login",
          Local = "Authentication",
          Person = user
        };
        logService.NewLog(log);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string GetMaristas(string login, string senha)
    {
      try
      {
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri("http://integracoes.maristas.org.br");
          var content = new StringContent("login=" + login + "&senha=" + senha);
          content.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
          client.DefaultRequestHeaders.Add("ContentType", "application/x-www-form-urlencoded");
          var result = client.PostAsync("wspucsede/WService.asmx/ValidateUser", content).Result;
          if (result.StatusCode == System.Net.HttpStatusCode.OK)
            return "ok";
          else
            return "error";
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
#pragma warning restore 1998
}
