using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Services.Specific
{
  public class ServiceOnBoarding : Repository<OnBoarding>, IServiceOnBoarding
  {
    private readonly ServiceGeneric<OnBoarding> onBoardingService;
    private readonly ServiceGeneric<Person> personService;

    public ServiceOnBoarding(DataContext context)
      : base(context)
    {
      try
      {
        onBoardingService = new ServiceGeneric<OnBoarding>(context);
        personService = new ServiceGeneric<Person>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<OnBoarding> ListOnBoardingsEnd(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        var manager = personService.GetAll(p => p._id == idmanager).FirstOrDefault();
        int skip = (count * (page - 1));
        var detail = onBoardingService.GetAll(p => p.Person.Manager == manager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private void newOnZero()
    {
      try
      {
        var on = onBoardingService.GetAuthentication(p => p.Status == EnumStatus.Disabled).Count();
        if (on == 0)
        {
          var person = personService.GetAll().FirstOrDefault();
          var zero = onBoardingService.Insert(new OnBoarding() { Person = person, Status = EnumStatus.Disabled, StatusOnBoarding = EnumStatusOnBoarding.End });
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public List<OnBoarding> ListOnBoardingsWait(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        newOnZero();
        int skip = (count * (page - 1));
        var list = personService.GetAll(p => p.Manager._id == idmanager
        & p.Name.ToUpper().Contains(filter.ToUpper()))
        .ToList().Select(p => new { Person = p, OnBoarding = onBoardingService.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
        .ToList();

        var detail = new List<OnBoarding>();
        foreach (var item in list)
        {
          if (item.OnBoarding == null)
            detail.Add(new OnBoarding
            {
              Person = item.Person,
              _id = null,
              StatusOnBoarding = EnumStatusOnBoarding.Open
            });
          else
            if (item.OnBoarding.StatusOnBoarding != EnumStatusOnBoarding.End)
            detail.Add(item.OnBoarding);
        }

        total = detail.Count();
        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public OnBoarding PersonOnBoardingsEnd(string idmanager)
    {
      try
      {
        return onBoardingService.GetAll(p => p.Person._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public OnBoarding PersonOnBoardingsWait(string idmanager)
    {
      try
      {
        var item = personService.GetAll(p => p._id == idmanager)
        .ToList().Select(p => new { Person = p, OnBoarding = onBoardingService.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
        .FirstOrDefault();

        if (item.OnBoarding == null)
          return new OnBoarding
          {
            Person = item.Person,
            _id = null,
            StatusOnBoarding = EnumStatusOnBoarding.Open
          };
        else
         if (item.OnBoarding.StatusOnBoarding != EnumStatusOnBoarding.End)
          return item.OnBoarding;
         else
          return null;


      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public OnBoarding GetOnBoardings(string id)
    {
      try
      {
        return onBoardingService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private OnBoarding loadMap(OnBoarding onBoarding)
    {
      try
      {
        onBoarding.SkillsCompany = new List<OnBoardingSkills>();
        foreach (var item in onBoarding.Person.Company.Skills)
        {
          onBoarding.SkillsCompany.Add(new OnBoardingSkills() { Skill = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString() });
        }

        onBoarding.SkillsGroup = new List<OnBoardingSkills>();
        foreach (var item in onBoarding.Person.Occupation.Group.Skills)
        {
          onBoarding.SkillsGroup.Add(new OnBoardingSkills() { Skill = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString() });
        }

        onBoarding.SkillsOccupation = new List<OnBoardingSkills>();
        foreach (var item in onBoarding.Person.Occupation.Skills)
        {
          onBoarding.SkillsOccupation.Add(new OnBoardingSkills() { Skill = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString() });
        }

        onBoarding.Scopes = new List<OnBoardingScope>();
        foreach (var item in onBoarding.Person.Occupation.Group.Scope)
        {
          onBoarding.Scopes.Add(new OnBoardingScope() { Scope = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString() });
        }

        onBoarding.Activities = new List<OnBoardingActivities>();
        foreach (var item in onBoarding.Person.Occupation.Activities)
        {
          onBoarding.Activities.Add(new OnBoardingActivities() { Activitie = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString() });
        }

        onBoarding.Schoolings = new List<OnBoardingSchooling>();
        foreach (var item in onBoarding.Person.Occupation.Schooling)
        {
          onBoarding.Schoolings.Add(new OnBoardingSchooling() { Schooling = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString() });
        }

        return onBoarding;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public OnBoarding NewOnBoarding(OnBoarding onBoarding, string idperson)
    {
      try
      {
        if (onBoarding._id == null)
        {
          loadMap(onBoarding);

          if (onBoarding.Person._id == idperson)
          {
            onBoarding.DateBeginPerson = DateTime.Now;
            onBoarding.StatusOnBoarding = EnumStatusOnBoarding.InProgressPerson;
          }
          else
          {
            onBoarding.DateBeginManager = DateTime.Now;
            onBoarding.StatusOnBoarding = EnumStatusOnBoarding.InProgressManager;
          }

          onBoardingService.Insert(onBoarding);
        }
        else
        {
          if (onBoarding.StatusOnBoarding == EnumStatusOnBoarding.Wait)
          {
            onBoarding.DateBeginEnd = DateTime.Now;
          }
          onBoardingService.Update(onBoarding, null);
        }

        return onBoarding;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateOnBoarding(OnBoarding onboarding, string idperson)
    {
      try
      {
        if (onboarding.Person._id != idperson)
        {
          if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.Wait)
          {
            onboarding.DateEndManager = DateTime.Now;
          }
        }
        else
        {
          if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.End)
          {
            onboarding.DateEndEnd = DateTime.Now;
          }
          else if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.WaitManager)
          {
            onboarding.DateEndPerson = DateTime.Now;
          }

        }
        onBoardingService.Update(onboarding, null);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      personService._user = _user;
      onBoardingService._user = _user;
    }

  }
}
