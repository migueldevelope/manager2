﻿using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessNew;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools;

namespace Manager.Services.Specific
{
  public class ServiceAccount : Repository<Account>, IServiceAccount
  {

    private readonly ServiceGeneric<Account> serviceAccount;
    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<User> serviceUser;
    private readonly ServiceGeneric<Company> serviceCompany;
    private readonly ServiceGeneric<Parameter> serviceParameter;
    private readonly ServiceInfra serviceInfra;
    private readonly ServiceLog serviceLog;

    #region Constructor
    public ServiceAccount(DataContext context, DataContext contextLog, IServiceControlQueue serviceControlQueue) : base(context)
    {
      try
      {
        serviceAccount = new ServiceGeneric<Account>(context);
        serviceAuthentication = new ServiceAuthentication(context, contextLog, serviceControlQueue ,"");
        servicePerson = new ServiceGeneric<Person>(context);
        serviceUser = new ServiceGeneric<User>(context);
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceInfra = new ServiceInfra(context);
        serviceLog = new ServiceLog(context, contextLog);
        serviceParameter = new ServiceGeneric<Parameter>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceAccount._user = _user;
      servicePerson._user = _user;
      serviceUser._user = _user;
      serviceCompany._user = _user;
      serviceInfra.SetUser(_user);
      serviceLog.SetUser(_user);
      serviceParameter._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceAccount._user = _user;
      servicePerson._user = _user;
      serviceUser._user = _user;
      serviceCompany._user = _user;
      serviceInfra.SetUser(_user);
      serviceLog.SetUser(_user);
      serviceParameter._user = _user;
    }
    #endregion

    #region Account
    public string NewAccount(ViewNewAccount view)
    {
      try
      {
        // Criar uma nova conta
        Account account = new Account()
        {
          Name = view.NameAccount,
          Status = EnumStatus.Enabled,
          InfoClient = view.InfoClient
        };
        account = serviceAccount.InsertAccountNewVersion(account).Result;
        serviceAccount._user = new BaseUser()
        {
          _idAccount = account._id,
          NameAccount = account.Name,
          Mail = view.Mail,
          NamePerson = view.NameCompany
        };
        // Criar o usuário para autenticação
        serviceUser._user = serviceAccount._user;
        User user = new User()
        {
          Name = view.NameAccount,
          Mail = view.Mail,
          Password = EncryptServices.GetMD5Hash(view.Password),
          UserAdmin = true
        };
        user = serviceUser.InsertNewVersion(user).Result;
        // Criar a empresa nesta conta
        serviceCompany._user = serviceAccount._user;
        Company company = new Company()
        {
          Name = view.NameCompany,
          Skills = new List<ViewListSkill>()
        };
        company = serviceCompany.InsertNewVersion(company).Result;
        // Criar a pessoa para autenticação
        servicePerson._user = serviceAccount._user;
        Person person = new Person()
        {
          TypeUser = EnumTypeUser.Administrator,
          TypeJourney = EnumTypeJourney.OutOfJourney,
          StatusUser = EnumStatusUser.Enabled,
          Company = company.GetViewList(),
          User = user.GetViewCrud()
        };
        person = servicePerson.InsertNewVersion(person).Result;
        serviceAccount._user._idUser = person.User._id;
        // Criar os parâmetros básicos
        serviceInfra._user = serviceAccount._user;
        //Task.Run(() => serviceInfra.CopyTemplateInfraAsync(company));
        serviceInfra.CopyTemplateInfraAsync(company).Wait();
        return "Account created!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateAccount(ViewCrudAccount view, string id)
    {
      try
      {
        // Criar uma nova conta
        var account = serviceAccount.GetFreeNewVersion(p => p._id == id).Result;
        account.Name = view.Name;
        account.InfoClient = view.InfoClient;

        serviceAccount.UpdateAccount(account, null).Wait();
        return "Account altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListAccount> GetAccountList(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        total = serviceAccount.CountFreeNewVersion(p => p.Status == EnumStatus.Enabled && p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return serviceAccount.GetAllFreeNewVersion(p => p.Status == EnumStatus.Enabled && p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name")
          .Result.Select(x => x.GetViewList()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudAccount GetAccount(string id)
    {
      try
      {
        return serviceAccount.GetFreeNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Change Account Authentication or Person Authentication
    public ViewPerson AlterAccount(string idaccount)
    {
      try
      {
        Person person = servicePerson.GetFreeNewVersion(p => p._idAccount == idaccount & p.TypeUser == EnumTypeUser.Administrator).Result;
        if (person == null)
          person = servicePerson.GetFreeNewVersion(p => p._idAccount == idaccount & p.TypeUser == EnumTypeUser.Support).Result;
        User user = serviceUser.GetFreeNewVersion(p => p._id == person.User._id).Result;
        Task.Run(() => LogSave(person, "Authentication Change Account"));
        return serviceAuthentication.Authentication(user, false);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewPerson AlterAccountPerson(string idperson)
    {
      try
      {
        Person person = servicePerson.GetFreeNewVersion(p => p._id == idperson).Result;
        User user = serviceUser.GetFreeNewVersion(p => p._id == person.User._id).Result;
        Task.Run(() => LogSave(person, "Authentication Change Person"));
        return serviceAuthentication.Authentication(user, false);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void LogSave(Person person, string local)
    {
      try
      {
        serviceLog.NewLog(
          new ViewLog()
          {
            Description = string.Format("Alter Account/User out: {0}/{1} - in: {2}/{3}", servicePerson._user.NameAccount, servicePerson._user.NamePerson, serviceAccount.GetFreeNewVersion(p => p._id == person._idAccount).Result.Name, person.User.Name),
            Local = local,
            _idPerson = person._id
          });
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Syncronize Parameters
    public string SynchronizeParameters()
    {
      Task.Run(() => serviceInfra.SynchronizeParametersAsync());
      return "Parameters synchonized!";
    }
    #endregion

  }
}
