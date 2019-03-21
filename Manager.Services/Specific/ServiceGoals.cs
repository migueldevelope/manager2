using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Services.Specific
{
  public class ServiceGoals : Repository<Goals>, IServiceGoals
  {
    private readonly ServiceGeneric<Goals> goalsService;
    private readonly ServiceGeneric<GoalsPeriod> goalsPeriodService;
    private readonly ServiceGeneric<GoalsCompany> goalsCompanyService;
    private readonly ServiceGeneric<Person> personService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceGoals(DataContext context)
      : base(context)
    {
      try
      {
        goalsService = new ServiceGeneric<Goals>(context);
        personService = new ServiceGeneric<Person>(context);
        goalsPeriodService = new ServiceGeneric<GoalsPeriod>(context);
        goalsCompanyService = new ServiceGeneric<GoalsCompany>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }


    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      goalsService._user = _user;
      personService._user = _user;
      goalsPeriodService._user = _user;
      goalsCompanyService._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      _user = baseUser;
      goalsService._user = baseUser;
      personService._user = baseUser;
      goalsPeriodService._user = baseUser;
      goalsCompanyService._user = baseUser;
    }

    public string New(Goals view)
    {
      try
      {
        goalsService.Insert(view);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Update(Goals view)
    {
      try
      {
        goalsService.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Remove(string id)
    {
      try
      {
        var item = goalsService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        goalsService.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Goals Get(string id)
    {
      try
      {
        return goalsService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Goals> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = goalsService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = goalsService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewGoalsPeriod(GoalsPeriod view)
    {
      try
      {
        goalsPeriodService.Insert(view);

        //var grades = gradeService.GetAll(p => p.Goals._id == view.Goals._id).ToList();
        //foreach (var grade in grades)
        //{
        //  var list = new List<ListSteps>();
        //  for (var step = 0; step <= 7; step++)
        //  {
        //    list.Add(new ListSteps()
        //    {
        //      _id = ObjectId.GenerateNewId().ToString(),
        //      _idAccount = _user._idAccount,
        //      Status = EnumStatus.Enabled,
        //      Salary = 0,
        //      Step = (EnumSteps)step,
        //    });
        //  }

        //  var item = new SalaryScale()
        //  {
        //    Grade = grade,
        //    GoalsPeriod = view,
        //    ListSteps = list
        //  };
        //  salaryScaleService.Insert(item);

        //}

        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateGoalsPeriod(GoalsPeriod view)
    {
      try
      {
        goalsPeriodService.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveGoalsPeriod(string id)
    {
      try
      {
        var item = goalsPeriodService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        goalsPeriodService.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public GoalsPeriod GetGoalsPeriod(string id)
    {
      try
      {
        return goalsPeriodService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<GoalsPeriod> ListGoalsPeriod(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        try
        {
          int skip = (count * (page - 1));
          var detail = goalsPeriodService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = goalsPeriodService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

          return detail.ToList();
        }
        catch (Exception e)
        {
          throw new ServiceException(_user, e, this._context);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public string NewGoalsCompany(GoalsCompany view)
    {
      try
      {
        goalsCompanyService.Insert(view);

        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateGoalsCompany(GoalsCompany view)
    {
      try
      {
        goalsCompanyService.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveGoalsCompany(string id)
    {
      try
      {
        var item = goalsCompanyService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        goalsCompanyService.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public GoalsCompany GetGoalsCompany(string id)
    {
      try
      {
        return goalsCompanyService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<GoalsCompany> ListGoalsCompany(string idgoalsperiod, string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        try
        {
          int skip = (count * (page - 1));
          var detail = goalsCompanyService.GetAll(p => p.GoalsPeriod._id == idgoalsperiod & p.Company._id == idcompany & p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.GoalsPeriod.Name).Skip(skip).Take(count).ToList();
          total = goalsCompanyService.GetAll(p => p.GoalsPeriod._id == idgoalsperiod & p.Company._id == idcompany & p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper())).Count();

          return detail.ToList();
        }
        catch (Exception e)
        {
          throw new ServiceException(_user, e, this._context);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<GoalsCompany> ListGoalsCompany(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        try
        {
          int skip = (count * (page - 1));
          var detail = goalsCompanyService.GetAll(p => p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.GoalsPeriod.Name).Skip(skip).Take(count).ToList();
          total = goalsCompanyService.GetAll(p => p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper())).Count();

          return detail.ToList();
        }
        catch (Exception e)
        {
          throw new ServiceException(_user, e, this._context);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
}
