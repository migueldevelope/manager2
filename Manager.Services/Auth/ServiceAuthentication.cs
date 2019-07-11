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
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Manager.Views.Enumns;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Manager.Core.BusinessModel;
using Manager.Views.CustomClient;

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
    private readonly ServiceTermsOfService serviceTermsOfService;

    #region Constructor
    public ServiceAuthentication(DataContext context, DataContext contextLog)
    {
      try
      {
        serviceTermsOfService = new ServiceTermsOfService(context);
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

    public  string AlterContract(string idperson)
    {
      try
      {
        var person = servicePerson.GetAllFreeNewVersion(p => p._id == idperson).Result.FirstOrDefault();
        var account = serviceAccount.GetAllFreeNewVersion(p => p._id == person._idAccount).Result.FirstOrDefault();

        // Token
        Claim[] claims = new[]
        {
        new Claim(ClaimTypes.Name, person.User.Name),
        new Claim(ClaimTypes.Hash, person._idAccount),
        new Claim(ClaimTypes.Email, person.User.Mail),
        new Claim(ClaimTypes.NameIdentifier, account.Name),
        new Claim(ClaimTypes.UserData, person.User._id),
        new Claim(ClaimTypes.Actor, idperson),
        };
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "localhost",
            audience: "localhost",
            claims: claims,
            expires: DateTime.Now.AddDays(2),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)), SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public  ViewPerson Authentication(ViewAuthentication userLogin)
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
        //else if (userLogin.Mail.IndexOf("@unimednordesters.com.br") != -1)
        //{
        //  GetUnimedAsync(userLogin.Mail, userLogin.Password);
        //  // Localizar pelo apelido
        //  user = serviceUser.GetFreeNewVersion(p => p.Nickname == userLogin.Mail & p.Status == EnumStatus.Enabled).Result;
        //  if (user == null)
        //    // Localizar pelo e-mail
        //    user = serviceUser.GetFreeNewVersion(p => p.Mail == userLogin.Mail & p.Status == EnumStatus.Enabled).Result;
        //  if (user == null)
        //    throw new Exception("User not authorized!");
        //}
        else if (userLogin.Mail.IndexOf("@unimednordesters.com.br") != -1 || userLogin.Mail.IndexOf("@") == -1)
        {
          // Localizar usuário por e-mail
          user = serviceUser.GetFreeNewVersion(p => p.Mail == userLogin.Mail).Result;
          if (user == null)
          {
            // Localizar usuário por apelido
            user = serviceUser.GetFreeNewVersion(p => p.Nickname == userLogin.Mail.Replace("@unimednordesters.com.br", string.Empty)).Result;
            if (user == null)
            {
              throw new Exception("User not found!");
            }
          }
          if (user.Status == EnumStatus.Disabled)
          {
            throw new Exception("User disabled!");
          }
          GetUnimedAsync(user.Nickname, userLogin.Password);
        }
        else
        {
          user =  serviceUser.GetFreeNewVersion(p => p.Mail == userLogin.Mail && p.Password == EncryptServices.GetMD5Hash(userLogin.Password)).Result;
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
        serviceTermsOfService.SetUser(_user);
        serviceTermsOfService._user = _user;

        var date = serviceTermsOfService.GetTerm();

        UserTermOfService term = null;
        UserTermOfService viewDate = null;

        if (date != null)
        {
          viewDate = new UserTermOfService()
          {
            Date = date?.Date,
            _idTermOfService = date?._id
          };

          if (user.UserTermOfServices != null)
          {
            foreach (var item in user.UserTermOfServices)
            {
              if (viewDate._idTermOfService == item._idTermOfService)
              {
                term = viewDate;
              }
            }
          }
        }


        serviceDictionarySystem.SetUser(_user);
        ViewPerson person = new ViewPerson()
        {
          IdUser = user._id,
          Name = user.Name,
          IdAccount = user._idAccount,
          ChangePassword = user.ChangePassword,
          Photo = user.PhotoUrl,
          NameAccount = serviceAccount.GetFreeNewVersion(p => p._id == user._idAccount).Result.Name,
          TermOfService = date == null ? true : term == null ? false : true,
          DictionarySystem = null
        };

        person.Contracts = servicePerson.GetAllFreeNewVersion(p => p.User._id == user._id).Result
          .Select(x => new ViewContract()
          {
            IdPerson = x._id,
            TypeUser = x.TypeUser,
            Registration = x.Registration,
            Occupation = x.Occupation == null ? string.Empty : x.Occupation.Name
          }).ToList();

        if (registerLog)
        {
          serviceLog.SetUser(_user);
          Task.Run(() => LogSave(person.Contracts[0].IdPerson));
        }



        person.DictionarySystem = serviceDictionarySystem.GetAllFreeNewVersion(p => p._idAccount == _user._idAccount).Result;
        // Token
        Claim[] claims = new[]
        {
        new Claim(ClaimTypes.Name, person.Name),
        new Claim(ClaimTypes.Hash, person.IdAccount),
        new Claim(ClaimTypes.Email, user.Mail),
        new Claim(ClaimTypes.NameIdentifier, person.NameAccount),
        new Claim(ClaimTypes.UserData, person.IdUser),
        new Claim(ClaimTypes.Actor, person.Contracts.FirstOrDefault().IdPerson)
        };
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "localhost",
            audience: "localhost",
            claims: claims,
            expires: DateTime.Now.AddDays(2),
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
    public string AuthenticationMail(Person loginPerson)
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
        new Claim(ClaimTypes.UserData, person.IdUser),
        new Claim(ClaimTypes.Actor, person.Contracts.FirstOrDefault().IdPerson)
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
        return person.Token;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region private
    private void GetMaristasAsync(string login, string password)
    {
      try
      {
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri("http://integracoes.maristas.org.br");
          var content = new StringContent("login=" + login + "&senha=" + password);
          content.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
          client.DefaultRequestHeaders.Add("ContentType", "application/x-www-form-urlencoded");
          HttpResponseMessage result =  client.PostAsync("wspucsede/WService.asmx/ValidateUser", content).Result;
          if (result.StatusCode != System.Net.HttpStatusCode.OK)
            throw new Exception("User/Password invalid!");
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void GetUnimedAsync(string login, string passwordClient)
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
          //client.BaseAddress = new Uri("https://apip1.unimednordesters.com.br");
          client.BaseAddress = new Uri("https://api.unimednordesters.com.br");

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
          HttpResponseMessage result =  client.PostAsync("/", content).Result;
          if (result.StatusCode != System.Net.HttpStatusCode.OK)
          {
            throw new Exception(result.ReasonPhrase);
            // throw new Exception("User/Password invalid!");
          }
          ViewUnimedStatusAuthentication status = JsonConvert.DeserializeObject<ViewUnimedStatusAuthentication>(result.Content.ReadAsStringAsync().Result);
          if (status.Status == false)
          {
            throw new Exception("User/Password invalid!");
          }
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void LogSave(string idPerson)
    {
      try
      {
        serviceLog.NewLog(new ViewLog()
        {
          Description = "Login",
          Local = "Authentication",
          _idPerson = idPerson
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
