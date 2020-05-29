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
  public class ServiceObjective : Repository<Objective>, IServiceObjective
  {
    private readonly ServiceGeneric<Dimension> serviceDimension;
    private readonly ServiceGeneric<Objective> serviceObjective;
    private readonly ServiceGeneric<KeyResult> serviceKeyResult;
    private readonly ServiceGeneric<Person> servicePerson;

    #region Constructor
    public ServiceObjective(DataContext context) : base(context)
    {
      try
      {
        serviceDimension = new ServiceGeneric<Dimension>(context);
        serviceObjective = new ServiceGeneric<Objective>(context);
        serviceKeyResult = new ServiceGeneric<KeyResult>(context);
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
      serviceObjective._user = _user;
      serviceKeyResult._user = _user;
      servicePerson._user = _user;
      serviceDimension._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceObjective._user = user;
      serviceKeyResult._user = user;
      servicePerson._user = user;
      serviceDimension._user = user;
    }
    #endregion

    #region Objective
    public string Delete(string id)
    {
      try
      {
        Objective item = serviceObjective.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceObjective.Update(item, null).Wait();
        return "Objective deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string New(ViewCrudObjective view)
    {
      try
      {
        Objective objective = serviceObjective.InsertNewVersion(new Objective()
        {
          _id = view._id,
        }).Result;

        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");

 

        return "Objective added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudObjective view)
    {
      try
      {
        Objective objective = serviceObjective.GetNewVersion(p => p._id == view._id).Result;
    
        serviceObjective.Update(objective, null).Wait();


        return "Objective altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudObjective Get(string id)
    {
      try
      {
        return serviceObjective.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListObjective> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListObjective> detail = serviceObjective.GetAllNewVersion(p => p.Description.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Description").Result
          .Select(x => new ViewListObjective()
          {
            _id = x._id,
            Description = x.Description
          }).ToList();
        total = serviceObjective.CountNewVersion(p => p.Description.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    #endregion

    #region KeyResult
    public string DeleteKeyResult(string id)
    {
      try
      {
        KeyResult item = serviceKeyResult.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceKeyResult.Update(item, null).Wait();
        return "KeyResult deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string NewKeyResult(ViewCrudKeyResult view)
    {
      try
      {
        Objective objective = serviceObjective.GetNewVersion(p => p._id == view.Objective._id).Result;
        KeyResult keyresult = serviceKeyResult.InsertNewVersion(
          new KeyResult()
          {
            _id = view._id,
            Name = view.Name,
            Objective = objective.GetViewList()
          }).Result;
        return "KeyResult added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateKeyResult(ViewCrudKeyResult view)
    {
      try
      {
        Objective objective = serviceObjective.GetNewVersion(p => p._id == view.Objective._id).Result;
        KeyResult keyresult = serviceKeyResult.GetNewVersion(p => p._id == view._id).Result;
        keyresult.Name = view.Name;
        keyresult.Objective = objective.GetViewList();
        serviceKeyResult.Update(keyresult, null).Wait();
        return "KeyResult altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudKeyResult GetKeyResult(string id)
    {
      try
      {
        return serviceKeyResult.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListKeyResult> ListKeyResult(string idobjective, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListKeyResult> detail = serviceKeyResult.GetAllNewVersion(p => p.Objective._id == idobjective && p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewListKeyResult
          {
            _id = p._id,
            Name = p.Name
          }).ToList();
        total = serviceKeyResult.CountNewVersion(p => p.Objective._id == idobjective && p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListKeyResult> ListKeyResult(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListKeyResult> detail = serviceKeyResult.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewListKeyResult
          {
            _id = p._id,
            Name = p.Name
          }).ToList();
        total = serviceKeyResult.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion


    #region Dimension
    public string DeleteDimension(string id)
    {
      try
      {
        Dimension item = serviceDimension.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceDimension.Update(item, null).Wait();
        return "Dimension deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string NewDimension(ViewCrudDimension view)
    {
      try
      {
        Dimension dimension = serviceDimension.InsertNewVersion(
          new Dimension()
          {
            _id = view._id,
            Name = view.Name,
          }).Result;
        return "Dimension added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateDimension(ViewCrudDimension view)
    {
      try
      {
        Dimension dimension = serviceDimension.GetNewVersion(p => p._id == view._id).Result;
        dimension.Name = view.Name;
        serviceDimension.Update(dimension, null).Wait();
        return "Dimension altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudDimension GetDimension(string id)
    {
      try
      {
        return serviceDimension.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListDimension> ListDimension(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListDimension> detail = serviceDimension.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewListDimension
          {
            _id = p._id,
            Name = p.Name
          }).ToList();
        total = serviceDimension.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
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
