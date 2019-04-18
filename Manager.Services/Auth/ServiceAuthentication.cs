﻿using Manager.Core.Base;
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
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Manager.Views.Enumns;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Manager.Services.Auth
{
  public class ServiceAuthentication : IServiceAuthentication
  {
    private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";
    private readonly ServicePerson servicePerson;
    private readonly ServiceUser serviceUser;
    private readonly ServiceLog serviceLog;
    private readonly ServiceGeneric<Account> serviceAccount;
    private readonly ServiceDictionarySystem serviceDictionarySystem;

    #region Constructor
    public ServiceAuthentication(DataContext context, DataContext contextLog)
    {
      try
      {
        serviceAccount = new ServiceGeneric<Account>(context);
        serviceLog = new ServiceLog(context, contextLog);
        servicePerson = new ServicePerson(context, contextLog);
        serviceUser = new ServiceUser(context, contextLog);
        serviceDictionarySystem = new ServiceDictionarySystem(context);
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
          user = serviceUser.GetFreeNewVersion(p => p.Mail == userLogin.Mail & p.Status == EnumStatus.Enabled).Result;
          if (user == null)
            throw new Exception("User not authorized!");
        }
        else if (userLogin.Mail.IndexOf("@unimednordesters.com.br") != -1)
        {

          GetUnimedAsync(userLogin.Mail, userLogin.Password);
          user = serviceUser.GetFreeNewVersion(p => p.Mail == userLogin.Mail & p.Status == EnumStatus.Enabled).Result;
          if (user == null)
            throw new Exception("User not authorized!");
        }
        else
        {
          user = serviceUser.GetFreeNewVersion(p => p.Mail == userLogin.Mail && p.Password == EncryptServices.GetMD5Hash(userLogin.Password)).Result;
          if (user == null)
            throw new Exception("User/Password invalid!");
        }
        return Authentication(user, true);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewPerson Authentication(User user, bool registerLog)
    {
      try
      {
        BaseUser _user = new BaseUser()
        {
          _idAccount = user._idAccount
        };

        serviceDictionarySystem.SetUser(_user);
        ViewPerson person = new ViewPerson()
        {
          IdUser = user._id,
          Name = user.Name,
          IdAccount = user._idAccount,
          ChangePassword = user.ChangePassword,
          Photo = user.PhotoUrl,
          NameAccount = serviceAccount.GetFreeNewVersion(p => p._id == user._idAccount).Result.Name,
          DictionarySystem = null
        };
        person.Contracts = servicePerson.GetAllFreeNewVersion(p => p.User._id == user._id).Result
          .Select(x => new ViewContract()
          {
            IdPerson = x._id,
            Logo = x.Company.Logo,
            TypeUser = x.TypeUser,
            Registration = x.Registration,
            Occupation = x.Occupation == null ? string.Empty : x.Occupation.Name
          }).ToList();

        if (registerLog)
        {
          serviceLog.SetUser(_user);
          Task.Run(() => LogSave(servicePerson.GetFreeNewVersion(p => p.User._id == user._id).Result));
        }

        person.DictionarySystem = serviceDictionarySystem.GetAllFreeNewVersion(p => p._idAccount == _user._idAccount).Result;
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
        person.Contracts = new List<ViewContract>
        {
          view
        };

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
    #endregion

    #region private
    private async void GetMaristasAsync(string login, string password)
    {
      try
      {
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri("http://integracoes.maristas.org.br");
          var content = new StringContent("login=" + login + "&senha=" + password);
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
    private async void GetUnimedAsync(string login, string passwordClient)
    {
      try
      {
        string username = "apiadv";
        string password = "ad6072616b467db08f60918070e03622" + DateTime.Now.ToString("ddMMyyyyHHmm");
        string password2 = EncryptServices.GetMD5HashTypeTwo(password).ToLower();

        using (HttpClient client = new HttpClient())
        {

          client.DefaultRequestHeaders.Add("Autorization", "Basic " + password);
          client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(new UTF8Encoding().GetBytes(username + ":" + password2)));
          client.BaseAddress = new Uri("https://apip1.unimednordesters.com.br");

          var data = new
          {
            channel = "apiadv",
            parametros = new
            {
              usuario = login,
              senha = passwordClient
            }
          };
          string json = JsonConvert.SerializeObject(data);
          StringContent content = new StringContent(json);
          content.Headers.ContentType.MediaType = "application/json";
          client.DefaultRequestHeaders.Add("ContentType", "application/json");
          HttpResponseMessage result = client.PostAsync("/", content).Result;
          //var resultContent = result.Content.ReadAsStringAsync().Result;

          if (result.StatusCode != System.Net.HttpStatusCode.OK)
            throw new Exception("User/Password invalid!");
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async Task LogSave(Person user)
    {
      try
      {
        serviceLog.NewLog(new ViewLog()
        {
          Description = "Login",
          Local = "Authentication",
          _idPerson = user._id
        });
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

  }
}
