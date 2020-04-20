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
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;

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
    private readonly ServiceGeneric<Parameter> serviceParameter;
    private readonly ServiceGeneric<Newsletter> serviceNewsletter;
    private readonly ServiceGeneric<NewsletterRead> serviceNewsletterRead;
    private readonly IServicePerson serviceIPerson;
    private readonly string pathSignalr;

    #region Constructor
    public ServiceAuthentication(DataContext context, DataContext contextLog, IServiceControlQueue serviceControlQueue, string _pathSignalr)
    {
      try
      {
        serviceTermsOfService = new ServiceTermsOfService(context);
        serviceAccount = new ServiceGeneric<Account>(context);
        serviceLog = new ServiceLog(context, contextLog);
        servicePerson = new ServicePerson(context, contextLog, serviceControlQueue, _pathSignalr);
        serviceUser = new ServiceUser(context, contextLog);
        serviceDictionarySystem = new ServiceDictionarySystem(context);
        serviceParameter = new ServiceGeneric<Parameter>(context);
        serviceIPerson = new ServicePerson(context, contextLog, serviceControlQueue, _pathSignalr);
        serviceNewsletter = new ServiceGeneric<Newsletter>(context);
        serviceNewsletterRead = new ServiceGeneric<NewsletterRead>(context);
        pathSignalr = _pathSignalr;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Authentication Controller
    public string AlterContract(string idperson, string _idAccount)
    {
      try
      {
        var person = servicePerson.GetAllFreeNewVersion(p => p._id == idperson && p._idAccount == _idAccount).Result.FirstOrDefault();
        var account = serviceAccount.GetAllFreeNewVersion(p => p._id == person._idAccount).Result.FirstOrDefault();
        var mail = person.User?.Mail;
        if (mail == null)
          mail = "";

        // Token
        Claim[] claims = new[]
        {
        new Claim(ClaimTypes.Name, person.User.Name),
        new Claim(ClaimTypes.Hash, person._idAccount),
        new Claim(ClaimTypes.Email, mail),
        new Claim(ClaimTypes.NameIdentifier, account.Name),
        new Claim(ClaimTypes.UserData, person.User._id),
        new Claim(ClaimTypes.Actor, idperson),
        };
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "localhost",
            audience: "localhost",
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)), SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewPerson Authentication(ViewAuthentication userLogin, bool manager)
    {
      try
      {
        if (string.IsNullOrEmpty(userLogin.Mail))
        {
          throw new Exception("MSG1");
        }
        if (string.IsNullOrEmpty(userLogin.Password))
        {
          throw new Exception("MSG2");
        }
        User user = null;
        if (userLogin.Mail.IndexOf("@maristas.org.br") != -1 || userLogin.Mail.IndexOf("@pucrs.br") != -1)
        {
          GetMaristasAsync(userLogin.Mail, userLogin.Password);
          user = serviceUser.GetFreeNewVersion(p => p.Mail == userLogin.Mail & p.Status == EnumStatus.Enabled).Result;
          if (user == null)
            throw new Exception("User not authorized!");
        }
        else
        {
          if (userLogin.Mail.IndexOf("@unimednordesters.com.br") != -1 || userLogin.Mail.IndexOf("@") == -1)
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
            user = serviceUser.GetFreeNewVersion(p => p.Mail == userLogin.Mail && p.Password == EncryptServices.GetMD5Hash(userLogin.Password)).Result;
            if (user == null)
            {
              throw new Exception("User/Password invalid!");
            }
          }
        }
        if (manager)
        {
          return Authentication(user, true);
        }
        else
        {
          return AuthenticationIntegration(user);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public ViewPerson AuthenticationV2(ViewAuthentication userLogin, EnumTypeAuth typeauth)
    {
      try
      {
        if (string.IsNullOrEmpty(userLogin.Mail))
        {
          throw new Exception("mail_empty");
        }
        if (string.IsNullOrEmpty(userLogin.Password))
        {
          throw new Exception("password_empty");
        }
        User user = null;
        var list = serviceUser.GetAllFreeNewVersion(p => p.Mail == userLogin.Mail
        && p.Status == EnumStatus.Enabled).Result;
        if (list.Count > 1)
          throw new Exception("mailmany");
        else if (list.Count == 1)
          user = list.FirstOrDefault();
        else
        {
          if (userLogin.Mail != null)
            userLogin.Mail = userLogin.Mail.Replace(".", "").Replace("-", "").Trim();

          user = serviceUser.GetAllFreeNewVersion(p => p.Document == userLogin.Mail && p.Status == EnumStatus.Enabled).Result.FirstOrDefault();
          if (user == null)
          {
            user = serviceUser.GetAllFreeNewVersion(p => p.Nickname == userLogin.Mail && p.Status == EnumStatus.Enabled).Result.FirstOrDefault();
            if (user == null)
              throw new Exception("user_notfound");
          }
        }

        if (user.Status == EnumStatus.Disabled)
          throw new Exception("user_disabled");

        var counttype = servicePerson.CountFreeNewVersion(p => p.User._id == user._id && p.TypeUser == EnumTypeUser.Administrator).Result;

        if (counttype == 0 && user.Mail != null && (user._idAccount == "5b7c752468e3f81bb876dcdb") || (user._idAccount == "5be5db1f14dc3fb08f5e5ccf"))
          GetMaristasAsync(user.Mail, userLogin.Password);
        else if (counttype == 0 && user.Nickname != null && user._idAccount == "5b91299a17858f95ffdb79f6")
          GetUnimedAsync(user.Nickname, userLogin.Password);
        else
        {
          if (user.Password != EncryptServices.GetMD5Hash(userLogin.Password))
            throw new Exception("password_invalid");
        }

        if (typeauth == EnumTypeAuth.Default)
        {
          return Authentication(user, true);
        }
        else if (typeauth == EnumTypeAuth.Integration)
        {
          return AuthenticationIntegration(user);
        }
        else if (typeauth == EnumTypeAuth.Mobile)
        {
          return AuthenticationMobile(user);
        }
        else
        {
          return null;
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region Authentication Internal
    // Valida autenticação para Manager
    public ViewPerson Authentication(User user, bool logRegistration)
    {
      try
      {
        BaseUser _user = new BaseUser()
        {
          _idAccount = user._idAccount
        };
        serviceIPerson.SetUser(_user);
        serviceTermsOfService.SetUser(_user);
        serviceTermsOfService._user = _user;
        ViewListTermsOfService date = serviceTermsOfService.GetTerm();
        int countterm = 0;
        Account account = serviceAccount.GetFreeNewVersion(p => p._id == user._idAccount).Result;
        if ((account.InfoClient != null) && (account?.InfoClient != string.Empty))
        {
          countterm = 1;
        }
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
          ShowSalary = user.ShowSalary,
          NameAccount = account?.Name,
          TermOfService = date == null ? true : term == null ? false : true,
          ExistsTermOfService = countterm == 0 ? false : true,
          ViewLO = serviceParameter.GetFreeNewVersion(p => p._idAccount == user._idAccount && p.Key == "viewlo").Result?.Content,
          GoalProcess = serviceParameter.GetFreeNewVersion(p => p._idAccount == user._idAccount && p.Key == "goalProcess").Result?.Content,
          MeritocracyProcess = serviceParameter.GetFreeNewVersion(p => p._idAccount == user._idAccount && p.Key == "meritocracyProcess").Result?.Content,
          ShowAutoManager = serviceParameter.GetFreeNewVersion(p => p._idAccount == user._idAccount && p.Key == "showAutoManager").Result?.Content,
          ShowSalaryScaleManager = serviceParameter.GetFreeNewVersion(p => p._idAccount == user._idAccount && p.Key == "showSalaryScaleManager").Result?.Content,
          DictionarySystem = serviceDictionarySystem.GetAllFreeNewVersion(p => p._idAccount == _user._idAccount).Result,
        };
        person.Contracts = servicePerson.GetAllFreeNewVersion(p => p.User._id == user._id && p.StatusUser != EnumStatusUser.Disabled).Result
          .Select(x => new ViewContract()
          {
            IdPerson = x._id,
            TypeUser = x.TypeUser,
            Registration = x.Registration,
            _idAccount = x._idAccount,
            NameAccount = serviceAccount.GetFreeNewVersion(p => p._id == x._idAccount).Result?.Name,
            Occupation = x.Occupation == null ? string.Empty : x.Occupation.Name
          }).ToList();

        var newsletter = serviceNewsletter.GetAllFreeNewVersion(p => p.Enabled == true).Result.Select(p => p._id).ToList();
        var newsletterread = serviceNewsletterRead.GetAllFreeNewVersion(p => p._idUser == person.IdUser).Result.Select(p => p._idNewsletter);
        foreach (var item in newsletter.Where(p => !newsletterread.Contains(p)).ToList())
        {
          var i = serviceNewsletterRead.InsertNewVersion(new NewsletterRead(){
            _idNewsletter = item,
            _idUser = person.IdUser,
            DontShow = false,
            ReadDate = DateTime.Now
          });
        };
        serviceLog.SetUser(_user);
        if (logRegistration)
          Task.Run(() => LogSave(person.Contracts[0].IdPerson, EnumTypeAuth.Default));

        List<ViewContract> per = person?.Contracts;
        if (per.Count() > 0)
        {
          ViewContract perT = per.FirstOrDefault();
          if ((perT.TypeUser == EnumTypeUser.Manager) || (perT.TypeUser == EnumTypeUser.ManagerHR))
          {
            List<_ViewList> idmanagers = new List<_ViewList>
            {
              new _ViewList() { _id = perT.IdPerson }
            };
            person.Team = serviceIPerson.GetFilterPersons(idmanagers);
          }
        }
        var mail = user.Mail;
        if (mail == null)
          mail = "";

        // Token
        Claim[] claims = new[]
        {
        new Claim(ClaimTypes.Name, person.Name),
        new Claim(ClaimTypes.Hash, person.IdAccount),
        new Claim(ClaimTypes.Email,mail ),
        new Claim(ClaimTypes.NameIdentifier, person.NameAccount),
        new Claim(ClaimTypes.UserData, person.IdUser),
        new Claim(ClaimTypes.Actor, person.Contracts.FirstOrDefault().IdPerson)
        };
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "localhost",
            audience: "localhost",
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)), SecurityAlgorithms.HmacSha256)
        );
        person.Token = new JwtSecurityTokenHandler().WriteToken(token);
        return person;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewPerson AuthenticationMobile(User user)
    {
      try
      {
        BaseUser _user = new BaseUser()
        {
          _idAccount = user._idAccount
        };
        serviceIPerson.SetUser(_user);
        serviceTermsOfService.SetUser(_user);
        serviceTermsOfService._user = _user;
        ViewListTermsOfService date = serviceTermsOfService.GetTerm();
        int countterm = 0;
        Account account = serviceAccount.GetFreeNewVersion(p => p._id == user._idAccount).Result;
        if ((account.InfoClient != null) && (account?.InfoClient != string.Empty))
        {
          countterm = 1;
        }
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
          NameAccount = account?.Name,
          ShowSalary = user.ShowSalary,
          TermOfService = date == null ? true : term == null ? false : true,
          ExistsTermOfService = countterm == 0 ? false : true,
          ViewLO = serviceParameter.GetFreeNewVersion(p => p._idAccount == user._idAccount && p.Key == "viewlo").Result?.Content,
          GoalProcess = serviceParameter.GetFreeNewVersion(p => p._idAccount == user._idAccount && p.Key == "goalProcess").Result?.Content,
          MeritocracyProcess = serviceParameter.GetFreeNewVersion(p => p._idAccount == user._idAccount && p.Key == "meritocracyProcess").Result?.Content,
          ShowAutoManager = serviceParameter.GetFreeNewVersion(p => p._idAccount == user._idAccount && p.Key == "showAutoManager").Result?.Content,
          ShowSalaryScaleManager = serviceParameter.GetFreeNewVersion(p => p._idAccount == user._idAccount && p.Key == "showSalaryScaleManager").Result?.Content,
          DictionarySystem = serviceDictionarySystem.GetAllFreeNewVersion(p => p._idAccount == _user._idAccount).Result,
        };
        person.Contracts = servicePerson.GetAllFreeNewVersion(p => p.User._id == user._id && p.StatusUser != EnumStatusUser.Disabled).Result
          .Select(x => new ViewContract()
          {
            IdPerson = x._id,
            TypeUser = x.TypeUser,
            Registration = x.Registration,
            _idAccount = x._idAccount,
            NameAccount = serviceAccount.GetFreeNewVersion(p => p._id == x._idAccount).Result?.Name,
            Occupation = x.Occupation == null ? string.Empty : x.Occupation.Name
          }).ToList();

        serviceLog.SetUser(_user);
        Task.Run(() => LogSave(person.Contracts[0].IdPerson, EnumTypeAuth.Mobile));

        List<ViewContract> per = person?.Contracts;
        if (per.Count() > 0)
        {
          ViewContract perT = per.FirstOrDefault();
          if ((perT.TypeUser == EnumTypeUser.Manager) || (perT.TypeUser == EnumTypeUser.ManagerHR))
          {
            List<_ViewList> idmanagers = new List<_ViewList>
            {
              new _ViewList() { _id = perT.IdPerson }
            };
            person.Team = serviceIPerson.GetFilterPersons(idmanagers);
          }
        }
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
            expires: DateTime.Now.AddYears(2),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)), SecurityAlgorithms.HmacSha256)
        );
        person.Token = new JwtSecurityTokenHandler().WriteToken(token);
        return person;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    // Valida autenticação para IntegrationServer
    public ViewPerson AuthenticationIntegration(User user)
    {
      try
      {
        BaseUser _user = new BaseUser()
        {
          _idAccount = user._idAccount
        };
        serviceIPerson.SetUser(_user);
        serviceTermsOfService.SetUser(_user);
        serviceTermsOfService._user = _user;
        Account account = serviceAccount.GetFreeNewVersion(p => p._id == user._idAccount).Result;
        ViewPerson person = new ViewPerson()
        {
          IdUser = user._id,
          Name = user.Name,
          IdAccount = user._idAccount,
          ChangePassword = user.ChangePassword,
          Photo = user.PhotoUrl,
          NameAccount = account?.Name,
          TermOfService = false,
          DictionarySystem = null,
          ExistsTermOfService = false
        };
        // Token
        Claim[] claims = new[]
        {
        new Claim(ClaimTypes.Name, person.Name),
        new Claim(ClaimTypes.Hash, person.IdAccount),
        new Claim(ClaimTypes.Email, user.Mail),
        new Claim(ClaimTypes.NameIdentifier, person.NameAccount),
        new Claim(ClaimTypes.UserData, person.IdUser),
        new Claim(ClaimTypes.Actor, person.IdUser)
        };
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "localhost",
            audience: "localhost",
            claims: claims,
            expires: DateTime.Now.AddYears(10),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)), SecurityAlgorithms.HmacSha256)
        );
        person.Token = new JwtSecurityTokenHandler().WriteToken(token);
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
          _idAccount = loginPerson._idAccount,
          NameAccount = serviceAccount.GetFreeNewVersion(p => p._id == loginPerson._idAccount).Result?.Name,
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

    #region Private
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
          HttpResponseMessage result = client.PostAsync("wspucsede/WService.asmx/ValidateUser", content).Result;
          if (result.StatusCode != System.Net.HttpStatusCode.OK)
          {
            throw new Exception("api_invalid");
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    private void GetUnimedAsync(string login, string passwordClient)
    {
      try
      {
        string username = "apiadv";
        string password = "ad6072616b467db08f60918070e03622" + DateTime.Now.AddHours(-3).ToString("ddMMyyyyHHmm");
        //string password = "ad6072616b467db08f60918070e03622" + DateTime.Now.ToString("ddMMyyyyHHmm");
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
          HttpResponseMessage result = client.PostAsync("/", content).Result;
          if (result.StatusCode != System.Net.HttpStatusCode.OK)
          {
            throw new Exception(result.ReasonPhrase);
          }
          //Console.Write(result.Content.ReadAsStringAsync().Result);
          //Console.Write(result.Content.ReadAsStringAsync());
          //Console.Write(result.Content);
          ViewUnimedStatusAuthentication status = JsonConvert.DeserializeObject<ViewUnimedStatusAuthentication>(result.Content.ReadAsStringAsync().Result);
          if (status.Status == false)
          {
            throw new Exception("api_invalid");
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    private void LogSave(string idPerson, EnumTypeAuth type)
    {
      try
      {
        string local = "";
        if (type == EnumTypeAuth.Default)
          local = "Web";
        else if (type == EnumTypeAuth.Mobile)
          local = "Mobile";
        else if (type == EnumTypeAuth.Integration)
          local = "Integration";

        serviceLog.NewLog(new ViewLog()
        {
          Description = "Login " + local,
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
