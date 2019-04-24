using Manager.Core.Base;
using Manager.Core.Business;
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
  public class ServiceParameters : Repository<Company>, IServiceParameters
  {
    private readonly ServiceGeneric<Parameter> serviceParameter;
    private readonly ServiceInfra serviceInfra;
    private readonly ServiceGeneric<Person> servicePerson;

    #region Constructor
    public ServiceParameters(DataContext context) : base(context)
    {
      try
      {
        serviceInfra = new ServiceInfra(context);
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
      serviceInfra.SetUser(_user);
      serviceParameter._user = _user;
      servicePerson._user = _user;
    }

    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceInfra.SetUser(user);
      serviceParameter._user = user;
      servicePerson._user = user;
    }
    #endregion

    #region Parameter
    public string Delete(string id)
    {
      try
      {
        Parameter item = serviceParameter.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceParameter.Update(item, null);
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
        Person person = servicePerson.GetNewVersion(p => p._id == _user._idPerson).Result;
        if (person.TypeUser != EnumTypeUser.Support)
          throw new Exception("Add parameter not available!");

        if (string.IsNullOrEmpty(view.Key))
          throw new Exception("Parameter Key invalid!");

        Parameter parameter = serviceParameter.GetNewVersion(p => p.Key == view.Key).Result;
        if (parameter != null)
          throw new Exception("Parameter Key exists!");

        parameter = serviceParameter.InsertNewVersion(new Parameter()
        {
          _id = view._id,
          Name = view.Name,
          Content = view.Content,
          Key = view.Key,
          Help = view.Help,
        }).Result;
        serviceInfra.AddParameterAccounts(parameter);
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
        parameter.Name = view.Name;
        // Não deve ser alterada a propriedade key
        parameter.Content = view.Content;
        parameter.Help = view.Help;
        serviceParameter.Update(parameter, null);
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
          Help = item.Help,
          Content = item.Content,
          Key = item.Key
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudParameter GetKey(string key)
    {
      try
      {
        Parameter item = serviceParameter.GetNewVersion(p => p.Key == key).Result;
        return new ViewCrudParameter()
        {
          _id = item._id,
          Name = item.Name,
          Help = item.Help,
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

  }
}
