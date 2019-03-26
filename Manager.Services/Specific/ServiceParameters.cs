﻿using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Services.Specific
{
  public class ServiceParameters : Repository<Company>, IServiceParameters
  {
    private readonly ServiceGeneric<Parameter> serviceParameter;
    private readonly ServiceGeneric<Person> servicePerson;

    #region Constructor
    public ServiceParameters(DataContext context) : base(context)
    {
      try
      {
        serviceParameter = new ServiceGeneric<Parameter>(context);
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
      serviceParameter._user = _user;
      servicePerson._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      serviceParameter._user = baseUser;
      servicePerson._user = baseUser;
    }
    #endregion

    #region Parameter
    public string Remove(string id)
    {
      try
      {
        Parameter item = serviceParameter.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        item = serviceParameter.UpdateNewVersion(item).Result;
        return "Parameter deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string New(ViewCrudParameter view)
    {
      try
      {
        Parameter parameter = serviceParameter.InsertNewVersion(new Parameter() { _id = view._id, Name = view.Name, Content = view.Content }).Result;
        return "Parameter added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudParameter view)
    {
      try
      {
        Parameter parameter = serviceParameter.GetNewVersion(p => p._id == view._id).Result;
        parameter.Content = view.Content;
        parameter = serviceParameter.UpdateNewVersion(parameter).Result;
        return "Parameter update!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudParameter Get(string id)
    {
      try
      {
        Parameter item = serviceParameter.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudParameter()
        {
          _id = item._id,
          Name = item.Name,
          Content = item.Content
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudParameter GetName(string name)
    {
      try
      {
        Parameter item = serviceParameter.GetNewVersion(p => p.Name == name).Result;
        return new ViewCrudParameter()
        {
          _id = item._id,
          Name = item.Name,
          Content = item.Content
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListParameter> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListParameter> detail = serviceParameter.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()),count, count * (page - 1), "Name").Result
          .Select(p => new ViewListParameter()
          {
            _id = p._id,
            Name = p.Name,
            Content = p.Content
          }).ToList();
        total = serviceParameter.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Old
    public string NewOld(Parameter view)
    {
      try
      {
        serviceParameter.Insert(view);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateOld(Parameter view)
    {
      try
      {
        serviceParameter.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Parameter GetOld(string id)
    {
      try
      {
        return serviceParameter.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Parameter GetNameOld(string name)
    {
      try
      {
        return serviceParameter.GetAll(p => p.Name == name).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Parameter> ListOld(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceParameter.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = serviceParameter.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    #endregion

  }

}
