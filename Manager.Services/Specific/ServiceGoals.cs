using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
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

namespace Manager.Services.Specific
{
  public class ServiceGoals : Repository<Goals>, IServiceGoals
  {
    private readonly ServiceGeneric<Goals> serviceGoals;
    private readonly ServiceGeneric<GoalsPeriod> serviceGoalsPeriod;
    private readonly ServiceGeneric<GoalsCompany> serviceGoalsCompany;
    private readonly ServiceGeneric<Person> servicePerson;

    #region Constructor
    public ServiceGoals(DataContext context) : base(context)
    {
      try
      {
        serviceGoals = new ServiceGeneric<Goals>(context);
        serviceGoalsCompany = new ServiceGeneric<GoalsCompany>(context);
        serviceGoalsPeriod = new ServiceGeneric<GoalsPeriod>(context);
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
      serviceGoals._user = _user;
      serviceGoalsCompany._user = _user;
      serviceGoalsPeriod._user = _user;
      servicePerson._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      _user = baseUser;
      serviceGoals._user = baseUser;
      serviceGoalsCompany._user = baseUser;
      serviceGoalsPeriod._user = baseUser;
      servicePerson._user = baseUser;
    }
    #endregion

    #region Goals
    public string New(ViewCrudGoal view)
    {
      try
      {
        Goals goal = new Goals()
        {
          Name = view.Name,
          Concept = view.Concept,
          TypeGoals = view.TypeGoals
        };
        goal = serviceGoals.InsertNewVersion(goal).Result;
        return "Goal added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudGoal view)
    {
      try
      {
        Goals goal = serviceGoals.GetNewVersion(p => p._id == view._id).Result;
        if (goal == null)
          throw new Exception("Goal not available!");
        goal.Name = view.Name;
        goal.Concept = view.Concept;
        goal.TypeGoals = view.TypeGoals;
        goal = serviceGoals.InsertNewVersion(goal).Result;
        return "Goal altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Delete(string id)
    {
      try
      {
        Goals item = serviceGoals.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceGoals.Update(item, null);
        return "Goal deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudGoal Get(string id)
    {
      try
      {
        var goal = serviceGoals.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudGoal()
        {
          _id = goal._id,
          Name = goal.Name,
          Concept = goal.Concept,
          TypeGoals = goal.TypeGoals
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListGoal> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListGoal> detail = serviceGoals.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewListGoal()
          {
            _id = p._id,
            Name = p.Name
          }).ToList();
        total = serviceGoals.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Period Goals
    public string NewGoalsPeriod(ViewCrudGoalPeriod view)
    {
      try
      {
        GoalsPeriod goalsPeriod = new GoalsPeriod()
        {
          Review = view.Review,
          Name = view.Name,
          ChangeCheck = view.ChangeCheck,
          DateBegin = view.DateBegin,
          DateEnd = view.DateEnd
        };
        goalsPeriod = serviceGoalsPeriod.InsertNewVersion(goalsPeriod).Result;
        return "Period goals added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateGoalsPeriod(ViewCrudGoalPeriod view)
    {
      try
      {
        GoalsPeriod goalsPeriod = serviceGoalsPeriod.GetNewVersion(p => p._id == view._id).Result;
        goalsPeriod.Review = view.Review;
        goalsPeriod.Name = view.Name;
        goalsPeriod.ChangeCheck = view.ChangeCheck;
        goalsPeriod.DateBegin = view.DateBegin;
        goalsPeriod.DateEnd = view.DateEnd;
        serviceGoalsPeriod.Update(goalsPeriod, null);
        return "Period goals altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string DeleteGoalsPeriod(string id)
    {
      try
      {
        GoalsPeriod item = serviceGoalsPeriod.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceGoalsPeriod.Update(item, null);
        return "Period goals deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudGoalPeriod GetGoalsPeriod(string id)
    {
      try
      {
        GoalsPeriod goalsPeriod =  serviceGoalsPeriod.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudGoalPeriod()
        {
          _id = goalsPeriod._id,
          Review = goalsPeriod.Review,
          Name = goalsPeriod.Name,
          ChangeCheck = goalsPeriod.ChangeCheck,
          DateBegin = goalsPeriod.DateBegin,
          DateEnd = goalsPeriod.DateEnd
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewCrudGoalPeriod> ListGoalsPeriod(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewCrudGoalPeriod> detail = serviceGoalsPeriod.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewCrudGoalPeriod()
          {
            _id = p._id,
            Review = p.Review,
            Name = p.Name,
            ChangeCheck = p.ChangeCheck,
            DateBegin = p.DateBegin,
            DateEnd = p.DateEnd
          }).ToList();

        total = serviceGoalsPeriod.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Company Goals
    public string NewGoalsCompany(ViewCrudGoalCompany view)
    {
      try
      {
        GoalsCompany goalsCompany = new GoalsCompany()
        {
          GoalsPeriod = view.GoalsPeriod,
          Company = view.Company,
          GoalsCompanyList = view.GoalsCompanyList?.Select(p => new GoalsCompanyItem()
          {
            Weight = p.Weight,
            Achievement = p.Achievement,
            Deadline = p.Deadline,
            Goal = p.Goal,
            Goals = p.Goals,
            Realized = p.Realized,
            Result = p.Result
          }).ToList()
        };
        goalsCompany = serviceGoalsCompany.InsertNewVersion(goalsCompany).Result;
        return "Company goal added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateGoalsCompany(ViewCrudGoalCompany view)
    {
      try
      {
        GoalsCompany goalsCompany = serviceGoalsCompany.GetNewVersion(p => p._id == view._id).Result;
        goalsCompany.GoalsPeriod = view.GoalsPeriod;
        goalsCompany.Company = view.Company;
        goalsCompany.GoalsCompanyList = view.GoalsCompanyList?.Select(p => new GoalsCompanyItem()
        {
          Weight = p.Weight,
          Achievement = p.Achievement,
          Deadline = p.Deadline,
          Goal = p.Goal,
          Goals = p.Goals,
          Realized = p.Realized,
          Result = p.Result
        }).ToList();
        serviceGoalsCompany.Update(goalsCompany, null);
        return "Company goal altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string DeleteGoalsCompany(string id)
    {
      try
      {
        GoalsCompany goalsCompany = serviceGoalsCompany.GetNewVersion(p => p._id == id).Result;
        goalsCompany.Status = EnumStatus.Disabled;
        serviceGoalsCompany.Update(goalsCompany, null);
        return "Company goal deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudGoalCompany GetGoalsCompany(string id)
    {
      try
      {
        GoalsCompany goalsCompany = serviceGoalsCompany.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudGoalCompany()
        {
          GoalsPeriod = goalsCompany.GoalsPeriod,
          Company = goalsCompany.Company,
          GoalsCompanyList = goalsCompany.GoalsCompanyList?.Select(p => new ViewCrudGoalCompanyItem()
          {
            Weight = p.Weight,
            Achievement = p.Achievement,
            Deadline = p.Deadline,
            Goal = p.Goal,
            Goals = p.Goals,
            Realized = p.Realized,
            Result = p.Result
          }).ToList()
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListGoalCompany> ListGoalsCompany(string idGoalsPeriod, string idCompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListGoalCompany> detail = serviceGoalsCompany.GetAllNewVersion(p => p.GoalsPeriod._id == idGoalsPeriod
                  && p.Company._id == idCompany && p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper()),count, count * (page - 1), "Company.Name").Result
          .Select(p => new ViewListGoalCompany()
          {
            _id = p._id,
            Company = p.Company,
            GoalsPeriod = p.GoalsPeriod
          }).ToList();

        total = serviceGoalsCompany.CountNewVersion(p => p.GoalsPeriod._id == idGoalsPeriod && p.Company._id == idCompany && p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListGoalCompany> ListGoalsCompany(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListGoalCompany> detail = serviceGoalsCompany.GetAllNewVersion(p => p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Company.Name").Result
          .Select(p => new ViewListGoalCompany()
          {
            _id = p._id,
            Company = p.Company,
            GoalsPeriod = p.GoalsPeriod
          }).ToList();
        total = serviceGoalsCompany.CountNewVersion(p => p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper())).Result;
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
