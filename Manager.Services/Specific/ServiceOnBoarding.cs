using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Services.Specific
{
  public class ServiceOnBoarding : Repository<Account>, IServiceOnBoarding
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

        int skip = (count * (page - 1));
        var detail = onBoardingService.GetAll(p => p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<OnBoarding> ListOnBoardingsWait(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {

        int skip = (count * (page - 1));
        var detail = (from person in personService.GetAll()
                      join onboard in onBoardingService.GetAll() on person._id equals onboard._id into onboardleft
                      from onboarding in onboardleft.DefaultIfEmpty(
                        new OnBoarding()
                        {
                          Person = person,
                          _id = null,
                          StatusOnBoarding = EnumStatusOnBoarding.Open
                        })
                      select onboarding);

        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();
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
          onBoarding.SkillsCompany.Add(new OnBoardingSkills() { Skill = item });
        }

        onBoarding.SkillsGroup = new List<OnBoardingSkills>();
        foreach (var item in onBoarding.Person.Occupation.Group.Skills)
        {
          onBoarding.SkillsGroup.Add(new OnBoardingSkills() { Skill = item });
        }

        onBoarding.SkillsOccupation = new List<OnBoardingSkills>();
        foreach (var item in onBoarding.Person.Occupation.Skills)
        {
          onBoarding.SkillsOccupation.Add(new OnBoardingSkills() { Skill = item });
        }

        onBoarding.Scopes = new List<OnBoardingScope>();
        foreach (var item in onBoarding.Person.Occupation.Group.Scope)
        {
          onBoarding.Scopes.Add(new OnBoardingScope() { Scope = item });
        }

        onBoarding.Activities = new List<OnBoardingActivities>();
        foreach (var item in onBoarding.Person.Occupation.Activities)
        {
          onBoarding.Activities.Add(new OnBoardingActivities() { Activitie = item });
        }

        onBoarding.Schoolings = new List<OnBoardingSchooling>();
        foreach (var item in onBoarding.Person.Occupation.Schooling)
        {
          onBoarding.Schoolings.Add(new OnBoardingSchooling() { Schooling = item });
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

    public string UpdateOnBoarding(OnBoarding onboarding)
    {
      try
      {
        onBoardingService.Update(onboarding, null);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
  }
}
