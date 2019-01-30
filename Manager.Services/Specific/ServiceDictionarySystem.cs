using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manager.Services.Specific
{
  public class ServiceDictionarySystem : Repository<DictionarySystem>, IServiceDictionarySystem
  {
    private readonly ServiceGeneric<DictionarySystem> dictionarySystemService;
    private readonly ServiceGeneric<Person> personService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceDictionarySystem(DataContext context)
      : base(context)
    {
      try
      {
        dictionarySystemService = new ServiceGeneric<DictionarySystem>(context);
        personService = new ServiceGeneric<Person>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      dictionarySystemService._user = _user;
      personService._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      dictionarySystemService._user = baseUser;
      personService._user = baseUser;
    }

    public string New(DictionarySystem view)
    {
      try
      {
        dictionarySystemService.Insert(view);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public string New(List<DictionarySystem> list)
    {
      try
      {
        foreach(var view in list)
        {
          dictionarySystemService.Insert(view);
        }
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Update(DictionarySystem view)
    {
      try
      {
        dictionarySystemService.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Remove(string id)
    {
      try
      {
        var item = dictionarySystemService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        dictionarySystemService.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public DictionarySystem Get(string id)
    {
      try
      {
        return dictionarySystemService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public DictionarySystem GetName(string name)
    {
      try
      {
        return dictionarySystemService.GetAll(p => p.Name == name).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<DictionarySystem> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = dictionarySystemService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = dictionarySystemService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
  }

}
