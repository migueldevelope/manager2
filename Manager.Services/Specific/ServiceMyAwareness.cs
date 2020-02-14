using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Core.Views;
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
    private readonly ServiceLog serviceLog;

    #region Constructor
    public ServiceMyAwareness(DataContext context) : base(context)
    {
      try
      {
        serviceMyAwareness = new ServiceGeneric<MyAwareness>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceLog = new ServiceLog(context, context);
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
      serviceLog.SetUser(_user);
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceMyAwareness._user = user;
      servicePerson._user = user;
      serviceLog.SetUser(_user);
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
        MyAwareness myawareness = serviceMyAwareness.GetNewVersion(p => p._idPerson == _user._idPerson).Result;
        var id = "";
        if (myawareness == null)
        {
          myawareness = new MyAwareness()
          {
            _id = view._id,
            NamePerson = view.NamePerson,
            _idPerson = view._idPerson,
            Date = DateTime.Now
          };
          myawareness.FutureVision = new MyAwarenessQuestions()
          {
            Health = view.FutureVision.Health,
            PersonalInterest = view.FutureVision.PersonalInterest,
            PersonalRelationships = view.FutureVision.PersonalRelationships,
            PurposeOfLife = view.FutureVision.PurposeOfLife,
            SelfImage = view.FutureVision.SelfImage,
            Worker = view.FutureVision.Worker
          };
          myawareness.Impediment = new MyAwarenessQuestions()
          {
            Health = view.Impediment.Health,
            PersonalInterest = view.Impediment.PersonalInterest,
            PersonalRelationships = view.Impediment.PersonalRelationships,
            PurposeOfLife = view.Impediment.PurposeOfLife,
            SelfImage = view.Impediment.SelfImage,
            Worker = view.Impediment.Worker
          };
          myawareness.Reality = new MyAwarenessQuestions()
          {
            Health = view.Reality.Health,
            PersonalInterest = view.Reality.PersonalInterest,
            PersonalRelationships = view.Reality.PersonalRelationships,
            PurposeOfLife = view.Reality.PurposeOfLife,
            SelfImage = view.Reality.SelfImage,
            Worker = view.Reality.Worker
          };
          myawareness.Planning = new MyAwarenessQuestions()
          {
            Health = view.Planning.Health,
            PersonalInterest = view.Planning.PersonalInterest,
            PersonalRelationships = view.Planning.PersonalRelationships,
            PurposeOfLife = view.Planning.PurposeOfLife,
            SelfImage = view.Planning.SelfImage,
            Worker = view.Planning.Worker
          };

          view._id = serviceMyAwareness.InsertNewVersion(myawareness).Result._id;
          Task.Run(() => LogSave(_user._idPerson, string.Format("Start process | {0}", view._id)));
          id = myawareness._id;
        }
        else
        {

          var myawarenessCopy = new MyAwareness()
          {
            _id = view._id,
            NamePerson = view.NamePerson,
            _idPerson = view._idPerson,
            Date = DateTime.Now
          };
          myawarenessCopy.FutureVision = new MyAwarenessQuestions()
          {
            Health = myawareness.FutureVision.Health,
            PersonalInterest = myawareness.FutureVision.PersonalInterest,
            PersonalRelationships = myawareness.FutureVision.PersonalRelationships,
            PurposeOfLife = myawareness.FutureVision.PurposeOfLife,
            SelfImage = myawareness.FutureVision.SelfImage,
            Worker = myawareness.FutureVision.Worker
          };
          myawarenessCopy.Impediment = new MyAwarenessQuestions()
          {
            Health = myawareness.Impediment.Health,
            PersonalInterest = myawareness.Impediment.PersonalInterest,
            PersonalRelationships = myawareness.Impediment.PersonalRelationships,
            PurposeOfLife = myawareness.Impediment.PurposeOfLife,
            SelfImage = myawareness.Impediment.SelfImage,
            Worker = myawareness.Impediment.Worker
          };
          myawarenessCopy.Reality = new MyAwarenessQuestions()
          {
            Health = myawareness.Reality.Health,
            PersonalInterest = myawareness.Reality.PersonalInterest,
            PersonalRelationships = myawareness.Reality.PersonalRelationships,
            PurposeOfLife = myawareness.Reality.PurposeOfLife,
            SelfImage = myawareness.Reality.SelfImage,
            Worker = myawareness.Reality.Worker
          };
          myawarenessCopy.Planning = new MyAwarenessQuestions()
          {
            Health = myawareness.Planning.Health,
            PersonalInterest = myawareness.Planning.PersonalInterest,
            PersonalRelationships = myawareness.Planning.PersonalRelationships,
            PurposeOfLife = myawareness.Planning.PurposeOfLife,
            SelfImage = myawareness.Planning.SelfImage,
            Worker = myawareness.Planning.Worker
          };

          id = myawarenessCopy._id;
          view._id = serviceMyAwareness.InsertNewVersion(myawarenessCopy).Result._id;
          Task.Run(() => LogSave(_user._idPerson, string.Format("Start process | {0}", view._id)));

        }

        return id;
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

        Task.Run(() => LogSave(_user._idPerson, string.Format("Update process | {0}", myawareness._id)));

        myawareness.FutureVision = new MyAwarenessQuestions()
        {
          Health = view.FutureVision.Health,
          PersonalInterest = view.FutureVision.PersonalInterest,
          PersonalRelationships = view.FutureVision.PersonalRelationships,
          PurposeOfLife = view.FutureVision.PurposeOfLife,
          SelfImage = view.FutureVision.SelfImage,
          Worker = view.FutureVision.Worker
        };
        myawareness.Impediment = new MyAwarenessQuestions()
        {
          Health = view.Impediment.Health,
          PersonalInterest = view.Impediment.PersonalInterest,
          PersonalRelationships = view.Impediment.PersonalRelationships,
          PurposeOfLife = view.Impediment.PurposeOfLife,
          SelfImage = view.Impediment.SelfImage,
          Worker = view.Impediment.Worker
        };
        myawareness.Reality = new MyAwarenessQuestions()
        {
          Health = view.Reality.Health,
          PersonalInterest = view.Reality.PersonalInterest,
          PersonalRelationships = view.Reality.PersonalRelationships,
          PurposeOfLife = view.Reality.PurposeOfLife,
          SelfImage = view.Reality.SelfImage,
          Worker = view.Reality.Worker
        };
        myawareness.Planning = new MyAwarenessQuestions()
        {
          Health = view.Planning.Health,
          PersonalInterest = view.Planning.PersonalInterest,
          PersonalRelationships = view.Planning.PersonalRelationships,
          PurposeOfLife = view.Planning.PurposeOfLife,
          SelfImage = view.Planning.SelfImage,
          Worker = view.Planning.Worker
        };

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
        Task.Run(() => LogSave(_user._idPerson, string.Format("Query process | {0}", id)));
        return serviceMyAwareness.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudMyAwareness GetNow()
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, string.Format("Query process | {0}", _user._idPerson)));
        return serviceMyAwareness.GetAllNewVersion(p => p._idPerson == _user._idPerson).Result.LastOrDefault()?.GetViewCrud();
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

    public List<ViewListMyAwareness> ListPerson(string idperson, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListMyAwareness> detail = serviceMyAwareness.GetAllNewVersion(p =>
        p._idPerson == idperson, count, count * (page - 1), "NamePerson").Result
          .Select(x => x.GetViewList()).ToList();
        total = serviceMyAwareness.CountNewVersion(p => p._idPerson == idperson).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region private

    private void LogSave(string idperson, string local)
    {
      try
      {
        var user = servicePerson.GetAllNewVersion(p => p._id == idperson).Result.FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "MyAwareness",
          Local = local,
          _idPerson = user._id
        };
        serviceLog.NewLog(log);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion


  }
}
