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
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceMyAwareness : Repository<MyAwareness>, IServiceMyAwareness
  {
    private readonly ServiceGeneric<MyAwareness> serviceMyAwareness;
    private readonly ServiceGeneric<Person> servicePerson;

    #region Constructor
    public ServiceMyAwareness(DataContext context) : base(context)
    {
      try
      {
        serviceMyAwareness = new ServiceGeneric<MyAwareness>(context);
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
      serviceMyAwareness._user = _user;
      servicePerson._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceMyAwareness._user = user;
      servicePerson._user = user;
    }
    #endregion

    #region MyAwareness
    public string Delete(string id)
    {
      try
      {
        MyAwareness item = serviceMyAwareness.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceMyAwareness.Update(item, null).Wait();
        return "MyAwareness deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string New(ViewCrudMyAwareness view)
    {
      try
      {
        MyAwareness myawareness = serviceMyAwareness.InsertNewVersion(new MyAwareness()
        {
          _id = view._id,
        }).Result;

        return "MyAwareness added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudMyAwareness view)
    {
      try
      {
        MyAwareness myawareness = serviceMyAwareness.GetNewVersion(p => p._id == view._id).Result;

        serviceMyAwareness.Update(myawareness, null).Wait();


        return "MyAwareness altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudMyAwareness Get(string id)
    {
      try
      {
        return serviceMyAwareness.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListMyAwareness> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListMyAwareness> detail = serviceMyAwareness.GetAllNewVersion(p => p.Status == EnumStatus.Enabled, count, count * (page - 1), "_id").Result
          .Select(x => x.GetViewList()).ToList();
        total = serviceMyAwareness.CountNewVersion(p => p.Status == EnumStatus.Enabled).Result;
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
