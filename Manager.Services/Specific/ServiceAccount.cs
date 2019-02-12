using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.Enumns;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using Tools;

namespace Manager.Services.Specific
{
#pragma warning disable 1998
#pragma warning disable 4014
  public class ServiceAccount : Repository<Account>, IServiceAccount
  {
    private readonly ServiceGeneric<Account> accountService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<User> userService;
    private readonly ServiceGeneric<Company> companyService;
    private readonly ServiceInfra infraService;
    private readonly ServiceGeneric<Parameter> parameterService;
    private IServiceLog logService;

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

    public Account GeAccount(Expression<Func<Account, bool>> filter)
    {
      return accountService.GetAuthentication(filter).FirstOrDefault();
    }

    public void NewAccount(ViewNewAccount view)
    {
      try
      {
        var account = new Account()
        {
          Name = view.NameAccount,
          Status = EnumStatus.Enabled
        };
        var id = accountService.Insert(account)._id;
        account._idAccount = id;
        accountService.UpdateAccount(account, null);

        var company = new Company()
        {
          Name = view.NameCompany,
          _idAccount = id,
          Skills = new List<Skill>(),
          Status = EnumStatus.Enabled
        };
        var _company = companyService.InsertAccount(company);
        var user = new User()
        {
          _idAccount = id,
          Name = view.NameAccount,
          Mail = view.Mail,
          Document = view.Document,
          Status = EnumStatus.Enabled,
          Password = EncryptServices.GetMD5Hash(view.Password),
          TypeUser = EnumTypeUser.Administrator
        };
        userService.InsertAccount(user);

        infraService._idAccount = account._id;
        infraService.CopyTemplateInfra(company);


        var parameter = parameterService.GetAuthentication(p => p.Name == "viewlo" & p._idAccount == id).FirstOrDefault();
        if (parameter == null)
        {
          parameter = new Parameter
          {
            _idAccount = id,
            Name = "viewlo",
            Content = "show",
            Status = EnumStatus.Enabled
          };
          parameterService.InsertAccount(parameter);
        }

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Account> GeAccounts(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = accountService.GetAuthentication(p => p.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).ToList();

        total = accountService.GetAuthentication(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();


        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public ViewPerson AlterAccount(string idaccount, string link)
    {
      try
      {
        var user = personService.GetAuthentication(p => p._idAccount == idaccount & p.User.TypeUser == EnumTypeUser.Administrator).FirstOrDefault();
        if (user == null)
        {
          user = personService.GetAuthentication(p => p._idAccount == idaccount & p.User.TypeUser == EnumTypeUser.Support).FirstOrDefault();
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

        LogSave(user);
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link);
          var data = new
          {
            user.User.Mail,
            user.User.Password
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
        LogSave(user);

        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link);
          var data = new
          {
            user.User.Mail,
            user.User.Password
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
