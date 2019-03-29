using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Services.Specific
{
  public class ServiceDictionarySystem : Repository<DictionarySystem>, IServiceDictionarySystem
  {
    private readonly ServiceGeneric<DictionarySystem> serviceDictionarySystem;
    private readonly ServiceGeneric<Person> servicePerson;

    #region Constructor
    public ServiceDictionarySystem(DataContext context) : base(context)
    {
      try
      {
        serviceDictionarySystem = new ServiceGeneric<DictionarySystem>(context);
        servicePerson = new ServiceGeneric<Person>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceDictionarySystem._user = _user;
      servicePerson._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      serviceDictionarySystem._user = user;
      servicePerson._user = user;
    }
    #endregion

    #region Dictonary System
    public string New(ViewCrudDictionarySystem view)
    {
      try
      {
        DictionarySystem dictionarySystem = new DictionarySystem()
        {
          Name = view.Name,
          Description = view.Description          
        };
        dictionarySystem = serviceDictionarySystem.InsertNewVersion(dictionarySystem).Result;
        return "Dictionary system added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudDictionarySystem view)
    {
      try
      {
        DictionarySystem dictionarySystem = serviceDictionarySystem.GetNewVersion(p => p._id == view._id).Result;
        dictionarySystem.Description = view.Description;
        dictionarySystem = serviceDictionarySystem.UpdateNewVersion(dictionarySystem).Result;
        return "Dictionary system altered!";
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
        DictionarySystem item = serviceDictionarySystem.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        item = serviceDictionarySystem.UpdateNewVersion(item).Result;
        return "Dictionary system deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudDictionarySystem Get(string id)
    {
      try
      {
        DictionarySystem dictionarySystem = serviceDictionarySystem.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudDictionarySystem()
        {
          _id = dictionarySystem._id,
          Name = dictionarySystem.Name,
          Description = dictionarySystem.Description
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewListDictionarySystem GetName(string name)
    {
      try
      {
        DictionarySystem dictionarySystem = serviceDictionarySystem.GetNewVersion(p => p.Name == name).Result;
        return new ViewListDictionarySystem()
        {
          _id = dictionarySystem._id,
          Name = dictionarySystem.Name,
          Description = dictionarySystem.Description
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListDictionarySystem> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceDictionarySystem.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, skip, "Name").Result
            .Select(x => new ViewListDictionarySystem()
            {
              _id = x._id,
              Name = x.Name,
              Description = x.Description
            }).ToList();
        total = serviceDictionarySystem.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Old
    public string New(List<ViewListDictionarySystem> list)
    {
      try
      {
        DictionarySystem dictionarySystem;
        foreach (var view in list)
        {
          dictionarySystem = new DictionarySystem()
          {
            Name = view.Name,
            Description = view.Description
          };
          dictionarySystem = serviceDictionarySystem.InsertNewVersion(dictionarySystem).Result;
        }
        return "List dictionary system added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

  }
}
