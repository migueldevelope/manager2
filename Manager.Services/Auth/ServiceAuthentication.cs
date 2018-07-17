using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using System.Linq;
using System;
using MongoDB.Bson;
using Manager.Services.Specific;
using Tools;

namespace Manager.Services.Auth
{
  public class ServiceAuthentication : IServiceAuthentication
  {
    private readonly IServicePerson servicePerson;
    private ServiceGeneric<Account> accountService;
    private IServiceCompany companyService;
    private IServiceLog logService;
    private DataContext _context;

    public ServiceAuthentication(DataContext context, IServiceLog _logService, 
      IServicePerson _servicePerson, IServiceCompany _companyService)
    {
      try
      {
        servicePerson = _servicePerson;
        accountService = new ServiceGeneric<Account>(context);
        companyService = _companyService;
        logService = _logService;
        _context = context;
      }
      catch (Exception)
      {
        //throw new ServiceException(null, e);
      }
    }


    public ViewPerson Authentication(string mail, string password)
    {
      try
      {
        var user = servicePerson.GetAuthentication(mail, EncryptServices.GetMD5Hash(password));
        if (user == null)
          throw new ServiceException(new BaseUser() { _idAccount = "000000000000000000000000" }, new Exception("Usuário/Senha inválido!"), _context);


        var _user = new BaseUser { _idAccount = user._idAccount };
        companyService.SetUser(_user);
        ViewPerson person = new ViewPerson()
        {
          IdPerson = user._id,
          Name = user.Name,
          IdAccount = user._idAccount,
          ChangePassword = user.ChangePassword,
          Photo = user.PhotoUrl,
          TypeUser = user.TypeUser,
          NameAccount = accountService.GetAuthentication(p => p._id == user._idAccount).FirstOrDefault().Name,
          Logo = companyService.GetLogo(user.Company._id.ToString())
        };

        LogSave(user);

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
        var user = servicePerson.GetAuthentication(mail, password);
        if (user == null)
          throw new ServiceException(new BaseUser() { _idAccount = "000000000000000000000000" }, new Exception("Usuário/Senha inválido!"), _context);



        ViewPerson person = new ViewPerson()
        {
          IdPerson = user._id,
          Name = user.Name,
          IdAccount = user._idAccount,
          ChangePassword = user.ChangePassword,
          Photo = user.PhotoUrl,
          TypeUser = user.TypeUser,
          NameAccount = accountService.GetAuthentication(p => p._id == user._idAccount).FirstOrDefault().Name
        };

        var _user = new BaseUser()
        {
          _idAccount = user._idAccount,
          NamePerson = user.Name,
          Mail = user.Mail,
          _idPerson = user._id,
          NameAccount = person.NameAccount
        };

        logService = new ServiceLog(_context);

        var log = new ViewLog()
        {
          Description = "Login",
          Local = "Authentication",
          Person = user
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
        ViewPerson person = new ViewPerson()
        {
          IdPerson = user._id.ToString(),
          Name = user.Name
        };

        var _user = new BaseUser()
        {
          _idAccount = user._idAccount,
          NamePerson = user.Name,
          Mail = user.Mail,
          _idPerson = user._id
        };

        logService = new ServiceLog(_context);

        var log = new ViewLog()
        {
          Description = "Logoff",
          Local = "Authentication",
          Person = user
        };
        logService.NewLog(log);

        return person;
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
          NamePerson = user.Name,
          Mail = user.Mail,
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

  }
}
