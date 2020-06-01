using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
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
    private readonly ServiceGeneric<PendingCheckinObjective> servicePendingCheckinObjective;

    #region Constructor
    public ServiceObjective(DataContext context) : base(context)
    {
      try
      {
        serviceDimension = new ServiceGeneric<Dimension>(context);
        serviceObjective = new ServiceGeneric<Objective>(context);
        serviceKeyResult = new ServiceGeneric<KeyResult>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        servicePendingCheckinObjective = new ServiceGeneric<PendingCheckinObjective>(context);
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
      servicePendingCheckinObjective._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceObjective._user = user;
      serviceKeyResult._user = user;
      servicePerson._user = user;
      serviceDimension._user = user;
      servicePendingCheckinObjective._user = user;
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

    public ViewCrudObjective New(ViewCrudObjective view)
    {
      try
      {
        var model = serviceObjective.InsertNewVersion(new Objective()
        {
          _id = view._id,
          Description = view.Description,
          Detail = view.Detail,
          StartDate = view.StartDate,
          EndDate = view.EndDate,
          StausObjective = view.StausObjective,
          Dimension = view.Dimension,
          Responsible = view.Responsible,
          Editors = view.Editors,
          TypeCheckin = view.TypeCheckin
        }).Result;

        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");



        return model.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudObjective Update(ViewCrudObjective view)
    {
      try
      {
        var model = serviceObjective.GetNewVersion(p => p._id == view._id).Result;
        model.Description = view.Description;
        model.Detail = view.Detail;
        model.StartDate = view.StartDate;
        model.EndDate = view.EndDate;
        model.StausObjective = view.StausObjective;
        model.Dimension = view.Dimension;
        model.Responsible = view.Responsible;
        model.Editors = view.Editors;
        model.TypeCheckin = view.TypeCheckin;
        serviceObjective.Update(model, null).Wait();


        return model.GetViewCrud();
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
    public ViewCrudKeyResult NewKeyResult(ViewCrudKeyResult view)
    {
      try
      {
        Objective objective = serviceObjective.GetNewVersion(p => p._id == view.Objective._id).Result;
        var model = serviceKeyResult.InsertNewVersion(
          new KeyResult()
          {
            _id = view._id,
            Name = view.Name,
            TypeKeyResult = view.TypeKeyResult,
            QuantityGoal = view.QuantityGoal,
            QualityGoal = view.QualityGoal,
            BeginProgressGoal = view.BeginProgressGoal,
            EndProgressGoal = view.EndProgressGoal,
            Sense = view.Sense,
            Description = view.Description,
            Weight = view.Weight,
            Participants = new List<ViewCrudParticipantKeyResult>(),
            Objective = objective.GetViewList()
          }).Result;

        return model.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudKeyResult UpdateKeyResult(ViewCrudKeyResult view)
    {
      try
      {
        Objective objective = serviceObjective.GetNewVersion(p => p._id == view.Objective._id).Result;
        var model = serviceKeyResult.GetNewVersion(p => p._id == view._id).Result;
        model.Name = view.Name;
        model.Objective = objective.GetViewList();
        model.TypeKeyResult = view.TypeKeyResult;
        model.QuantityGoal = view.QuantityGoal;
        model.QualityGoal = view.QualityGoal;
        model.BeginProgressGoal = view.BeginProgressGoal;
        model.EndProgressGoal = view.EndProgressGoal;
        model.Sense = view.Sense;
        model.Description = view.Description;
        model.Weight = view.Weight;
        serviceKeyResult.Update(model, null).Wait();
        return model.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudKeyResult UpdateResultKeyResult(string idkeyresult, decimal result, ViewText view)
    {
      try
      {
        var model = serviceKeyResult.GetNewVersion(p => p._id == idkeyresult).Result;
        //Objective objective = serviceObjective.GetNewVersion(p => p._id == model.Objective._id).Result;
        model.QuanlityResult = view.Text;
        model.QuantityResult = result;

        //var average = serviceKeyResult.GetAllNewVersion(p => p.Objective._id == objective._id).Result.Average(p => p.QuantityResult);
        //serviceObjective.Update(objective, null).Wait();

        serviceKeyResult.Update(model, null).Wait();

        return model.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddParticipants(string idkeyresult, ViewCrudParticipantKeyResult view)
    {
      try
      {
        var model = serviceKeyResult.GetNewVersion(p => p._id == idkeyresult).Result;
        model.Participants.Add(view);

        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteParticipants(string idkeyresult, string idperson)
    {
      try
      {
        var model = serviceKeyResult.GetNewVersion(p => p._id == idkeyresult).Result;
        foreach (var item in model.Participants)
        {
          if (item._idPerson == idperson)
          {
            model.Participants.Remove(item);
            var i = serviceKeyResult.Update(model, null);
          }
        }

        return "remove";
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
        var model = serviceKeyResult.GetNewVersion(p => p._id == id).Result;
        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled).Result;
        var view = model.GetViewCrud();
        view.Participants = new List<ViewListPersonPhoto>();
        foreach (var item in model.Participants)
        {
          if (item.TypeParticipantKeyResult == EnumTypeParticipantKeyResult.Team)
          {
            var team = persons.Where(p => p.Manager._id == item._idPerson).Select(p => p.GetViewListPhoto());
            foreach (var person in team)
            {
              view.Participants.Add(person);
            }
          }
          else
          {
            view.Participants.Add(persons.Where(p => p._id == item._idPerson).FirstOrDefault().GetViewListPhoto());
          }

        }

        return view;
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
    public ViewCrudDimension NewDimension(ViewCrudDimension view)
    {
      try
      {
        var model = serviceDimension.InsertNewVersion(
          new Dimension()
          {
            _id = view._id,
            Name = view.Name,
          }).Result;
        return model.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudDimension UpdateDimension(ViewCrudDimension view)
    {
      try
      {
        var model = serviceDimension.GetNewVersion(p => p._id == view._id).Result;
        model.Name = view.Name;
        serviceDimension.Update(model, null).Wait();
        return model.GetViewCrud();
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

    #region PendingCheckinObjective
    public string DeletePendingCheckinObjective(string id)
    {
      try
      {
        PendingCheckinObjective item = servicePendingCheckinObjective.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        servicePendingCheckinObjective.Update(item, null).Wait();
        return "PendingCheckinObjective deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudPendingCheckinObjective NewPendingCheckinObjective(ViewCrudPendingCheckinObjective view)
    {
      try
      {
        var model = servicePendingCheckinObjective.InsertNewVersion(
          new PendingCheckinObjective()
          {
            _id = view._id,
            _idObjective = view._idObjective,
            _idKeyResult = view._idKeyResult,
            _idPerson = view._idPerson,
            LevelTrust = view.LevelTrust,
            Date = DateTime.Now,
            Impediments = new List<ViewCrudImpedimentsIniciatives>(),
            Iniciatives = new List<ViewCrudImpedimentsIniciatives>(),
          }).Result;
        return model.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListPersonPhoto> AddEditors(string idobjective, string idperson)
    {
      try
      {
        var model = serviceObjective.GetNewVersion(p => p._id == idobjective).Result;
        model.Editors.Add(servicePerson.GetNewVersion(p => p._id == idperson).Result.GetViewListPhoto());
        return model.Editors;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteEditor(string idobjetctive, string idperson)
    {
      try
      {
        var model = serviceObjective.GetNewVersion(p => p._id == idobjetctive).Result;
        foreach (var item in model.Editors)
        {
          if (item._id == idperson)
          {
            model.Editors.Remove(item);
            var i = serviceObjective.Update(model, null);
          }
        }

        return "remove";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewCrudImpedimentsIniciatives> AddImpediment(string idchecking, ViewCrudImpedimentsIniciatives view)
    {
      try
      {
        var model = servicePendingCheckinObjective.GetNewVersion(p => p._id == idchecking).Result;
        view._id = ObjectId.GenerateNewId().ToString();
        view.Like = new List<ViewCrudLike>();
        view.Deslike = new List<ViewCrudLike>();
        model.Impediments.Add(view);
        var i = servicePendingCheckinObjective.Update(model, null);
        return model.Impediments;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewCrudImpedimentsIniciatives> DeleteImpediment(string idchecking, string idimpediment)
    {
      try
      {
        var model = servicePendingCheckinObjective.GetNewVersion(p => p._id == idchecking).Result;
        foreach (var item in model.Impediments)
        {
          if (item._id == idimpediment)
          {
            model.Impediments.Remove(item);
            var i = servicePendingCheckinObjective.Update(model, null);
          }

        }
        return model.Impediments;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewCrudImpedimentsIniciatives> AddInitiatives(string idchecking, ViewCrudImpedimentsIniciatives view)
    {
      try
      {
        var model = servicePendingCheckinObjective.GetNewVersion(p => p._id == idchecking).Result;
        view._id = ObjectId.GenerateNewId().ToString();
        view.Like = new List<ViewCrudLike>();
        view.Deslike = new List<ViewCrudLike>();
        model.Iniciatives.Add(view);
        var i = servicePendingCheckinObjective.Update(model, null);

        return model.Iniciatives;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewCrudImpedimentsIniciatives> DeleteIniciative(string idchecking, string idiniciative)
    {
      try
      {
        var model = servicePendingCheckinObjective.GetNewVersion(p => p._id == idchecking).Result;
        foreach (var item in model.Iniciatives)
        {
          if (item._id == idiniciative)
          {
            model.Iniciatives.Remove(item);
            var i = servicePendingCheckinObjective.Update(model, null);
          }

        }
        return model.Impediments;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string LikeImpediment(string idimpediment, string idpendingchecking, bool like)
    {
      try
      {
        var model = servicePendingCheckinObjective.GetNewVersion(p => p._id == idpendingchecking).Result;

        foreach (var item in model.Impediments)
        {
          if (item._id == idimpediment)
          {
            var view = new ViewCrudLike()
            {
              Date = DateTime.Now,
              _idUser = _user._idUser
            };
            if (like)
              item.Like.Add(view);
            else
              item.Deslike.Add(view);
          }
        }


        return "like";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string LikeIniciative(string idiniciatives, string idpendingchecking, bool like)
    {
      try
      {
        var model = servicePendingCheckinObjective.GetNewVersion(p => p._id == idpendingchecking).Result;

        foreach (var item in model.Iniciatives)
        {
          if (item._id == idiniciatives)
          {
            var view = new ViewCrudLike()
            {
              Date = DateTime.Now,
              _idUser = _user._idUser
            };
            if (like)
              item.Like.Add(view);
            else
              item.Deslike.Add(view);
          }
        }


        return "like";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudPendingCheckinObjective UpdatePendingCheckinObjective(ViewCrudPendingCheckinObjective view)
    {
      try
      {
        var model = servicePendingCheckinObjective.GetNewVersion(p => p._id == view._id).Result;
        model._idObjective = view._idObjective;
        model._idKeyResult = view._idKeyResult;
        model._idPerson = view._idPerson;
        model.LevelTrust = view.LevelTrust;
        model.Date = DateTime.Now;

        servicePendingCheckinObjective.Update(model, null).Wait();
        return model.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudPendingCheckinObjective GetPendingCheckinObjective(string id)
    {
      try
      {
        return servicePendingCheckinObjective.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPendingCheckinObjective> ListPendingCheckinObjective(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListPendingCheckinObjective> detail = servicePendingCheckinObjective.GetAllNewVersion(p => p._id.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewListPendingCheckinObjective
          {
            _id = p._id,
          }).ToList();
        total = servicePendingCheckinObjective.CountNewVersion(p => p._id.ToUpper().Contains(filter.ToUpper())).Result;
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
