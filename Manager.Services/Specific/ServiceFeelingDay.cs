using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
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
        //var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");
        var datenow = DateTime.Now;
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

    public List<ViewFeelingQtd> GetQuantity(string idmanager, long days)
    {
      try
      {
        days *= -1;
        var date = DateTime.Now.AddDays(days);
        var list = new List<ViewFeelingQtd>();

        var feeling = serviceFeelingDay.GetAllNewVersion(p => p.Date >= date).Result;
        if (idmanager != "")
        {
          var persons = servicePerson.GetAllNewVersion(p => p.Manager._id == idmanager).Result.Select(p => p.User._id);
          feeling = feeling.Where(p => persons.Contains(p._idUser)).ToList();
        }


        for (var item = 0; item < 5; item++)
        {
          var view = new ViewFeelingQtd();
          view.Feeling = (EnumFeeling)item;
          view.Qtd = feeling.Where(p => p.Feeling == (EnumFeeling)item).Count();
          list.Add(view);
        }


        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewFeelingManager> GetManager(string idmanager, long days)
    {
      try
      {
        days *= -1;
        var date = DateTime.Now.AddDays(days);
        var list = new List<ViewFeelingManager>();

        var feeling = serviceFeelingDay.GetAllNewVersion(p => p.Date >= date).Result;

        var persons = servicePerson.GetAllNewVersion(p => p.Manager._id == idmanager).Result;
        feeling = feeling.Where(p => persons.Select(p => p.User._id).Contains(p._idUser)).ToList();



        foreach (var item in feeling)
        {
          var view = new ViewFeelingManager();
          view.Felling = item.Feeling;
          view.Day = item.Date;
          view._idUser = item._idUser;
          view.Name = persons.Where(p => p.User._id == item._idUser).FirstOrDefault().User.Name;
          list.Add(view);
        }


        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudFeelingDay GetFeelingDay()
    {
      try
      {
        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");
        
        var view = serviceFeelingDay.GetNewVersion(p => p._idUser == _user._idUser
        && p.Date > datenow.AddDays(-1) && p.Date < datenow.AddDays(+1)).Result;
        if (view == null)
          return null;
        else
          return view.GetViewCrud();
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
