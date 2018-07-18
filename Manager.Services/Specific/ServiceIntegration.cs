using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tools;

namespace Manager.Services.Specific
{
  public class ServiceIntegration : Repository<Person>, IServiceIntegration
  {
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<Schooling> schoolingService;
    private readonly ServiceGeneric<Company> companyService;
    private readonly ServiceGeneric<Account> accountService;
    private readonly IServiceLog logService;

    public ServiceIntegration(DataContext context)
      : base(context)
    {
      try
      {
        personService = new ServiceGeneric<Person>(context);
        schoolingService = new ServiceGeneric<Schooling>(context);
        companyService = new ServiceGeneric<Company>(context);
        accountService = new ServiceGeneric<Account>(context);
        logService = new ServiceLog(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Company CompanyGet(string name)
    {
      try
      {
        var item = companyService.GetAll(p => p.Name == name);
        if (item.Count() > 0)
          return item.FirstOrDefault();
        else
          return NewCompany(name);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Person ManagerGet(string document)
    {
      try
      {
        var item = personService.GetAll(p => p.Document == document);
        var retorno = item.FirstOrDefault();
        return retorno;
      }
      catch (ServiceException)
      {
        return null;
      }
    }

    public string NewAccount(string name)
    {
      try
      {
        var item = new Account()
        {
          Name = name,
          Status = EnumStatus.Enabled
        };
        return accountService.Insert(item)._id;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Company NewCompany(string name)
    {
      try
      {
        var item = new Company()
        {
          Name = name,
          Status = EnumStatus.Enabled
        };
        return companyService.Insert(item);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AccountGet(string name)
    {
      try
      {
        var item = accountService.GetAll(p => p.Name == name);
        if (item.Count() > 0)
          return item.FirstOrDefault()._id;
        else
          return NewAccount(name);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Schooling SchoolingGet(string name)
    {
      try
      {
        var item = schoolingService.GetAll(p => p.Name == name);
        if (item.Count() > 0)
          return item.FirstOrDefault();
        else
          return NewSchooling(name);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Schooling NewSchooling(string name)
    {
      try
      {
        var item = new Schooling()
        {
          Name = name,
          Status = EnumStatus.Enabled
        };
        return schoolingService.Insert(item);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void UpdateManager()
    {
      var list = personService.GetAll();
      try
      {
        foreach (var item in list)
        {
          var manager = ManagerGet(item.DocumentManager);
          if (manager != null)
          {
            item.Manager = manager;
            personService.Update(item, null);
          }
        }
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewPersonImport> ListPersonJson(StreamReader file)
    {
      try
      {
        string row = null;
        string[] itemArray = null;
        var list = new List<ViewPersonImport>();
        var head = 0;
        while ((row = file.ReadLine()) != null)
        {
          itemArray = row.Split(';');
          //File.WriteAllText("", itemArray.ToString(), Encoding.UTF8);
          if (head != 0)
          {
            var item = new ViewPersonImport()
            {
              Name = itemArray[0].ToString(),
              Document = itemArray[1].ToString(),
              Mail = itemArray[2].ToString(),
              Phone = itemArray[3].ToString(),
              TypeUser = (EnumTypeUser)byte.Parse(itemArray[4].ToString()),
              StatusUser = (EnumStatusUser)byte.Parse(itemArray[5].ToString()),
              NameCompany = itemArray[6].ToString(),
              Registration = long.Parse(itemArray[9].ToString()),
              NameManager = itemArray[10].ToString(),
              DateBirth = DateTime.Parse(itemArray[11].ToString()),
              DateAdm = DateTime.Parse(itemArray[12].ToString()),
              DocumentManager = itemArray[13].ToString(),
              NameSchooling = itemArray[15].ToString(),
            };
            list.Add(item);
          }
          else
          {
            head = 1;
          }
        }
        file.Close();
        return list;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public async Task<string> SetItem(ViewPersonImport item)
    {
      try
      {
        var person = new Person();
        var exists = personService.GetAll(p => p.Document == item.Document);
        var bopExists = false;
        if (exists.Count() > 0)
        {
          person = exists.FirstOrDefault();
          bopExists = true;
        }
        else
        {
          person.Password = EncryptServices.GetMD5Hash(item.Document);
          person.StatusUser = EnumStatusUser.Enabled;
          person.Status = EnumStatus.Enabled;
          bopExists = false;
        }
        person.Company = CompanyGet(item.NameCompany.Trim());
        person.Schooling = SchoolingGet(item.NameSchooling.Trim());
        person.Name = item.Name.Trim();
        person.DocumentManager = item.DocumentManager.Trim();
        person.Phone = item.Phone.Trim();
        person.Mail = item.Mail.Trim();
        person.Registration = item.Registration;
        person.DateAdm = item.DateAdm;
        person.DateBirth = item.DateBirth;
        person.Document = item.Document.Trim();
        person.TypeUser = item.TypeUser;
        if (bopExists == false)
          personService.Insert(person);
        else
          personService.Update(person, null);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public async Task<string> ImportPerson(List<ViewPersonImport> list)
    {
      try
      {

        foreach (var item in list)
        {
          SetItem(item);
        }
        //UpdateManager();
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public async void Log()
    {
      try
      {
        var user = personService.GetAll(p => p._id == _user._idPerson).FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "Import",
          Local = "ImportPerson",
          Person = user
        };
        logService.NewLog(log);
      }
      catch (Exception)
      {
        throw;
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      personService._user = _user;
      schoolingService._user = _user;
      companyService._user = _user;
      accountService._user = _user;
      logService.user = _user;
    }

    public void SetUser(BaseUser user)
    {
      _user = user;
      personService._user = user;
      schoolingService._user = user;
      companyService._user = user;
      accountService._user = user;
    }
  }
}
