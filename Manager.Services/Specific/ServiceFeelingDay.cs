﻿using Manager.Core.Base;
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
  public class ServiceFeelingDay : Repository<FeelingDay>, IServiceFeelingDay
  {
    private readonly ServiceGeneric<FeelingDay> serviceFeelingDay;
    private readonly ServiceGeneric<Person> servicePerson;

    #region Constructor
    public ServiceFeelingDay(DataContext context) : base(context)
    {
      try
      {
        serviceFeelingDay = new ServiceGeneric<FeelingDay>(context);
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
      serviceFeelingDay._user = _user;
      servicePerson._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceFeelingDay._user = user;
      servicePerson._user = user;
    }
    #endregion

    #region FeelingDay
    public string Delete(string id)
    {
      try
      {
        var item = serviceFeelingDay.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceFeelingDay.Update(item, null).Wait();
        return "FeelingDay deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string New(EnumFeeling feeling)
    {
      try
      {
        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");
        var model = serviceFeelingDay.InsertNewVersion(new FeelingDay()
        {
          Date = datenow,
          Feeling = feeling,
          _idUser = _user._idUser
        }).Result;

        

        return "FeelingDay added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudFeelingDay view)
    {
      try
      {
        var model = serviceFeelingDay.GetNewVersion(p => p._id == view._id).Result;

        model.Feeling = view.Feeling;
        model.Date = view.Date;
        model._idUser = view._idUser;
        serviceFeelingDay.Update(model, null).Wait();


        return "FeelingDay altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudFeelingDay Get(string id)
    {
      try
      {
        return serviceFeelingDay.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudFeelingDay GetFeeelingDay()
    {
      try
      {
        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");

        return serviceFeelingDay.GetNewVersion(p => p._idUser == _user._idUser 
        && p.Date == datenow).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListFeelingDay> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListFeelingDay> detail = serviceFeelingDay.GetAllNewVersion(p => p._idUser.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(x => x.GetViewList()).ToList();
        total = serviceFeelingDay.CountNewVersion(p => p._idUser.ToUpper().Contains(filter.ToUpper())).Result;
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