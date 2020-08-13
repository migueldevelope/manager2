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
using System.Globalization;
using System.IO;
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
    private readonly ServiceExcel serviceExcel;

    #region Constructor
    public ServiceObjective(DataContext context) : base(context)
    {
      try
      {
        serviceDimension = new ServiceGeneric<Dimension>(context);
        serviceObjective = new ServiceGeneric<Objective>(context);
        serviceKeyResult = new ServiceGeneric<KeyResult>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceExcel = new ServiceExcel();
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
        if (view.Editors == null)
          view.Editors = new List<ViewListPersonPhoto>();

        if (view.Participants == null)
          view.Participants = new List<ViewListPersonPhoto>();

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
          Participants = view.Participants,
          Reached = false,
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
        model.Participants = view.Participants;

        if (model.Editors == null)
          model.Editors = view.Editors;
        else
          model.Editors = new List<ViewListPersonPhoto>();

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


    public ViewListObjectiveParticipantCard GetParticipantCard()
    {
      try
      {
        Calendar calendar = CultureInfo.InvariantCulture.Calendar;
        var week = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        byte fortnight = DateTime.Now.Day >= 15 ? byte.Parse("2") : byte.Parse("1");
        var month = DateTime.Now.Month;

        var view = new ViewListObjectiveParticipantCard();
        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");

        var objective = serviceObjective.GetAllNewVersion(p => p.StartDate <= datenow && p.EndDate >= datenow && p.StausObjective == EnumStausObjective.Active).Result;
        var idsobjective = objective.Select(p => p._id);

        var keyresultsprevious = serviceKeyResult.GetAllNewVersion(p => p.Status != EnumStatus.Disabled
         && idsobjective.Contains(p.Objective._id)).Result;
        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled).Result;
        var keyresults = new List<KeyResult>();

        foreach (var item in keyresultsprevious)
        {
          foreach (var par in item.ParticipantsAdd)
          {
            if (par.TypeParticipantKeyResult == EnumTypeParticipantKeyResult.Team)
            {
              var team = persons.Where(p => p.Manager?._id == par._idPerson).Select(p => p.GetViewListPhoto());
              foreach (var person in team)
              {
                if (person._id == _user._idPerson)
                  keyresults.Add(item);
              }
            }
            else
            {
              if (par._idPerson == _user._idPerson)
                keyresults.Add(item);
            }
          }

          var obj = objective.Where(p => p._id == item.Objective._id).FirstOrDefault();
          if (obj != null)
            foreach (var par in obj.Participants)
            {
              if (par._id == _user._idPerson)
                keyresults.Add(item);
            }
        }


        //keyresults = keyresults.Where(p => p.ParticipantsGet.Where(x => x._id == _user._idPerson).Count() > 0).ToList();

        var pendingchecking = servicePendingCheckinObjective.GetAllNewVersion(p => p._idPerson == _user._idPerson
        && p.Lasted == true && idsobjective.Contains(p._idObjective)).Result;


        if (keyresults.Count() > 0)
          view.AverageAchievement = keyresults.Average(p => p.Achievement);
        if (pendingchecking.Count() > 0)
          view.AverageTrust = pendingchecking.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString()));

        view.QtdKeyResults = keyresults.Count();
        if (view.AverageTrust <= 50)
          view.LevelTrust = 0;
        else if ((view.AverageTrust > 50) && (view.AverageTrust <= 75))
          view.LevelTrust = 1;
        else
          view.LevelTrust = 2;

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListObjectiveParticipantCard GetParticipantCard(string idperson)
    {
      try
      {
        Calendar calendar = CultureInfo.InvariantCulture.Calendar;
        var week = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        byte fortnight = DateTime.Now.Day >= 15 ? byte.Parse("2") : byte.Parse("1");
        var month = DateTime.Now.Month;
        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");

        var view = new ViewListObjectiveParticipantCard();
        var objective = serviceObjective.GetAllNewVersion(p => datenow >= p.EndDate).Result.Select(p => p._id);
        var keyresultsprevious = serviceKeyResult.GetAllNewVersion(p => p.Status != EnumStatus.Disabled
        && objective.Contains(p.Objective._id)).Result;

        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled).Result;
        var keyresults = new List<KeyResult>();

        foreach (var item in keyresultsprevious)
        {
          foreach (var par in item.ParticipantsAdd)
          {
            if (par.TypeParticipantKeyResult == EnumTypeParticipantKeyResult.Team)
            {
              var team = persons.Where(p => p.Manager?._id == par._idPerson).Select(p => p.GetViewListPhoto());
              foreach (var person in team)
              {
                if (person._id == idperson)
                  keyresults.Add(item);
              }
            }
            else
            {
              if (par._idPerson == idperson)
                keyresults.Add(item);
            }
          }
        }

        //keyresults = keyresults.Where(p => p.ParticipantsGet.Where(x => x._id == idperson).Count() > 0).ToList();

        //var pendingchecking = servicePendingCheckinObjective.GetAllNewVersion(p => p.Week == week && p._idPerson == idperson).Result;
        var pendingchecking = servicePendingCheckinObjective.GetAllNewVersion(p => p._idPerson == idperson).Result;


        if (keyresults.Count() > 0)
          view.AverageAchievement = keyresults.Average(p => p.Achievement);
        if (pendingchecking.Count() > 0)
          view.AverageTrust = pendingchecking.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString()));

        view.QtdKeyResults = keyresults.Count();
        if (view.AverageTrust <= 50)
          view.LevelTrust = 0;
        else if ((view.AverageTrust > 50) && (view.AverageTrust <= 75))
          view.LevelTrust = 1;
        else
          view.LevelTrust = 2;

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListObjectiveResponsibleCard GetResponsibleCard()
    {
      try
      {
        Calendar calendar = CultureInfo.InvariantCulture.Calendar;
        var week = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        byte fortnight = DateTime.Now.Day >= 15 ? byte.Parse("2") : byte.Parse("1");
        var month = DateTime.Now.Month;

        var view = new ViewListObjectiveResponsibleCard();
        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");


        var objective = serviceObjective.GetAllNewVersion(p => p.StartDate <= datenow && p.EndDate >= datenow && p.StausObjective == EnumStausObjective.Active).Result
          .Where(p => p.Editors.Where(x => x._id == _user._idPerson).Count() > 0 || p.Responsible._id == _user._idPerson).Select(p => p._id);
        var keyresults = serviceKeyResult.GetAllNewVersion(p => objective.Contains(p.Objective._id)).Result;
        //var pendingchecking = servicePendingCheckinObjective.GetAllNewVersion(p => p.Week == week && p._idPerson == _user._idPerson).Result;

        var pendingchecking = servicePendingCheckinObjective.GetAllNewVersion(p => p._idPerson == _user._idPerson
        && p.Lasted == true && objective.Contains(p._idObjective)).Result;

        if (keyresults.Count() > 0)
        {
          decimal totalweight = 0;
          decimal totalachievment = 0;
          foreach (var item in keyresults)
          {
            totalweight += item.Weight;
            totalachievment += item.Achievement * item.Weight;
          }
          view.AverageAchievement = totalachievment / (totalweight == 0 ? 1 : totalweight);
        }

        if (pendingchecking.Count() > 0)
          view.AverageTrust = pendingchecking.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString()));

        view.QtdObjective = objective.Count();
        if (view.AverageTrust <= 50)
          view.LevelTrust = 0;
        else if ((view.AverageTrust > 50) && (view.AverageTrust <= 75))
          view.LevelTrust = 1;
        else
          view.LevelTrust = 2;

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public ViewListObjectiveResponsibleCard GetResponsibleCard(string id)
    {
      try
      {
        Calendar calendar = CultureInfo.InvariantCulture.Calendar;
        var week = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        byte fortnight = DateTime.Now.Day >= 15 ? byte.Parse("2") : byte.Parse("1");
        var month = DateTime.Now.Month;

        var view = new ViewListObjectiveResponsibleCard();

        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");

        var objective = serviceObjective.GetAllNewVersion(p => datenow >= p.EndDate).Result
          .Where(p => p.Editors.Where(x => x._id == id).Count() > 0 || p.Responsible._id == id).Select(p => p._id);
        var keyresults = serviceKeyResult.GetAllNewVersion(p => objective.Contains(p.Objective._id)).Result;
        //var pendingchecking = servicePendingCheckinObjective.GetAllNewVersion(p => p.Week == week && p._idPerson == id).Result;

        var pendingchecking = servicePendingCheckinObjective.GetAllNewVersion(p => p._idPerson == id).Result;

        if (keyresults.Count() > 0)
        {
          decimal totalweight = 0;
          decimal totalachievment = 0;
          foreach (var item in keyresults)
          {
            totalweight += item.Weight;
            totalachievment += item.Achievement * item.Weight;
          }
          view.AverageAchievement = totalachievment / (totalweight == 0 ? 1 : totalweight);
        }

        if (pendingchecking.Count() > 0)
          view.AverageTrust = pendingchecking.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString()));

        view.QtdObjective = objective.Count();
        if (view.AverageTrust <= 50)
          view.LevelTrust = 0;
        else if ((view.AverageTrust > 50) && (view.AverageTrust <= 75))
          view.LevelTrust = 1;
        else
          view.LevelTrust = 2;

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewListPersonPhoto> GetListManager()
    {
      try
      {
        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.TypeUser == EnumTypeUser.Manager || p.TypeUser == EnumTypeUser.ManagerHR).Result;
        return persons.Select(p => p.GetViewListPhoto()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPersonPhoto> GetListEmployee()
    {
      try
      {
        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled).Result;
        return persons.Select(p => p.GetViewListPhoto()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListDetailResposibleObjective> GetDetailResposibleObjective(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        var list = new List<ViewListDetailResposibleObjective>();
        Calendar calendar = CultureInfo.InvariantCulture.Calendar;
        var week = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        byte fortnight = DateTime.Now.Day >= 15 ? byte.Parse("2") : byte.Parse("1");
        var month = DateTime.Now.Month;


        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");

        var objectives = serviceObjective.GetAllNewVersion(p =>
        p.StartDate <= datenow && p.EndDate >= datenow && p.StausObjective == EnumStausObjective.Active
        && p.Description.Contains(filter))
        .Result.Where(p => p.Editors.Where(x => x._id == _user._idPerson).Count() > 0
        || p.Responsible._id == _user._idPerson).ToList();


        foreach (var item in objectives)
        {
          var keyresults = serviceKeyResult.GetAllNewVersion(p => item._id == p.Objective._id).Result;
          var pendingcheckingprevious = servicePendingCheckinObjective.GetAllNewVersion(p => p._idPerson == _user._idPerson
          && p._idObjective == item._id).Result;
          var pendingchecking = pendingcheckingprevious.Where(p => p._idPerson == _user._idPerson
          && p._idObjective == item._id).ToList();
          //&& p.Week == week && p._idObjective == item._id).Result;

          var view = new ViewListDetailResposibleObjective();

          if (pendingchecking.Count() > 0)
          {
            view.Impediments = pendingcheckingprevious.Sum(p => p.Impediments.Count());
            view.Iniciatives = pendingcheckingprevious.Sum(p => p.Iniciatives.Count());

            var trust = pendingchecking.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString()));
            view.AverageTrust = trust;
            if (trust <= 50)
              view.LevelTrust = 0;
            else if ((trust > 50) && (trust <= 75))
              view.LevelTrust = 1;
            else
              view.LevelTrust = 2;
          }
          view.Description = item.Description;
          view.Detail = item.Detail;
          view.StartDate = item.StartDate;
          view.EndDate = item.EndDate;
          view._id = item._id;

          if (keyresults.Count() > 0)
          {

            if (keyresults.Count() > 0)
            {
              decimal totalweight = 0;
              decimal totalachievment = 0;
              foreach (var kr in keyresults)
              {
                totalweight += kr.Weight;
                totalachievment += kr.Achievement * kr.Weight;
              }
              view.AverageAchievement = totalachievment / (totalweight == 0 ? 1 : totalweight);
            }


            if (view.AverageAchievement <= 60)
              view.LevelAchievement = 0;
            else if ((view.AverageAchievement > 60) && (view.AverageAchievement <= 90))
              view.LevelAchievement = 1;
            else if ((view.AverageAchievement > 90) && (view.AverageAchievement <= 100))
              view.LevelAchievement = 2;
            else
              view.LevelAchievement = 3;
          }


          list.Add(view);
        }
        var skip = count * (page - 1);
        total = list.Count();

        return list.OrderBy(p => p.Description).Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListImpedimentsIniciatives> GetImpedimentsIniciatives(string idkeyresult, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        var pendingcheckings = servicePendingCheckinObjective.GetAllNewVersion(p => p._idKeyResult == idkeyresult).Result;
        var list = new List<ViewCrudImpedimentsIniciatives>();
        foreach (var item in pendingcheckings)
        {
          var view = new ViewCrudImpedimentsIniciatives();
          list = list.Concat(item.Impediments).ToList();
          list = list.Concat(item.Iniciatives).ToList();
        }

        total = list.Count();

        var skip = count * (page - 1);
        total = list.Count();

        list = list.Where(p => p.Description.Contains(filter)).OrderBy(p => p.Date).Skip(skip).Take(count).ToList();

        var listreturn = new List<ViewListImpedimentsIniciatives>();
        foreach (var item in list)
        {
          var view = new ViewListImpedimentsIniciatives();
          view._id = item._id;
          view._idPerson = item._idPerson;
          view.NamePerson = item.NamePerson;
          view.Description = item.Description;
          view.Like = item.Like;
          view.Deslike = item.Deslike;
          view.Date = item.Date;
          view.TypeImpedimentsIniciatives = item.TypeImpedimentsIniciatives;
          view.CountLike = item.CountLike;
          view.CountDeslike = item.CountDeslike;
          if (item.Like.Where(p => p._idUser == _user._idUser).Count() > 0)
            view.SetLike = true;
          if (item.Deslike.Where(p => p._idUser == _user._idUser).Count() > 0)
            view.SetDeslike = true;

          listreturn.Add(view);
        }

        return listreturn;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListObjectiveEdit> GetObjectiveEditParticipantRH(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        var list = new List<ViewListObjectiveEdit>();

        Calendar calendar = CultureInfo.InvariantCulture.Calendar;
        var week = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        byte fortnight = DateTime.Now.Day >= 15 ? byte.Parse("2") : byte.Parse("1");
        var month = DateTime.Now.Month;
        var keyresultsprevious = serviceKeyResult.GetAllNewVersion(p => p.Status != EnumStatus.Disabled).Result;
        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled).Result;
        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");
        var listobjectives = serviceObjective.GetAllNewVersion(p => p.StartDate <= datenow && p.EndDate >= datenow && p.StausObjective == EnumStausObjective.Active).Result;

        var keyresults = new List<KeyResult>();
        foreach (var item in keyresultsprevious)
        {
          var obj = listobjectives.Where(p => p._id == item.Objective._id).FirstOrDefault();
          if (obj != null)
            foreach (var par in obj.Participants)
            {
              if (par._id == _user._idPerson)
                keyresults.Add(item);
            }

          foreach (var par in item.ParticipantsAdd)
          {
            if (par.TypeParticipantKeyResult == EnumTypeParticipantKeyResult.Team)
            {
              var team = persons.Where(p => p.Manager?._id == par._idPerson).Select(p => p.GetViewListPhoto());
              foreach (var person in team)
              {
                if (keyresults.Where(p => p._id == item._id).Count() == 0)
                  keyresults.Add(item);
              }
            }
            else
            {
              if (keyresults.Where(p => p._id == item._id).Count() == 0)
                keyresults.Add(item);
            }
          }
        }

        var ids = keyresults.Select(p => p.Objective._id).ToList();

        var objectives = listobjectives.Where(p => ids.Contains(p._id));

        foreach (var obj in objectives)
        {
          var view = new ViewListObjectiveEdit();

          var pendingcheckingprevious = servicePendingCheckinObjective.GetAllNewVersion(p => p.Lasted == true).Result
            .Where(p => ids.Contains(p._idObjective));



          view.Description = obj.Description;
          view.Detail = obj.Detail;
          view.StartDate = obj.StartDate;
          view.EndDate = obj.EndDate;
          view._id = obj._id;
          view.Editors = obj.Editors;
          view.Responsible = obj.Responsible;
          if (pendingcheckingprevious.Count() > 0)
          {
            view.QuantityImpediments = pendingcheckingprevious.Where(p => p._idObjective == view._id).Sum(p => p.Impediments.Count());
            view.QuantityIniciatives = pendingcheckingprevious.Where(p => p._idObjective == view._id).Sum(p => p.Iniciatives.Count());
          }


          //if (keyresults.Where(p => p.Objective._id == obj._id).Count() > 0)
          // view.AverageAchievement = keyresults.Where(p => p.Objective._id == obj._id).Average(p => p.Achievement);
          if (pendingcheckingprevious.Count() > 0)
          {
            view.AverageTrust = pendingcheckingprevious.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString()));
          }

          if (view.AverageTrust <= 50)
            view.LevelTrust = 0;
          else if ((view.AverageTrust > 50) && (view.AverageTrust <= 75))
            view.LevelTrust = 1;
          else
            view.LevelTrust = 2;



          view.KeyResults = new List<ViewListKeyResultsEdit>();

          decimal totalweight = 0;
          decimal totalachievment = 0;
          foreach (var kr in keyresults.Where(p => p.Objective._id == obj._id))
          {
            totalweight += kr.Weight;
            totalachievment += kr.Achievement * kr.Weight;

            var viewKeyResult = new ViewListKeyResultsEdit();
            viewKeyResult._id = kr._id;
            viewKeyResult.Name = kr.Name;
            viewKeyResult.TypeKeyResult = kr.TypeKeyResult;
            viewKeyResult.QuantityGoal = kr.QuantityGoal;
            viewKeyResult.QualityGoal = kr.QualityGoal;
            viewKeyResult.BeginProgressGoal = kr.BeginProgressGoal;
            viewKeyResult.EndProgressGoal = kr.EndProgressGoal;
            viewKeyResult.Sense = kr.Sense;
            viewKeyResult.Description = kr.Description;
            viewKeyResult.Weight = kr.Weight;
            viewKeyResult.Objective = kr.Objective;
            viewKeyResult.QualityResult = kr.QualityResult;
            viewKeyResult.QuantityResult = kr.QuantityResult;
            viewKeyResult.TypeCheckin = kr.TypeCheckin;
            viewKeyResult.TypeBinary = kr.TypeBinary;
            //viewKeyResult.Binary = kr.Binary;
            viewKeyResult.ParticipantsAdd = kr.ParticipantsAdd;
            viewKeyResult.ParticipantsGet = new List<ViewListPersonPhotoKeyResult>();
            var pendingcheckingkeyresult = pendingcheckingprevious.Where(p => p._idKeyResult == kr._id);

            if (pendingcheckingkeyresult.Count() > 0)
            {
              viewKeyResult.QuantityImpediments = pendingcheckingkeyresult.Sum(p => p.Impediments.Count());
              viewKeyResult.QuantityIniciatives = pendingcheckingkeyresult.Sum(p => p.Iniciatives.Count());
              viewKeyResult.AverageTrust = pendingcheckingkeyresult.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString())); ;
            }

            if (obj != null)
              foreach (var par in obj.Participants)
              {
                if (par._id == _user._idPerson)
                  viewKeyResult.ParticipantsGet.Add(new ViewListPersonPhotoKeyResult()
                  {
                    Name = par.Name,
                    Photo = par.Photo,
                    _id = par._id,
                    TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Single
                  });
              }

            foreach (var item in viewKeyResult.ParticipantsAdd)
            {
              if (item.TypeParticipantKeyResult == EnumTypeParticipantKeyResult.Team)
              {
                var team = persons.Where(p => p.Manager?._id == item._idPerson).Select(p => new ViewListPersonPhotoKeyResult()
                {
                  Name = p.User.Name,
                  Photo = p.User.PhotoUrl,
                  _id = p._id,
                  TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Team
                });
                foreach (var person in team)
                {
                  viewKeyResult.ParticipantsGet.Add(person);
                }
              }
              else
              {
                viewKeyResult.ParticipantsGet.Add(persons.Where(p => p._id == item._idPerson)
                  .Select(p => new ViewListPersonPhotoKeyResult()
                  {
                    Name = p.User.Name,
                    Photo = p.User.PhotoUrl,
                    _id = p._id,
                    TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Single
                  })
                  .FirstOrDefault());
              }
            }

            List<PendingCheckinObjective> pendingcheckingkey = new List<PendingCheckinObjective>();

            if (viewKeyResult.TypeCheckin == EnumTypeCheckin.Weekly)
              pendingcheckingkey = pendingcheckingprevious.Where(p => p._idKeyResult == viewKeyResult._id && p.Week == week && p._idPerson == _user._idPerson).ToList();
            else if (viewKeyResult.TypeCheckin == EnumTypeCheckin.Monthly)
              pendingcheckingkey = pendingcheckingprevious.Where(p => p._idKeyResult == viewKeyResult._id && p.Month == month && p._idPerson == _user._idPerson).ToList();
            else
              pendingcheckingkey = pendingcheckingprevious.Where(p => p._idKeyResult == viewKeyResult._id && p.Month == month && p.Fortnight == fortnight && p._idPerson == _user._idPerson).ToList();


            viewKeyResult.PendingChecking = true;
            viewKeyResult.PendingCheckinTrust = true;
            viewKeyResult.PendingCheckinAchievement = true;

            if (pendingcheckingkey.Count() > 0)
            {
              var checkin = pendingcheckingkey.FirstOrDefault();
              viewKeyResult.PendingChecking = false;
              viewKeyResult._idPendingChecking = checkin._id;
              var trustkey = pendingcheckingkey.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString()));
              if (trustkey <= 50)
                viewKeyResult.LevelTrust = 0;
              else if ((trustkey > 50) && (trustkey <= 75))
                viewKeyResult.LevelTrust = 1;
              else
                viewKeyResult.LevelTrust = 2;

              if (checkin.LevelTrust == 0)
                viewKeyResult.PendingChecking = true;
              else
                viewKeyResult.PendingChecking = false;

              if (checkin.Achievement == 0)
                viewKeyResult.PendingCheckinAchievement = true;
              else
                viewKeyResult.PendingCheckinAchievement = false;



            }
            else
            {
              viewKeyResult.PendingChecking = true;
            }

            viewKeyResult.Achievement = kr.Achievement;
            if (viewKeyResult.Achievement <= 60)
              viewKeyResult.LevelAchievement = 0;
            else if ((viewKeyResult.Achievement > 60) && (viewKeyResult.Achievement <= 90))
              viewKeyResult.LevelAchievement = 1;
            else if ((viewKeyResult.Achievement > 90) && (viewKeyResult.Achievement <= 100))
              viewKeyResult.LevelAchievement = 2;
            else
              viewKeyResult.LevelAchievement = 3;





            view.KeyResults.Add(viewKeyResult);
          }

          view.AverageAchievement = totalachievment / (totalweight == 0 ? 1 : totalweight);
          if (view.AverageAchievement <= 60)
            view.LevelAchievement = 0;
          else if ((view.AverageAchievement > 60) && (view.AverageAchievement <= 90))
            view.LevelAchievement = 1;
          else if ((view.AverageAchievement > 90) && (view.AverageAchievement <= 100))
            view.LevelAchievement = 2;
          else
            view.LevelAchievement = 3;

          list.Add(view);
        }

        list = list.Where(p => p.KeyResults != null).ToList();
        list = list.Where(p => p.KeyResults.Count() > 0).ToList();

        total = list.Count();

        var skip = count * (page - 1);
        total = list.Count();

        list = list.Where(p => p.Description.Contains(filter)).OrderBy(p => p.Description).Skip(skip).Take(count).ToList();

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListObjectiveEdit> GetObjectiveEditParticipant(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        var list = new List<ViewListObjectiveEdit>();

        Calendar calendar = CultureInfo.InvariantCulture.Calendar;
        var week = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        byte fortnight = DateTime.Now.Day >= 15 ? byte.Parse("2") : byte.Parse("1");
        var month = DateTime.Now.Month;
        //var objectives = serviceObjective.GetAllNewVersion(p => p.StausObjective == EnumStausObjective.Active).Result
        // .Where(p => p.Editors.Where(x => x._id == _user._idPerson).Count() > 0 || p.Responsible._id == _user._idPerson);
        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");
        var keyresultsprevious = serviceKeyResult.GetAllNewVersion(p => p.Status != EnumStatus.Disabled).Result;
        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled).Result;
        var listobjectives = serviceObjective.GetAllNewVersion(p => p.StartDate <= datenow && p.EndDate >= datenow && p.StausObjective == EnumStausObjective.Active).Result;

        var keyresults = new List<KeyResult>();
        foreach (var item in keyresultsprevious)
        {
          var obj = listobjectives.Where(p => p._id == item.Objective._id).FirstOrDefault();
          if (obj != null)
            foreach (var par in obj.Participants)
            {
              if (par._id == _user._idPerson)
                keyresults.Add(item);
            }

          foreach (var par in item.ParticipantsAdd)
          {
            if (par.TypeParticipantKeyResult == EnumTypeParticipantKeyResult.Team)
            {
              var team = persons.Where(p => p.Manager?._id == par._idPerson).Select(p => p.GetViewListPhoto());
              foreach (var person in team)
              {
                if (person._id == _user._idPerson)
                  if (keyresults.Where(p => p._id == item._id).Count() == 0)
                    keyresults.Add(item);
              }
            }
            else
            {
              if (par._idPerson == _user._idPerson)
                if (keyresults.Where(p => p._id == item._id).Count() == 0)
                  keyresults.Add(item);
            }
          }
        }

        var ids = keyresults.Select(p => p.Objective._id).ToList();
        var objectives = listobjectives.Where(p => ids.Contains(p._id));

        foreach (var obj in objectives)
        {
          var view = new ViewListObjectiveEdit();

          var pendingcheckingprevious = servicePendingCheckinObjective.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result
            .Where(p => ids.Contains(p._idObjective));

          if (pendingcheckingprevious.Count() > 0)
          {
            view.QuantityImpediments = pendingcheckingprevious.Sum(p => p.Impediments.Count());
            view.QuantityIniciatives = pendingcheckingprevious.Sum(p => p.Iniciatives.Count());
          }


          pendingcheckingprevious = servicePendingCheckinObjective.GetAllNewVersion(p => p.Lasted == true).Result
            .Where(p => ids.Contains(p._idObjective));

          //var pendingchecking = pendingcheckingprevious.Where(p => p.Week == week).ToList();


          view.Description = obj.Description;
          view.Detail = obj.Detail;
          view.StartDate = obj.StartDate;
          view.EndDate = obj.EndDate;
          view._id = obj._id;
          view.Editors = obj.Editors;
          view.Responsible = obj.Responsible;



          //if (keyresults.Where(p => p.Objective._id == obj._id).Count() > 0)
          //view.AverageAchievement = keyresults.Where(p => p.Objective._id == obj._id).Average(p => p.Achievement);
          if (pendingcheckingprevious.Count() > 0)
          {
            view.AverageTrust = pendingcheckingprevious.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString()));
          }

          if (view.AverageTrust <= 50)
            view.LevelTrust = 0;
          else if ((view.AverageTrust > 50) && (view.AverageTrust <= 75))
            view.LevelTrust = 1;
          else
            view.LevelTrust = 2;


          view.KeyResults = new List<ViewListKeyResultsEdit>();
          decimal totalweight = 0;
          decimal totalachievment = 0;
          foreach (var kr in keyresults.Where(p => p.Objective._id == obj._id))
          {
            totalweight += kr.Weight;
            totalachievment += kr.Achievement * kr.Weight;

            var viewKeyResult = new ViewListKeyResultsEdit();
            viewKeyResult._id = kr._id;
            viewKeyResult.Name = kr.Name;
            viewKeyResult.TypeKeyResult = kr.TypeKeyResult;
            viewKeyResult.QuantityGoal = kr.QuantityGoal;
            viewKeyResult.QualityGoal = kr.QualityGoal;
            viewKeyResult.BeginProgressGoal = kr.BeginProgressGoal;
            viewKeyResult.EndProgressGoal = kr.EndProgressGoal;
            viewKeyResult.Sense = kr.Sense;
            viewKeyResult.Description = kr.Description;
            viewKeyResult.Weight = kr.Weight;
            viewKeyResult.Objective = kr.Objective;
            viewKeyResult.TypeCheckin = kr.TypeCheckin;
            viewKeyResult.TypeBinary = kr.TypeBinary;
            viewKeyResult.QualityResult = kr.QualityResult;
            viewKeyResult.QuantityResult = kr.QuantityResult;
            //viewKeyResult.Binary = kr.Binary;
            viewKeyResult.ParticipantsAdd = kr.ParticipantsAdd;
            viewKeyResult.ParticipantsGet = new List<ViewListPersonPhotoKeyResult>();
            var pendingcheckingkeyresult = pendingcheckingprevious.Where(p => p._idKeyResult == kr._id);

            viewKeyResult.PendingChecking = true;
            viewKeyResult.PendingCheckinTrust = true;
            viewKeyResult.PendingCheckinAchievement = false;


            if (obj != null)
              foreach (var par in obj.Participants)
              {
                if (par._id == _user._idPerson)
                  viewKeyResult.ParticipantsGet.Add(new ViewListPersonPhotoKeyResult()
                  {
                    Name = par.Name,
                    Photo = par.Photo,
                    _id = par._id,
                    TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Single
                  });
              }

            if (pendingcheckingkeyresult.Count() > 0)
            {
              PendingCheckinObjective checkinperson = null;
              if (viewKeyResult.TypeCheckin == EnumTypeCheckin.Weekly)
                checkinperson = pendingcheckingkeyresult.Where(p => p._idKeyResult == viewKeyResult._id && p.Week == week && p._idPerson == _user._idPerson).FirstOrDefault();
              else if (viewKeyResult.TypeCheckin == EnumTypeCheckin.Monthly)
                checkinperson = pendingcheckingkeyresult.Where(p => p._idKeyResult == viewKeyResult._id && p.Month == month && p._idPerson == _user._idPerson).FirstOrDefault();
              else
                checkinperson = pendingcheckingkeyresult.Where(p => p._idKeyResult == viewKeyResult._id && p.Month == month && p.Fortnight == fortnight && p._idPerson == _user._idPerson).FirstOrDefault();

              //viewKeyResult.PendingCheckinAchievement = true;
              viewKeyResult.PendingCheckinTrust = true;
              if (checkinperson != null)
              {
                viewKeyResult._idPendingChecking = checkinperson._id;

                if (checkinperson.LevelTrust > 0)
                  viewKeyResult.PendingCheckinTrust = false;

                //if (checkinperson.Achievement > 0)
                //  viewKeyResult.PendingCheckinAchievement = false;
              }

              var trustkey = pendingcheckingkeyresult.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString()));
              if (trustkey <= 50)
                viewKeyResult.LevelTrust = 0;
              else if ((trustkey > 50) && (trustkey <= 75))
                viewKeyResult.LevelTrust = 1;
              else
                viewKeyResult.LevelTrust = 2;

              viewKeyResult.QuantityImpediments = pendingcheckingkeyresult.Sum(p => p.Impediments.Count());
              viewKeyResult.QuantityIniciatives = pendingcheckingkeyresult.Sum(p => p.Iniciatives.Count());

              viewKeyResult.AverageTrust = pendingcheckingkeyresult.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString())); ;
            }

            foreach (var item in viewKeyResult.ParticipantsAdd)
            {
              if (item.TypeParticipantKeyResult == EnumTypeParticipantKeyResult.Team)
              {
                var team = persons.Where(p => p.Manager?._id == item._idPerson).Select(p => new ViewListPersonPhotoKeyResult()
                {
                  Name = p.User.Name,
                  Photo = p.User.PhotoUrl,
                  _id = p._id,
                  TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Team
                });
                foreach (var person in team)
                {
                  viewKeyResult.ParticipantsGet.Add(person);
                }
              }
              else
              {
                viewKeyResult.ParticipantsGet.Add(persons.Where(p => p._id == item._idPerson)
                  .Select(p => new ViewListPersonPhotoKeyResult()
                  {
                    Name = p.User.Name,
                    Photo = p.User.PhotoUrl,
                    _id = p._id,
                    TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Single
                  })
                  .FirstOrDefault());
              }

            }


            List<PendingCheckinObjective> pendingcheckingkey = new List<PendingCheckinObjective>();

            if (viewKeyResult.TypeCheckin == EnumTypeCheckin.Weekly)
              pendingcheckingkey = pendingcheckingprevious.Where(p => p._idKeyResult == viewKeyResult._id && p.Week == week && p._idPerson == _user._idPerson).ToList();
            else if (viewKeyResult.TypeCheckin == EnumTypeCheckin.Monthly)
              pendingcheckingkey = pendingcheckingprevious.Where(p => p._idKeyResult == viewKeyResult._id && p.Month == month && p._idPerson == _user._idPerson).ToList();
            else
              pendingcheckingkey = pendingcheckingprevious.Where(p => p._idKeyResult == viewKeyResult._id && p.Month == month && p.Fortnight == fortnight && p._idPerson == _user._idPerson).ToList();

            //if (pendingcheckingkey.Count() == 0)
            //{

            //}
            //else
            //{
            //  viewKeyResult.PendingChecking = true;
            //}

            viewKeyResult.Achievement = kr.Achievement;
            if (viewKeyResult.Achievement <= 60)
              viewKeyResult.LevelAchievement = 0;
            else if ((viewKeyResult.Achievement > 60) && (viewKeyResult.Achievement <= 90))
              viewKeyResult.LevelAchievement = 1;
            else if ((viewKeyResult.Achievement > 90) && (viewKeyResult.Achievement <= 100))
              viewKeyResult.LevelAchievement = 2;
            else
              viewKeyResult.LevelAchievement = 3;




            view.KeyResults.Add(viewKeyResult);
          }

          view.AverageAchievement = totalachievment / (totalweight == 0 ? 1 : totalweight);
          if (view.AverageAchievement <= 60)
            view.LevelAchievement = 0;
          else if ((view.AverageAchievement > 60) && (view.AverageAchievement <= 90))
            view.LevelAchievement = 1;
          else if ((view.AverageAchievement > 90) && (view.AverageAchievement <= 100))
            view.LevelAchievement = 2;
          else
            view.LevelAchievement = 3;

          list.Add(view);
        }

        list = list.Where(p => p.KeyResults != null).ToList();
        list = list.Where(p => p.KeyResults.Count() > 0).ToList();

        var skip = count * (page - 1);
        total = list.Count();

        list = list.Where(p => p.Description.Contains(filter)).OrderBy(p => p.Description).Skip(skip).Take(count).ToList();

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewListObjectiveEdit> GetObjectiveEditResponsible(string idobjective, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        var list = new List<ViewListObjectiveEdit>();

        Calendar calendar = CultureInfo.InvariantCulture.Calendar;
        var week = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        byte fortnight = DateTime.Now.Day >= 15 ? byte.Parse("2") : byte.Parse("1");
        var month = DateTime.Now.Month;
        //var objectives = serviceObjective.GetAllNewVersion(p => p.StausObjective == EnumStausObjective.Active).Result
        // .Where(p => p.Editors.Where(x => x._id == _user._idPerson).Count() > 0 || p.Responsible._id == _user._idPerson);

        var keyresults = serviceKeyResult.GetAllNewVersion(p => p.Status != EnumStatus.Disabled
        && p.Objective._id == idobjective).Result;
        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled).Result;

        var ids = keyresults.Select(p => p.Objective._id).ToList();

        var objectives = serviceObjective.GetAllNewVersion(p => p._id == idobjective).Result
         .Where(p => ids.Contains(p._id));

        foreach (var obj in objectives)
        {
          var view = new ViewListObjectiveEdit();
          keyresults = keyresults.Where(p => p.Objective._id == obj._id).ToList();

          var pendingcheckingprevious = servicePendingCheckinObjective.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result
            .Where(p => ids.Contains(p._idObjective));

          var pendingchecking = pendingcheckingprevious.Where(p => p.Lasted == true);
          pendingchecking = pendingcheckingprevious.Where(p => p._idPerson == _user._idPerson);


          view.Description = obj.Description;
          view.Detail = obj.Detail;
          view.StartDate = obj.StartDate;
          view.EndDate = obj.EndDate;
          view._id = obj._id;
          view.Editors = obj.Editors;
          view.Responsible = obj.Responsible;

          if (keyresults.Count() > 0)
            view.AverageAchievement = keyresults.Average(p => p.Achievement);
          if (pendingcheckingprevious.Count() > 0)
          {
            view.AverageTrust = pendingcheckingprevious.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString()));
          }

          if (view.AverageTrust <= 50)
            view.LevelTrust = 0;
          else if ((view.AverageTrust > 50) && (view.AverageTrust <= 75))
            view.LevelTrust = 1;
          else
            view.LevelTrust = 2;

          view.KeyResults = new List<ViewListKeyResultsEdit>();

          decimal totalweight = 0;
          decimal totalachievment = 0;

          foreach (var kr in keyresults)
          {
            var pendingcheckingkey = pendingchecking.Where(p => p._idKeyResult == kr._id);
            totalweight += kr.Weight;
            totalachievment += kr.Achievement * kr.Weight;
            var viewKeyResult = new ViewListKeyResultsEdit();
            if (pendingcheckingprevious.Where(p => p._idKeyResult == kr._id).Count() > 0)
            {
              viewKeyResult.QuantityImpediments = pendingcheckingprevious.Where(p => p._idKeyResult == kr._id).Sum(p => p.Impediments.Count());
              viewKeyResult.QuantityIniciatives = pendingcheckingprevious.Where(p => p._idKeyResult == kr._id).Sum(p => p.Iniciatives.Count());
            }
            List<PendingCheckinObjective> pendingcheckingkeyweek = new List<PendingCheckinObjective>();

            if (kr.TypeCheckin == EnumTypeCheckin.Weekly)
              pendingcheckingkey = pendingchecking.Where(p => p._idKeyResult == kr._id && p.Week == week).ToList();
            else if (kr.TypeCheckin == EnumTypeCheckin.Monthly)
              pendingcheckingkey = pendingchecking.Where(p => p._idKeyResult == kr._id && p.Month == month).ToList();
            else
              pendingcheckingkey = pendingchecking.Where(p => p._idKeyResult == kr._id && p.Month == month && p.Fortnight == fortnight).ToList();



            viewKeyResult._id = kr._id;
            viewKeyResult.Name = kr.Name;
            viewKeyResult.TypeKeyResult = kr.TypeKeyResult;
            viewKeyResult.QuantityGoal = Math.Round(kr.QuantityGoal,2);
            viewKeyResult.QualityGoal = kr.QualityGoal;
            viewKeyResult.BeginProgressGoal = kr.BeginProgressGoal;
            viewKeyResult.EndProgressGoal = kr.EndProgressGoal;
            viewKeyResult.Sense = kr.Sense;
            viewKeyResult.Description = kr.Description;
            viewKeyResult.Weight = kr.Weight;
            viewKeyResult.Objective = kr.Objective;
            viewKeyResult.QualityResult = kr.QualityResult;
            viewKeyResult.QuantityResult = Math.Round(kr.QuantityResult,2);
            viewKeyResult.TypeCheckin = kr.TypeCheckin;
            viewKeyResult.TypeBinary = kr.TypeBinary;
            //viewKeyResult.Binary = kr.Binary;
            viewKeyResult.ParticipantsAdd = kr.ParticipantsAdd;
            viewKeyResult.ParticipantsGet = new List<ViewListPersonPhotoKeyResult>();

            viewKeyResult.PendingChecking = true;
            viewKeyResult.PendingCheckinTrust = true;
            viewKeyResult.PendingCheckinAchievement = true;

            //var obj = listobjectives.Where(p => p._id == kr.Objective._id).FirstOrDefault();
            if (obj != null)
              foreach (var par in obj.Participants)
              {
                if (par._id == _user._idPerson)
                  viewKeyResult.ParticipantsGet.Add(new ViewListPersonPhotoKeyResult()
                  {
                    Name = par.Name,
                    Photo = par.Photo,
                    _id = par._id,
                    TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Single
                  });
              }

            if (pendingcheckingkey.Count() > 0)
            {
              //viewKeyResult.QuantityImpediments = pendingcheckingkey.Sum(p => p.Impediments.Count());
              //viewKeyResult.QuantityIniciatives = pendingcheckingkey.Sum(p => p.Iniciatives.Count());
              //viewKeyResult.AverageTrust = pendingcheckingkeyweek.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString())); ;
              viewKeyResult.AverageTrust = pendingcheckingkey.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString())); ;
              var checkinperson = pendingcheckingkey.Where(p => p._idPerson == _user._idPerson).FirstOrDefault();
              viewKeyResult.PendingCheckinAchievement = true;
              viewKeyResult.PendingCheckinTrust = true;
              if (checkinperson != null)
              {
                viewKeyResult.PendingChecking = false;
                viewKeyResult._idPendingChecking = checkinperson._id;
                if (checkinperson.LevelTrust > 0)
                  viewKeyResult.PendingCheckinTrust = false;

                if (checkinperson.Achievement > 0)
                  viewKeyResult.PendingCheckinAchievement = false;
              }


              var trustkey = pendingcheckingkey.Average(p => decimal.Parse((p.LevelTrust == EnumLevelTrust.Low ? 0 : p.LevelTrust == EnumLevelTrust.Medium ? 50 : p.LevelTrust == EnumLevelTrust.Hight ? 100 : 0).ToString()));
              if (trustkey <= 50)
                viewKeyResult.LevelTrust = 0;
              else if ((trustkey > 50) && (trustkey <= 75))
                viewKeyResult.LevelTrust = 1;
              else
                viewKeyResult.LevelTrust = 2;
            }
            else
              viewKeyResult.PendingChecking = true;

            foreach (var item in viewKeyResult.ParticipantsAdd)
            {
              if (item.TypeParticipantKeyResult == EnumTypeParticipantKeyResult.Team)
              {
                var team = persons.Where(p => p.Manager?._id == item._idPerson).Select(p => new ViewListPersonPhotoKeyResult()
                {
                  Name = p.User.Name,
                  Photo = p.User.PhotoUrl,
                  _id = p._id,
                  TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Team
                });
                foreach (var person in team)
                {
                  viewKeyResult.ParticipantsGet.Add(person);
                }
              }
              else
              {
                viewKeyResult.ParticipantsGet.Add(persons.Where(p => p._id == item._idPerson)
                  .Select(p => new ViewListPersonPhotoKeyResult()
                  {
                    Name = p.User.Name,
                    Photo = p.User.PhotoUrl,
                    _id = p._id,
                    TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Single
                  })
                  .FirstOrDefault());
              }

            }

            viewKeyResult.Achievement = kr.Achievement;
            if (viewKeyResult.Achievement <= 60)
              viewKeyResult.LevelAchievement = 0;
            else if ((viewKeyResult.Achievement > 60) && (viewKeyResult.Achievement <= 90))
              viewKeyResult.LevelAchievement = 1;
            else if ((viewKeyResult.Achievement > 90) && (viewKeyResult.Achievement <= 100))
              viewKeyResult.LevelAchievement = 2;
            else
              viewKeyResult.LevelAchievement = 3;

            view.KeyResults.Add(viewKeyResult);
          }

          view.QuantityImpediments = view.KeyResults.Sum(p => p.QuantityImpediments);
          view.QuantityIniciatives = view.KeyResults.Sum(p => p.QuantityIniciatives);

          view.AverageAchievement = totalachievment / (totalweight == 0 ? 1 : totalweight);
          if (view.AverageAchievement <= 60)
            view.LevelAchievement = 0;
          else if ((view.AverageAchievement > 60) && (view.AverageAchievement <= 90))
            view.LevelAchievement = 1;
          else if ((view.AverageAchievement > 90) && (view.AverageAchievement <= 100))
            view.LevelAchievement = 2;
          else
            view.LevelAchievement = 3;

          list.Add(view);
        }

        var skip = count * (page - 1);
        total = list.Count();

        list = list.Where(p => p.Description.Contains(filter)).OrderBy(p => p.Description).Skip(skip).Take(count).ToList();

        return list;
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
            Reached = false,
            ParticipantsAdd = view.ParticipantsAdd,
            TypeCheckin = view.TypeCheckin,
            Objective = objective.GetViewList()
          }).Result;

        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled).Result;
        var viewreturn = model.GetViewCrud();
        viewreturn.ParticipantsGet = new List<ViewListPersonPhotoKeyResult>();
        foreach (var item in model.ParticipantsAdd)
        {
          if (item.TypeParticipantKeyResult == EnumTypeParticipantKeyResult.Team)
          {
            var team = persons.Where(p => p.Manager?._id == item._idPerson).Select(p => new ViewListPersonPhotoKeyResult()
            {
              Name = p.User.Name,
              Photo = p.User.PhotoUrl,
              _id = p._id,
              TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Team
            });
            foreach (var person in team)
            {
              viewreturn.ParticipantsGet.Add(person);
            }
          }
          else
          {
            viewreturn.ParticipantsGet.Add(persons.Where(p => p._id == item._idPerson)
              .Select(p => new ViewListPersonPhotoKeyResult()
              {
                Name = p.User.Name,
                Photo = p.User.PhotoUrl,
                _id = p._id,
                TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Single
              })
              .FirstOrDefault());
          }

        }

        return viewreturn;
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
        model.ParticipantsAdd = view.ParticipantsAdd;
        model.TypeCheckin = view.TypeCheckin;

        var keyresults = serviceKeyResult.GetAllNewVersion(p => p.Objective._id == model.Objective._id).Result;
        var reached = true;
        foreach (var item in keyresults)
        {
          if (item.Reached == false)
            reached = false;
        }

        if (reached)
        {
          objective.Reached = true;
          var i = serviceObjective.Update(objective, null);
        }

        serviceKeyResult.Update(model, null).Wait();

        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled).Result;
        var viewreturn = model.GetViewCrud();
        viewreturn.ParticipantsGet = new List<ViewListPersonPhotoKeyResult>();
        foreach (var item in model.ParticipantsAdd)
        {
          if (item.TypeParticipantKeyResult == EnumTypeParticipantKeyResult.Team)
          {
            var team = persons.Where(p => p.Manager?._id == item._idPerson).Select(p => new ViewListPersonPhotoKeyResult()
            {
              Name = p.User.Name,
              Photo = p.User.PhotoUrl,
              _id = p._id,
              TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Team
            });
            foreach (var person in team)
            {
              viewreturn.ParticipantsGet.Add(person);
            }
          }
          else
          {
            viewreturn.ParticipantsGet.Add(persons.Where(p => p._id == item._idPerson)
              .Select(p => new ViewListPersonPhotoKeyResult()
              {
                Name = p.User.Name,
                Photo = p.User.PhotoUrl,
                _id = p._id,
                TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Single
              })
              .FirstOrDefault());
          }

        }


        return viewreturn;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudKeyResult UpdateResultKeyResult(string idkeyresult, string idcheckin, decimal achievement, decimal result, ViewText view)
    {
      try
      {
        var model = serviceKeyResult.GetNewVersion(p => p._id == idkeyresult).Result;
        var checkin = servicePendingCheckinObjective.GetNewVersion(p => p._id == idcheckin).Result;


        model.QualityResult = view.Text;
        model.QuantityResult = result;

        if (model.TypeKeyResult == EnumTypeKeyResult.Quantity)
          model.Achievement = (model.QuantityResult * 100) / ((model.QuantityGoal == 0) ? 1 : model.QuantityGoal);
        else if (model.TypeKeyResult == EnumTypeKeyResult.Progress)
        {
          var diffgoal = model.EndProgressGoal - model.BeginProgressGoal;
          var diffresult = model.QuantityResult - model.BeginProgressGoal;
          model.Achievement = (diffresult * 100) / diffgoal;
        }
        else if (model.TypeKeyResult == EnumTypeKeyResult.Binary)
        {
          if ((model.TypeBinary == EnumTypeBinary.Equal) && (model.QuantityResult == model.QuantityGoal))
            model.Achievement = 100;
          else if ((model.TypeBinary == EnumTypeBinary.Bigger) && (model.QuantityResult > model.QuantityGoal))
            model.Achievement = 100;
          else if ((model.TypeBinary == EnumTypeBinary.BiggerEqual) && (model.QuantityResult >= model.QuantityGoal))
            model.Achievement = 100;
          else if ((model.TypeBinary == EnumTypeBinary.Smaller) && (model.QuantityResult < model.QuantityGoal))
            model.Achievement = 100;
          else if ((model.TypeBinary == EnumTypeBinary.SmallerEqual) && (model.QuantityResult <= model.QuantityGoal))
            model.Achievement = 100;
          else if ((model.TypeBinary == EnumTypeBinary.Different) && (model.QuantityResult != model.QuantityGoal))
            model.Achievement = 100;
          else
            model.Achievement = 0;

        }
        else
          model.Achievement = achievement;

        if (model.Achievement >= 100)
          model.Reached = true;

        if (model.Achievement > 110)
          model.Achievement = 110;
        else if (model.Achievement < 0)
          model.Achievement = 0;

        var objective = serviceObjective.GetNewVersion(p => p._id == model.Objective._id).Result;

        var keyresults = serviceKeyResult.GetAllNewVersion(p => p.Objective._id == model.Objective._id).Result;
        var reached = true;
        foreach (var item in keyresults)
        {
          if (item.Reached == false)
            reached = false;
        }

        if (reached)
        {
          objective.Reached = true;
          var i = serviceObjective.Update(objective, null);
        }


        serviceKeyResult.Update(model, null).Wait();

        checkin.QualityResult = view.Text;
        checkin.QuantityResult = result;
        checkin.Achievement = model.Achievement;

        servicePendingCheckinObjective.Update(checkin, null).Wait();


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
        model.ParticipantsAdd.Add(view);

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
        foreach (var item in model.ParticipantsAdd)
        {
          if (item._idPerson == idperson)
          {
            model.ParticipantsAdd.Remove(item);
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
        view.ParticipantsGet = new List<ViewListPersonPhotoKeyResult>();
        foreach (var item in model.ParticipantsAdd)
        {
          if (item.TypeParticipantKeyResult == EnumTypeParticipantKeyResult.Team)
          {
            var team = persons.Where(p => p.Manager?._id == item._idPerson).ToList();
            if (team != null)
            {
              foreach (var person in team.Select(p => new ViewListPersonPhotoKeyResult()
              {
                Name = p.User.Name,
                Photo = p.User.PhotoUrl,
                _id = p._id,
                TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Team
              }))
              {
                view.ParticipantsGet.Add(person);
              }
            }

          }
          else
          {
            view.ParticipantsGet.Add(persons.Where(p => p._id == item._idPerson)
              .Select(p => new ViewListPersonPhotoKeyResult()
              {
                Name = p.User.Name,
                Photo = p.User.PhotoUrl,
                _id = p._id,
                TypeParticipantKeyResult = EnumTypeParticipantKeyResult.Single
              })
              .FirstOrDefault());
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
        Calendar calendar = CultureInfo.InvariantCulture.Calendar;
        var week = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        var keyresult = serviceKeyResult.GetNewVersion(p => p._id == view._idKeyResult).Result;

        var old = servicePendingCheckinObjective.GetNewVersion(p => p.Lasted == true && p._idPerson == _user._idPerson
        && p._idKeyResult == view._idKeyResult).Result;
        if (old != null)
        {
          old.Lasted = false;
          var x = servicePendingCheckinObjective.Update(old, null);
        }

        if (DateTime.Now.Day >= 15)
          view.Fortnight = 2;
        else
          view.Fortnight = 1;

        var model = servicePendingCheckinObjective.InsertNewVersion(
          new PendingCheckinObjective()
          {
            _id = view._id,
            _idObjective = view._idObjective,
            _idKeyResult = view._idKeyResult,
            _idPerson = view._idPerson,
            LevelTrust = view.LevelTrust,
            QualityGoal = keyresult.QualityGoal,
            QuantityGoal = keyresult.QuantityResult,
            Date = DateTime.Now,
            Week = week,
            Month = DateTime.Now.Month,
            Lasted = true,
            TypePersonObjective = view.TypePersonObjective,
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
        view.TypeImpedimentsIniciatives = EnumTypeImpedimentsIniciatives.Impediments;
        view.Date = DateTime.Now;
        view._idPerson = _user._idPerson;
        view.NamePerson = _user.NamePerson;
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
            return model.Impediments;
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
        view.TypeImpedimentsIniciatives = EnumTypeImpedimentsIniciatives.Iniciatives;
        view.Date = DateTime.Now;
        view._idPerson = _user._idPerson;
        view.NamePerson = _user.NamePerson;
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

    public string LikeImpediment(string idimpediment, string idkeyresult, bool like)
    {
      try
      {
        var list = servicePendingCheckinObjective.GetAllNewVersion(p => p._idKeyResult == idkeyresult).Result;

        foreach (var model in list)
        {
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
              {
                item.Like.Add(view);
                foreach (var lk in item.Deslike)
                {
                  if (lk._idUser == _user._idUser)
                  {
                    item.Deslike.Remove(lk);
                    item.CountLike = item.Like.Count();
                    item.CountDeslike = item.Deslike.Count();
                    var x = servicePendingCheckinObjective.Update(model, null);
                    return "like";
                  }
                }
                item.CountLike = item.Like.Count();
                item.CountDeslike = item.Deslike.Count();
                var i = servicePendingCheckinObjective.Update(model, null);
                return "like";
              }
              else
              {
                item.Deslike.Add(view);
                foreach (var lk in item.Like)
                {
                  if (lk._idUser == _user._idUser)
                  {
                    item.Like.Remove(lk);
                    item.CountLike = item.Like.Count();
                    item.CountDeslike = item.Deslike.Count();
                    var x = servicePendingCheckinObjective.Update(model, null);
                    return "like";
                  }
                }
                item.CountLike = item.Like.Count();
                item.CountDeslike = item.Deslike.Count();
                var i = servicePendingCheckinObjective.Update(model, null);
                return "like";
              }

            }


          }
        }

        return "like";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteLikeIniciative(string idiniciatives, string idkeyresult, bool like)
    {
      try
      {
        var list = servicePendingCheckinObjective.GetAllNewVersion(p => p._idKeyResult == idkeyresult).Result;

        foreach (var model in list)
        {
          foreach (var item in model.Iniciatives)
          {
            if (item._id == idiniciatives)
            {
              if (like)
              {
                foreach (var lk in item.Like)
                {
                  if (lk._idUser == _user._idUser)
                    item.Like.Remove(lk);
                }
              }
              else
              {
                foreach (var lk in item.Deslike)
                {
                  if (lk._idUser == _user._idUser)
                    item.Deslike.Remove(lk);
                }
              }

            }

            item.CountLike = item.Like.Count();
            item.CountDeslike = item.Deslike.Count();
          }
          var i = servicePendingCheckinObjective.Update(model, null);
        }




        return "remove like";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public string DeleteLikeImpediments(string idimpediments, string idkeyresult, bool like)
    {
      try
      {
        var list = servicePendingCheckinObjective.GetAllNewVersion(p => p._idKeyResult == idkeyresult).Result;

        foreach (var model in list)
        {
          foreach (var item in model.Impediments)
          {
            if (item._id == idimpediments)
            {
              if (like)
              {
                foreach (var lk in item.Like)
                {
                  if (lk._idUser == _user._idUser)
                    item.Like.Remove(lk);
                }
              }
              else
              {
                foreach (var lk in item.Deslike)
                {
                  if (lk._idUser == _user._idUser)
                    item.Deslike.Remove(lk);
                }
              }

            }

            item.CountLike = item.Like.Count();
            item.CountDeslike = item.Deslike.Count();
          }
          var i = servicePendingCheckinObjective.Update(model, null);
        }

        return "remove like";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public string LikeIniciative(string idiniciatives, string idkeyresult, bool like)
    {
      try
      {
        var list = servicePendingCheckinObjective.GetAllNewVersion(p => p._idKeyResult == idkeyresult).Result;

        foreach (var model in list)
        {
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
              {
                item.Like.Add(view);
                foreach (var lk in item.Deslike)
                {
                  if (lk._idUser == _user._idUser)
                  {
                    item.Deslike.Remove(lk);
                    item.CountLike = item.Like.Count();
                    item.CountDeslike = item.Deslike.Count();
                    var x = servicePendingCheckinObjective.Update(model, null);
                    return "like";
                  }
                }
                item.CountLike = item.Like.Count();
                item.CountDeslike = item.Deslike.Count();
                var i = servicePendingCheckinObjective.Update(model, null);
                return "like";
              }
              else
              {
                item.Deslike.Add(view);
                foreach (var lk in item.Like)
                {
                  if (lk._idUser == _user._idUser)
                  {
                    item.Like.Remove(lk);
                    item.CountLike = item.Like.Count();
                    item.CountDeslike = item.Deslike.Count();
                    var x = servicePendingCheckinObjective.Update(model, null);
                    return "like";
                  }
                }
                item.CountLike = item.Like.Count();
                item.CountDeslike = item.Deslike.Count();
                var i = servicePendingCheckinObjective.Update(model, null);
                return "like";
              }

            }


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
        var historys = servicePendingCheckinObjective.GetAllNewVersion(p => p._idKeyResult == model._idKeyResult).Result;

        model._idObjective = view._idObjective;
        model._idKeyResult = view._idKeyResult;
        model._idPerson = view._idPerson;
        model.LevelTrust = view.LevelTrust;
        model.Date = DateTime.Now;
        model.Achievement = view.Achievement;
        model.QualityResult = view.QualityResult;
        model.QuantityResult = view.QuantityResult;
        model.TypePersonObjective = view.TypePersonObjective;

        var histleveltrust = historys.Where(p => p.LevelTrust > 0);
        var hisachievement = historys.Where(p => p.Achievement > 0);

        var result = model.GetViewCrud();
        if (histleveltrust.Count() > 0)
          result.HistoryLevelTrust = histleveltrust.Select(p => new ViewListHistoryLevelTrust()
          {
            Date = p.Date,
            LevelTrust = p.LevelTrust
          }).ToList();

        if (hisachievement.Count() > 0)
          result.HistoryAchievement = hisachievement.Select(p => new ViewListHistoryAchievement()
          {
            Date = p.Date,
            Achievement = p.Achievement,
            QuanlityResult = p.QualityResult,
            QuantityResult = p.QuantityResult
          }).ToList();

        servicePendingCheckinObjective.Update(model, null).Wait();
        return result;
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
        var view = servicePendingCheckinObjective.GetNewVersion(p => p._id == id).Result.GetViewCrud();

        var historys = servicePendingCheckinObjective.GetAllNewVersion(p => p._idKeyResult == view._idKeyResult).Result;

        var objective = serviceObjective.GetNewVersion(p => p._id == view._idObjective).Result;
        if ((objective.Responsible._id == _user._idPerson) || (objective.Editors.Where(p => p._id == _user._idPerson).Count() > 0))
          view.TypePersonObjective = EnumTypePersonObjective.Responsible;
        else
          view.TypePersonObjective = EnumTypePersonObjective.Participant;

        var histleveltrust = historys.Where(p => p.LevelTrust > 0);
        var hisachievement = historys.Where(p => p.Achievement > 0);

        if (histleveltrust.Count() > 0)
          view.HistoryLevelTrust = histleveltrust.Select(p => new ViewListHistoryLevelTrust()
          {
            Date = p.Date,
            LevelTrust = p.LevelTrust
          }).OrderByDescending(p => p.Date).ToList();

        if (hisachievement.Count() > 0)
          view.HistoryAchievement = hisachievement.Select(p => new ViewListHistoryAchievement()
          {
            Date = p.Date,
            Achievement = p.Achievement,
            QuanlityResult = p.QualityResult,
            QuantityResult = p.QuantityResult
          }).OrderByDescending(p => p.Date).ToList();

        return view;
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

    public string ImportObjectiveModel1(Stream stream)
    {
      try
      {
        var list = serviceExcel.ImportObjectiveModel1(stream);
        var keyresults = serviceKeyResult.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        foreach (var item in list)
        {
          var listkeyresult = keyresults.Where(p => p.Name == item.Name).ToList();
          foreach(var keyresult in listkeyresult)
          {
            if (keyresult != null)
            {
              var checkin = NewPendingCheckinObjective(new ViewCrudPendingCheckinObjective()
              {
                _idKeyResult = keyresult._id,
                _idObjective = keyresult.Objective._id,
                _idPerson = _user._idPerson
              });

              var viewtext = new ViewText() { Text = "" };
              if (item.Type == EnumTypeKeyResult.Quantity)
              {
                UpdateResultKeyResult(keyresult._id, checkin._id, 0, decimal.Parse(item.Result), viewtext);
              }
              else
              {
                viewtext.Text = item.Result;
                UpdateResultKeyResult(keyresult._id, checkin._id, decimal.Parse(item.Achievment.Replace("%", "")), 0, viewtext);
              }
            }
          }
        }
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

  }
}
