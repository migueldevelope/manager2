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
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Manager.Views.Enumns;

namespace Manager.Services.Auth
{
  public class ServiceAuthentication : IServiceAuthentication
  {

    #region Constructior + Fields
    private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";
    private readonly ServicePerson personService;
    private readonly ServiceUser userService;
    private readonly ServiceLog logService;
    private ServiceGeneric<Account> accountService;
    private ServiceDictionarySystem dictionarySystemService;
    private DataContext _context;

    public ServiceAuthentication(DataContext context)
    {
      try
      {
        accountService = new ServiceGeneric<Account>(context);
        logService = new ServiceLog(context);
        personService = new ServicePerson(context);
        userService = new ServiceUser(context);
        dictionarySystemService = new ServiceDictionarySystem(context);
        _context = context;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Old Authentication
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
        //companyService.SetUser(_user);
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


        person.Contracts = new List<ViewContract>();
        foreach (var item in persons)
        {
          var view = new ViewContract()
          {
            IdPerson = item._id,
            Logo = item.Company.Logo,
            TypeUser = item.TypeUser,
            Registration = item.Registration.ToString()
          };
          if (item.Occupation != null)
            view.Occupation = item.Occupation.Name;
          person.Contracts.Add(view);
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


        person.Contracts = new List<ViewContract>();
        foreach (var item in persons)
        {
          var view = new ViewContract()
          {
            IdPerson = item._id,
            Logo = item.Company.Logo,
            TypeUser = item.TypeUser,
            Registration = item.Registration.ToString()
          };
          if (item.Occupation != null)
            view.Occupation = item.Occupation.Name;
          person.Contracts.Add(view);
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

        //logService = new ServiceLog(_context);
        var log = new ViewLog()
        {
          Description = "Login",
          Local = "Authentication",
          Person = persons.FirstOrDefault()
        };
        //logService.NewLog(log);

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
        var user = userService.GetAuthentication(mail, EncryptServices.GetMD5Hash(password));
        if (user == null)
          throw new ServiceException(new BaseUser() { _idAccount = "000000000000000000000000" }, new Exception("Usuário/Senha inválido!"), _context);


        var persons = personService.GetAuthentication(p => p.User._id == user._id).ToList();
        var _user = new BaseUser { _idAccount = user._idAccount };
        //companyService.SetUser(_user);
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


        person.Contracts = new List<ViewContract>();
        foreach (var item in persons)
        {
          var view = new ViewContract()
          {
            IdPerson = item._id,
            Logo = item.Company.Logo,
            TypeUser = item.TypeUser,
            Registration = item.Registration.ToString()
          };
          if (item.Occupation != null)
            view.Occupation = item.Occupation.Name;
          person.Contracts.Add(view);
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
        var user = userService.GetAuthentication(mail, password);
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


        person.Contracts = new List<ViewContract>();
        foreach (var item in persons)
        {
          var view = new ViewContract()
          {
            IdPerson = item._id,
            Logo = item.Company.Logo,
            TypeUser = item.TypeUser,
            Registration = item.Registration.ToString()
          };
          if (item.Occupation != null)
            view.Occupation = item.Occupation.Name;
          person.Contracts.Add(view);
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

        //logService = new ServiceLog(_context);

        var log = new ViewLog()
        {
          Description = "Login",
          Local = "Authentication",
          Person = persons.FirstOrDefault()
        };
        //logService.NewLog(log);

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
        var user = personService.GetPerson(idPerson);
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

        //logService = new ServiceLog(_context);

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
        //logService = new ServiceLog(_context);
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
    #endregion

    #region Authentication
    public ViewPerson Authentication(ViewAuthentication userLogin)
    {
      try
      {
        if (string.IsNullOrEmpty(userLogin.Mail))
          throw new Exception("MSG1");
        if (string.IsNullOrEmpty(userLogin.Password))
          throw new Exception("MSG2");

        User user = null;
        if (userLogin.Mail.IndexOf("@maristas.org.br") != -1 || userLogin.Mail.IndexOf("@pucrs.br") != -1)
        {
          GetMaristasAsync(userLogin.Mail, userLogin.Password);
          user = userService.GetAuthentication(p => p.Mail == userLogin.Mail & p.Status == EnumStatus.Enabled).FirstOrDefault();
          if (user == null)
            throw new Exception("User not authorized!");
        }
        else {
          user = userService.GetAuthentication(userLogin.Mail, EncryptServices.GetMD5Hash(userLogin.Password));
          if (user == null)
            throw new Exception("User/Password invalid!");
        }
        return Authentication(user);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewPerson Authentication(User user)
    {
      try
      {
        BaseUser _user = new BaseUser()
        {
          _idAccount = user._idAccount
        };

        dictionarySystemService.SetUser(_user);
        List<DictionarySystem> listDictionary = dictionarySystemService.GetAllFreeNewVersion(p => p._idAccount == _user._idAccount).Result;

        ViewPerson person = new ViewPerson()
        {
          IdUser = user._id,
          Name = user.Name,
          IdAccount = user._idAccount,
          ChangePassword = user.ChangePassword,
          Photo = user.PhotoUrl,
          NameAccount = accountService.GetFreeNewVersion(p => p._id == user._idAccount).Result.Name,
          DictionarySystem = listDictionary
        };
        List<Person> persons = personService.GetAllFreeNewVersion(p => p.User._id == user._id).Result;

        person.Contracts = new List<ViewContract>();
        foreach (Person item in persons)
        {
          ViewContract view = new ViewContract()
          {
            IdPerson = item._id,
            Logo = item.Company.Logo,
            TypeUser = item.TypeUser,
            Registration = item.Registration.ToString(),
            Occupation = item.Occupation?.Name
          };
          person.Contracts.Add(view);
        }

        LogSave(persons.FirstOrDefault());

        // Token
        Claim[] claims = new[]
        {
        new Claim(ClaimTypes.Name, person.Name),
        new Claim(ClaimTypes.Hash, person.IdAccount),
        new Claim(ClaimTypes.Email, user.Mail),
        new Claim(ClaimTypes.NameIdentifier, person.NameAccount),
        new Claim(ClaimTypes.UserData, person.Contracts.FirstOrDefault().IdPerson)
        };
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "localhost",
            audience: "localhost",
            claims: claims,
            expires: DateTime.Now.AddYears(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)), SecurityAlgorithms.HmacSha256)
        );
        person.Token = new JwtSecurityTokenHandler().WriteToken(token);
        //

        return person;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewPerson AuthenticationMail(Person loginPerson)
    {
      try
      {
        BaseUser _user = new BaseUser()
        {
          _idAccount = loginPerson._idAccount
        };

        ViewPerson person = new ViewPerson()
        {
          IdUser = loginPerson._id,
          Name = loginPerson.User.Name,
          IdAccount = loginPerson._idAccount,
          ChangePassword = EnumChangePassword.No,
          Photo = string.Empty,
          NameAccount = string.Empty,
          DictionarySystem = new List<DictionarySystem>()
        };
        ViewContract view = new ViewContract()
        {
          IdPerson = loginPerson._id,
          Logo = string.Empty,
          TypeUser = loginPerson.TypeUser,
          Registration = loginPerson.Registration,
          Occupation = loginPerson.Occupation?.Name
        };
        person.Contracts.Add(view);

        // Token
        Claim[] claims = new[]
        {
        new Claim(ClaimTypes.Name, person.Name),
        new Claim(ClaimTypes.Hash, person.IdAccount),
        new Claim(ClaimTypes.Email, loginPerson.User.Mail),
        new Claim(ClaimTypes.NameIdentifier, person.NameAccount),
        new Claim(ClaimTypes.UserData, person.Contracts.FirstOrDefault().IdPerson)
        };
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "localhost",
            audience: "localhost",
            claims: claims,
            expires: DateTime.Now.AddYears(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)), SecurityAlgorithms.HmacSha256)
        );
        person.Token = new JwtSecurityTokenHandler().WriteToken(token);
        //

        return person;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async void GetMaristasAsync(string login, string senha)
    {
      try
      {
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri("http://integracoes.maristas.org.br");
          var content = new StringContent("login=" + login + "&senha=" + senha);
          content.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
          client.DefaultRequestHeaders.Add("ContentType", "application/x-www-form-urlencoded");
          HttpResponseMessage result = await client.PostAsync("wspucsede/WService.asmx/ValidateUser", content);
          if (result.StatusCode != System.Net.HttpStatusCode.OK)
            throw new Exception("User/Password invalid!");
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

  }
}
