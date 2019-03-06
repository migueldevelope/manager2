using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessList;
using Manager.Views.BusinessNew;
using Manager.Views.Enumns;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Tools;

namespace Manager.Services.Specific
{
  public class ServiceAccount : Repository<Account>, IServiceAccount
  {
    private readonly ServiceGeneric<Account> accountService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<User> userService;
    private readonly ServiceGeneric<Company> companyService;
    private readonly ServiceInfra infraService;
    private readonly ServiceGeneric<Parameter> parameterService;
    private IServiceLog logService;

    #region Constructor
    public ServiceAccount(DataContext context)
      : base(context)
    {
      try
      {
        accountService = new ServiceGeneric<Account>(context);
        personService = new ServiceGeneric<Person>(context);
        userService = new ServiceGeneric<User>(context);
        companyService = new ServiceGeneric<Company>(context);
        infraService = new ServiceInfra(context);
        parameterService = new ServiceGeneric<Parameter>(context);
        logService = new ServiceLog(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    #endregion

    #region New account
    public async Task<string> NewAccount(ViewNewAccount view)
    {
      try
      {
        // Criar uma nova conta
        Account account = new Account()
        {
          Name = view.NameAccount,
          Status = EnumStatus.Enabled
        };
        account = accountService.InsertAccountNewVersion(account).Result;
        accountService._user = new BaseUser()
        {
          _idAccount = account._id,
          NameAccount = account.Name,
          Mail = view.Mail,
          NamePerson = view.NameCompany
        };
        // Criar o usuário para autenticação
        userService._user = accountService._user;
        User user = new User()
        {
          Name = view.NameAccount,
          Mail = view.Mail,
          Password = EncryptServices.GetMD5Hash(view.Password)
        };
        user = userService.InsertNewVersion(user).Result;
        // Criar a empresa nesta conta
        companyService._user = accountService._user;
        Company company = new Company()
        {
          Name = view.NameCompany,
          Skills = new List<Skill>()
        };
        company = companyService.InsertNewVersion(company).Result;
        // Criar a pessoa para autenticação
        personService._user = accountService._user;
        Person person = new Person()
        {
          TypeUser = EnumTypeUser.Administrator,
          StatusUser = EnumStatusUser.Enabled,
          Company = company,
          User = user
        };
        person = personService.InsertNewVersion(person).Result;
        accountService._user._idPerson = person._id;
        // Criar os parâmetros básicos
        // TODO: validar a rotina de cópia de infraestrutura
        infraService._idAccount = account._id;
        await infraService.CopyTemplateInfra(company);
        return "Account created!";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, _context);
      }
    }
    #endregion

    #region Get
    public List<ViewListAccount> GetAll(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        Task<List<Account>> detail = accountService.GetAllFreeNewVersion(p => p.Status == EnumStatus.Enabled && p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name");
        total = accountService.CountFreeNewVersion(p => p.Status == EnumStatus.Enabled && p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail.Result
          .Select(x => new ViewListAccount()
          {
            _id = x._id,
            Name = x.Name
          }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    #endregion

    public Account GeAccount(Expression<Func<Account, bool>> filter)
    {
      return accountService.GetAuthentication(filter).FirstOrDefault();
    }



    public ViewPerson AlterAccount(string idaccount, string link)
    {
      try
      {
        var user = personService.GetAuthentication(p => p._idAccount == idaccount & p.TypeUser == EnumTypeUser.Administrator).FirstOrDefault();
        if (user == null)
        {
          user = personService.GetAuthentication(p => p._idAccount == idaccount & p.TypeUser == EnumTypeUser.Support).FirstOrDefault();
        }

        var parameter = parameterService.GetAuthentication(p => p.Name == "viewlo" & p._idAccount == idaccount).FirstOrDefault();
        if (parameter == null)
        {
          parameter = new Parameter
          {
            _idAccount = idaccount,
            Name = "viewlo",
            Content = "show",
            Status = EnumStatus.Enabled
          };
          parameterService.InsertAccount(parameter);
        }

        var userPerson = userService.GetAuthentication(p => p._id == user.User._id).FirstOrDefault();

        LogSave(user);

        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link);
          var data = new
          {
            userPerson.Mail,
            userPerson.Password
          };
          var json = JsonConvert.SerializeObject(data);
          var content = new StringContent(json);
          content.Headers.ContentType.MediaType = "application/json";
          client.DefaultRequestHeaders.Add("ContentType", "application/json");
          var result = client.PostAsync("manager/authentication/encrypt", content).Result;

          var resultContent = result.Content.ReadAsStringAsync().Result;
          var auth = JsonConvert.DeserializeObject<ViewPerson>(resultContent);

          return auth;
        }

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public ViewPerson AlterAccountPerson(string idperson, string link)
    {
      try
      {
        var user = personService.GetAuthentication(p => p._id == idperson).FirstOrDefault();
        var userPerson = userService.GetAuthentication(p => p._id == user.User._id).FirstOrDefault();
        LogSave(user);

        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link);
          var data = new
          {
            userPerson.Mail,
            userPerson.Password
          };

          var json = JsonConvert.SerializeObject(data);
          var content = new StringContent(json);
          content.Headers.ContentType.MediaType = "application/json";
          client.DefaultRequestHeaders.Add("ContentType", "application/json");
          var result = client.PostAsync("manager/authentication/encrypt", content).Result;

          var resultContent = result.Content.ReadAsStringAsync().Result;
          var auth = JsonConvert.DeserializeObject<ViewPerson>(resultContent);

          return auth;
        }

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
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
          Description = "Alter Account/User out: " + user.User.Name + "/" + user._idAccount
          + " - in: " + _user.NamePerson + "/" + _user._idAccount,
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
#pragma warning restore 1998
#pragma warning restore 4014
}
