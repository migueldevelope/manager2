using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using Tools;

namespace Manager.Services.Specific
{
  public class ServiceAccount : Repository<Account>, IServiceAccount
  {
    private readonly ServiceGeneric<Account> accountService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<Company> companyService;
    private readonly ServiceInfra infraService;
    private IServiceLog logService;

    public ServiceAccount(DataContext context)
      : base(context)
    {
      try
      {
        accountService = new ServiceGeneric<Account>(context);
        personService = new ServiceGeneric<Person>(context);
        companyService = new ServiceGeneric<Company>(context);
        infraService = new ServiceInfra(context);
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
        var user = new Person()
        {
          _idAccount = id,
          Name = view.NameAccount,
          Company = _company,
          Mail = view.Mail,
          Document = view.Document,
          StatusUser = EnumStatusUser.Enabled,
          Status = EnumStatus.Enabled,
          Password = EncryptServices.GetMD5Hash(view.Password),
          TypeUser = EnumTypeUser.Administrator
        };
        personService.InsertAccount(user);

        infraService._idAccount = account._id;
        infraService.CopyTemplateInfra(company);
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
        var user = personService.GetAuthentication(p => p._idAccount == idaccount & p.TypeUser == EnumTypeUser.Administrator).FirstOrDefault();
        if(user == null)
        {
          user = personService.GetAuthentication(p => p._idAccount == idaccount & p.TypeUser == EnumTypeUser.Support).FirstOrDefault();
        }
        LogSave(user);
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link);
          var data = new
          {
            Mail = user.Mail,
            Password = user.Password
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
            Mail = user.Mail,
            Password = user.Password
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
          NamePerson = user.Name,
          Mail = user.Mail,
          _idPerson = user._id
        };
        logService = new ServiceLog(_context);
        var log = new ViewLog()
        {
          Description = "Alter Account/User out: " + user.Name + "/" + user._idAccount
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
}
