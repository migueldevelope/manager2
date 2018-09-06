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
  public class ServiceParameters : Repository<Company>, IServiceParameters
  {
    private readonly ServiceGeneric<Parameter> parameterService;
    private readonly ServiceGeneric<Person> personService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceParameters(DataContext context)
      : base(context)
    {
      try
      {
        parameterService = new ServiceGeneric<Parameter>(context);
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
      parameterService._user = _user;
      personService._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      parameterService._user = baseUser;
      personService._user = baseUser;
    }

    public string New(Parameter view)
    {
      try
      {
        parameterService.Insert(view);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Update(Parameter view)
    {
      try
      {
        parameterService.Update(view, null);
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
        var item = parameterService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        parameterService.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Parameter Get(string id)
    {
      try
      {
        return parameterService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Parameter> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        try
        {
          int skip = (count * (page - 1));
          var detail = parameterService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
          total = detail.Count();

          return detail.Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
        }
        catch (Exception e)
        {
          throw new ServiceException(_user, e, this._context);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
